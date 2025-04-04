using System;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace StageManager
{
    /// <summary>
    /// Gère la connexion à Firebase et fournit un client unique pour toute l'application
    /// </summary>
    public static class FirebaseManager
    {
        // Instance unique du client Firebase (pattern Singleton)
        private static IFirebaseClient client;

        /// <summary>
        /// Crée la configuration Firebase avec les paramètres de connexion
        /// </summary>
        private static IFirebaseConfig CreateConfig()
        {
            return new FirebaseConfig
            {
                // Clé d'authentification pour accéder à Firebase
                AuthSecret = "mC4wLsFopITjiOQfUZIqRRnH0j2ArWy8gBJhPS8d",
                
                // URL de la base de données Firebase
                BasePath = "https://stage-manager-7e2a9-default-rtdb.europe-west1.firebasedatabase.app/"
            };
        }

        /// <summary>
        /// Client Firebase partagé pour toute l'application.
        /// Crée une nouvelle instance si elle n'existe pas encore (lazy loading).
        /// </summary>
        public static IFirebaseClient Client
        {
            get
            {
                if (client == null)
                {
                    try
                    {
                        // Crée une nouvelle instance du client avec la configuration
                        client = new FirebaseClient(CreateConfig());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erreur de connexion à Firebase: {ex.Message}");
                    }
                }
                return client;
            }
        }
        
        /// <summary>
        /// Réinitialise le client Firebase, utile après une connexion/déconnexion
        /// </summary>
        public static void ResetClient()
        {
            client = null;
        }
    }
}
