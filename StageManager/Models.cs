using System;
using Newtonsoft.Json;

namespace StageManager
{
    /// <summary>
    /// Représente une école dans le système
    /// </summary>
    public class School
    {
        /// <summary>
        /// Identifiant unique de l'école
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nom de l'école
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ville où se trouve l'école
        /// </summary>
        public string City { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(City) ? Name : $"{Name} - {City}";
        }
    }

    /// <summary>
    /// Représente un étudiant dans le système.
    /// Cette classe correspond exactement à la structure des données dans Firebase.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Identifiant unique de l'étudiant dans Firebase
        /// </summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Année scolaire de l'étudiant (ex: "2023")
        /// </summary>
        [JsonProperty("Annee")]
        public string Annee { get; set; }

        /// <summary>
        /// Classe actuelle de l'étudiant (ex: "BTS SIO 2")
        /// </summary>
        [JsonProperty("Classe")]
        public string Classe { get; set; }

        /// <summary>
        /// Email du contact principal
        /// </summary>
        [JsonProperty("EmailContact")]
        public string EmailContact { get; set; }

        /// <summary>
        /// Nom de famille de l'étudiant
        /// </summary>
        [JsonProperty("NomEtudiant")]
        public string NomEtudiant { get; set; }

        /// <summary>
        /// Prénom de l'étudiant
        /// </summary>
        [JsonProperty("PrenomEtudiant")]
        public string PrenomEtudiant { get; set; }

        /// <summary>
        /// Prénom du contact principal
        /// </summary>
        [JsonProperty("PrenomContact")]
        public string PrenomContact { get; set; }

        /// <summary>
        /// Numéro de téléphone du contact
        /// </summary>
        [JsonProperty("TelephoneContact")]
        public string TelephoneContact { get; set; }

        /// <summary>
        /// Ville de résidence de l'étudiant
        /// </summary>
        [JsonProperty("Ville")]
        public string Ville { get; set; }

        /// <summary>
        /// Période de stage souhaitée
        /// </summary>
        [JsonProperty("Periode")]
        public string Periode { get; set; }

        /// <summary>
        /// Indique si l'étudiant souhaite faire son stage hors de France
        /// </summary>
        [JsonProperty("PaysHorsFrance")]
        public bool PaysHorsFrance { get; set; }

        /// <summary>
        /// Raison sociale / Spécialité de l'étudiant
        /// </summary>
        [JsonProperty("RaisonSociale")]
        public string RaisonSociale { get; set; }

        /// <summary>
        /// Retourne une représentation lisible de l'étudiant
        /// </summary>
        public override string ToString()
        {
            return $"{PrenomEtudiant} {NomEtudiant} - {Classe}";
        }
    }
}
