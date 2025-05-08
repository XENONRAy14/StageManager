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
}
