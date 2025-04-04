using System;

namespace StageManager
{
    public class Stage
    {
        // Informations de l'étudiant
        public string NomEtudiant { get; set; }
        public string PrenomEtudiant { get; set; }
        public string Classe { get; set; }
        public string Annee { get; set; }
        public string Periode { get; set; }
        public string SuiviPar { get; set; }
        public string TelephoneEtudiant { get; set; }
        public string EmailEtudiant { get; set; }

        // Informations de l'entreprise
        public string RaisonSociale { get; set; }
        public string AdresseOrganisation { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public bool PaysHorsFrance { get; set; }
        public string TelephoneOrganisation { get; set; }
        public string SiteWeb { get; set; }

        // Informations du contact principal
        public string NomContact { get; set; }
        public string PrenomContact { get; set; }
        public string CiviliteContact { get; set; }
        public string TelephoneContact { get; set; }
        public string EmailContact { get; set; }

        // Informations du tuteur
        public string NomTuteur { get; set; }
        public string PrenomTuteur { get; set; }
        public string CiviliteTuteur { get; set; }
        public string TelephoneTuteur { get; set; }
        public string EmailTuteur { get; set; }

        // Informations du responsable
        public string NomResponsable { get; set; }
        public string PrenomResponsable { get; set; }
        public string CiviliteResponsable { get; set; }
        public string TelephoneResponsable { get; set; }
        public string EmailResponsable { get; set; }

        // Description des activités
        public string DescriptionActivites { get; set; }

        // Identifiant unique pour le stage
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
