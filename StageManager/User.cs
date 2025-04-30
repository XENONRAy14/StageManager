using System;

namespace StageManager
{
    // Classe qui représente un utilisateur du système (entreprise ou administrateur)
    public class User
    {
        // Adresse email de l'utilisateur, sert d'identifiant unique
        public string Email { get; set; }
        
        // Mot de passe hashé de l'utilisateur (jamais stocké en clair pour des raisons de sécurité)
        public string Password { get; set; }
        
        // Nom de l'entreprise ou de l'organisation de l'utilisateur
        public string CompanyName { get; set; }
        
        // Rôle de l'utilisateur dans le système, par défaut "company" (entreprise)
        public string Role { get; set; } = "company"; // Valeurs possibles : "company", "admin"

        // Propriété qui indique si l'utilisateur est un administrateur
        // Permet de vérifier facilement les droits d'accès
        public bool IsAdmin
        {
            get { return Role == "admin"; }
        }
    }
}
