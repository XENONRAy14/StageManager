using System;

namespace StageManager
{
    // Classe qui représente un stage avec toutes ses informations associées
    // Utilisée pour stocker et manipuler les données des stages dans l'application
    public class Stage
    {
        // Informations de l'étudiant
        // Ces propriétés stockent les données personnelles de l'étudiant qui effectue le stage
        public string NomEtudiant { get; set; }      // Nom de famille de l'étudiant
        public string PrenomEtudiant { get; set; }   // Prénom de l'étudiant
        public string Classe { get; set; }          // Classe actuelle (ex: "BTS SIO 2")
        public string Annee { get; set; }           // Année scolaire (ex: "2024-2025")
        public string Periode { get; set; }         // Période du stage (ex: "Mai-Juin 2025")
        public string SuiviPar { get; set; }        // Personne qui suit l'étudiant côté école
        public string TelephoneEtudiant { get; set; } // Numéro de téléphone de l'étudiant
        public string EmailEtudiant { get; set; }    // Adresse email de l'étudiant

        // Informations de l'entreprise
        // Ces propriétés décrivent l'entreprise qui accueille l'étudiant en stage
        public string RaisonSociale { get; set; }       // Nom officiel de l'entreprise
        public string AdresseOrganisation { get; set; } // Adresse postale
        public string CodePostal { get; set; }          // Code postal
        public string Ville { get; set; }               // Ville
        public bool PaysHorsFrance { get; set; }        // Indique si le stage est à l'étranger
        public string TelephoneOrganisation { get; set; } // Numéro de téléphone de l'entreprise
        public string SiteWeb { get; set; }             // Site web de l'entreprise

        // Informations du contact principal
        // Personne à contacter en priorité dans l'entreprise
        public string NomContact { get; set; }         // Nom du contact
        public string PrenomContact { get; set; }      // Prénom du contact
        public string CiviliteContact { get; set; }    // Civilité (M., Mme, etc.)
        public string TelephoneContact { get; set; }   // Numéro de téléphone
        public string EmailContact { get; set; }       // Adresse email

        // Informations du tuteur
        // Personne qui encadre directement l'étudiant pendant son stage
        public string NomTuteur { get; set; }         // Nom du tuteur
        public string PrenomTuteur { get; set; }      // Prénom du tuteur
        public string CiviliteTuteur { get; set; }    // Civilité (M., Mme, etc.)
        public string TelephoneTuteur { get; set; }   // Numéro de téléphone
        public string EmailTuteur { get; set; }       // Adresse email

        // Informations du responsable
        // Responsable hiérarchique ou administratif du stage dans l'entreprise
        public string NomResponsable { get; set; }         // Nom du responsable
        public string PrenomResponsable { get; set; }      // Prénom du responsable
        public string CiviliteResponsable { get; set; }    // Civilité (M., Mme, etc.)
        public string TelephoneResponsable { get; set; }   // Numéro de téléphone
        public string EmailResponsable { get; set; }       // Adresse email

        // Description des activités
        // Détails sur les tâches et missions confiées à l'étudiant
        public string DescriptionActivites { get; set; }

        // Identifiant unique pour le stage
        // Généré automatiquement avec un GUID (Globally Unique Identifier)
        // pour garantir l'unicité de chaque stage dans la base de données
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
