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
        // Utilisation du pattern Singleton pour s'assurer qu'une seule connexion à Firebase est active
        // à tout moment dans l'application, ce qui économise les ressources
        private static IFirebaseClient client;

        /// <summary>
        /// Crée la configuration Firebase avec les paramètres de connexion
        /// </summary>
        /// <returns>Un objet de configuration pour se connecter à Firebase</returns>
        private static IFirebaseConfig CreateConfig()
        {
            return new FirebaseConfig
            {
                // Clé d'authentification pour accéder à Firebase (secret d'API)
                // Cette clé permet de sécuriser l'accès à la base de données
                // Dans un environnement de production, cette clé devrait être stockée de manière sécurisée
                // (fichier de configuration protégé, variables d'environnement, etc.)
                AuthSecret = "mC4wLsFopITjiOQfUZIqRRnH0j2ArWy8gBJhPS8d",
                
                // URL de la base de données Firebase
                // C'est l'adresse de notre base de données en temps réel sur Firebase
                BasePath = "https://stage-manager-7e2a9-default-rtdb.europe-west1.firebasedatabase.app/"
            };
        }

        /// <summary>
        /// Client Firebase partagé pour toute l'application.
        /// Crée une nouvelle instance si elle n'existe pas encore (lazy loading).
        /// </summary>
        /// <remarks>
        /// Utilise le principe de "lazy loading" (chargement paresseux) : le client
        /// n'est créé que lorsqu'on en a besoin pour la première fois, ce qui évite
        /// d'utiliser des ressources inutilement au démarrage de l'application
        /// </remarks>
        public static IFirebaseClient Client
        {
            get
            {
                if (client == null)
                {
                    try
                    {
                        // Crée une nouvelle instance du client avec la configuration
                        // Cette opération établit la connexion avec Firebase
                        client = new FirebaseClient(CreateConfig());
                    }
                    catch (Exception ex)
                    {
                        // En cas d'erreur de connexion, on lance une exception avec un message explicatif
                        // Cela peut se produire si l'URL est incorrecte, si la clé d'API est invalide
                        // ou si l'utilisateur n'a pas de connexion Internet
                        throw new Exception($"Erreur de connexion à Firebase: {ex.Message}");
                    }
                }
                return client;
            }
        }
        
        /// <summary>
        /// Réinitialise le client Firebase, utile après une connexion/déconnexion
        /// </summary>
        /// <remarks>
        /// Cette méthode est appelée lorsqu'un utilisateur se déconnecte ou change de compte
        /// pour s'assurer que la prochaine requête Firebase créera une nouvelle connexion
        /// </remarks>
        public static void ResetClient()
        {
            // En mettant client à null, la prochaine fois qu'on accède à la propriété Client,
            // une nouvelle instance sera créée avec les paramètres de configuration à jour
            client = null;
        }
    }
}
