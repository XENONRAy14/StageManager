using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using StageManager;

namespace StageManager
{
    /// <summary>
    /// Classe utilitaire pour importer des données depuis des fichiers Excel
    /// </summary>
    public static class ExcelImporter
    {
        /// <summary>
        /// Importe des données d'étudiants depuis un fichier Excel et les enregistre dans Firebase
        /// </summary>
        /// <param name="filePath">Chemin complet vers le fichier Excel à importer</param>
        /// <param name="school">L'école à laquelle associer les étudiants importés</param>
        /// <returns>Tâche asynchrone</returns>
        public static async Task ImportFromExcel(string filePath, School school)
        {
            try
            {
                // Crée un package Excel à partir du fichier sélectionné
                // Le bloc "using" garantit que les ressources sont libérées après utilisation
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    // Vérifier si le fichier contient au moins une feuille de calcul
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        MessageBox.Show("Le fichier Excel est vide ou corrompu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Obtenir la première feuille (EPPlus utilise l'index 1, pas 0 comme la plupart des collections C#)
                    var worksheet = package.Workbook.Worksheets[1];

                    // Vérifier si la feuille est vide
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        MessageBox.Show("La feuille Excel est vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Récupérer le nombre de lignes et de colonnes dans la feuille
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Afficher les dimensions de la feuille pour information
                    MessageBox.Show($"Feuille trouvée avec {rowCount} lignes et {colCount} colonnes");

                    // Vérifier qu'il y a au moins une ligne d'en-tête et une ligne de données
                    if (rowCount < 2)
                    {
                        MessageBox.Show("Le fichier doit contenir au moins une ligne d'en-tête et une ligne de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Compteur pour suivre le nombre d'étudiants importés avec succès
                    int importCount = 0;

                    // Parcourir chaque ligne du fichier Excel, en commençant à la ligne 2 (après l'en-tête)
                    // La ligne 1 est supposée contenir les en-têtes des colonnes
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            // Récupérer la raison sociale (colonne 4)
                            // Utilisation de l'opérateur de fusion null (??) pour gérer les cellules vides
                            string raisonSociale = worksheet.Cells[row, 4].Text?.Trim() ?? "";
                            
                            // Si la raison sociale est vide, on saute cette ligne
                            // C'est un contrôle de validation basique pour éviter d'importer des lignes incomplètes
                            if (string.IsNullOrWhiteSpace(raisonSociale))
                            {
                                continue;
                            }

                            // Créer un nouvel objet Stage avec les données de la ligne Excel
                            var stage = new Stage
                            {
                                // Générer un identifiant unique pour ce stage
                                Id = Guid.NewGuid().ToString(),
                                // Colonnes 1 à 10 : données de base de l'étudiant et du stage
                                NomEtudiant = worksheet.Cells[row, 1].Text?.Trim() ?? "",      // Colonne 1: Nom
                                PrenomEtudiant = worksheet.Cells[row, 2].Text?.Trim() ?? "",   // Colonne 2: Prénom
                                Classe = worksheet.Cells[row, 3].Text?.Trim() ?? "",          // Colonne 3: Classe
                                RaisonSociale = raisonSociale,                                 // Colonne 4: Entreprise
                                Ville = worksheet.Cells[row, 5].Text?.Trim() ?? "",           // Colonne 5: Ville
                                PrenomContact = worksheet.Cells[row, 6].Text?.Trim() ?? "",    // Colonne 6: Contact
                                TelephoneContact = worksheet.Cells[row, 7].Text?.Trim() ?? "", // Colonne 7: Téléphone
                                EmailContact = worksheet.Cells[row, 8].Text?.Trim() ?? "",     // Colonne 8: Email
                                Periode = worksheet.Cells[row, 9].Text?.Trim() ?? "",         // Colonne 9: Période
                                Annee = worksheet.Cells[row, 10].Text?.Trim() ?? ""            // Colonne 10: Année
                            };

                            // Enregistrer l'objet Stage dans Firebase
                            // Le chemin inclut l'ID de l'école et l'ID unique du stage
                            var response = await FirebaseManager.Client.SetAsync($"students/{school.Id}/{stage.Id}", stage);
                            // Si la réponse n'est pas null, l'enregistrement a réussi
                            if (response != null)
                            {
                                importCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Afficher un avertissement en cas d'erreur sur une ligne spécifique
                            // mais continuer l'importation des autres lignes
                            MessageBox.Show($"Erreur à la ligne {row} : {ex.Message}", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    // Afficher un message de confirmation avec le nombre d'étudiants importés
                    MessageBox.Show($"{importCount} stages ont été importés avec succès.", "Import terminé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur générale, afficher un message d'erreur détaillé
                MessageBox.Show($"Erreur lors de l'import : {ex.Message}\n\nDétails : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
