using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using StageManager;

namespace StageManager
{
    public static class ExcelImporter
    {
        public static async Task ImportFromExcel(string filePath, School school)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    // Vérifier si le fichier a des feuilles
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        MessageBox.Show("Le fichier Excel est vide ou corrompu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Obtenir la première feuille (EPPlus utilise l'index 1)
                    var worksheet = package.Workbook.Worksheets[1];

                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        MessageBox.Show("La feuille Excel est vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    MessageBox.Show($"Feuille trouvée avec {rowCount} lignes et {colCount} colonnes");

                    if (rowCount < 2)
                    {
                        MessageBox.Show("Le fichier doit contenir au moins une ligne d'en-tête et une ligne de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int importCount = 0;

                    // Commencer à la ligne 2 (après les en-têtes)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            string raisonSociale = worksheet.Cells[row, 4].Text?.Trim() ?? "";
                            
                            // Si la ligne n'a pas de raison sociale, on la saute
                            if (string.IsNullOrWhiteSpace(raisonSociale))
                            {
                                continue;
                            }

                            var stage = new Stage
                            {
                                Id = Guid.NewGuid().ToString(),
                                NomEtudiant = worksheet.Cells[row, 1].Text?.Trim() ?? "",
                                PrenomEtudiant = worksheet.Cells[row, 2].Text?.Trim() ?? "",
                                Classe = worksheet.Cells[row, 3].Text?.Trim() ?? "",
                                RaisonSociale = raisonSociale,
                                Ville = worksheet.Cells[row, 5].Text?.Trim() ?? "",
                                PrenomContact = worksheet.Cells[row, 6].Text?.Trim() ?? "",
                                TelephoneContact = worksheet.Cells[row, 7].Text?.Trim() ?? "",
                                EmailContact = worksheet.Cells[row, 8].Text?.Trim() ?? "",
                                Periode = worksheet.Cells[row, 9].Text?.Trim() ?? "",
                                Annee = worksheet.Cells[row, 10].Text?.Trim() ?? ""
                            };

                            var response = await FirebaseManager.Client.SetAsync($"students/{school.Id}/{stage.Id}", stage);
                            if (response != null)
                            {
                                importCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erreur à la ligne {row} : {ex.Message}", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    MessageBox.Show($"{importCount} stages ont été importés avec succès.", "Import terminé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'import : {ex.Message}\n\nDétails : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
