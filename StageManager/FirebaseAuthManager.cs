using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace StageManager
{
    /// <summary>
    /// Gère l'authentification Firebase pour l'application
    /// </summary>
    public static class FirebaseAuthManager
    {
        // Clé API Web Firebase (à récupérer depuis la console Firebase)
        // Cette clé est utilisée pour authentifier les requêtes vers l'API Firebase Authentication
        // Dans un environnement de production, cette clé devrait être stockée de manière sécurisée
        private const string API_KEY = "AIzaSyDpxeGgqlDYuLkULqAFQILea7V22F4eMvQ"; // Remplacez par votre clé API Web Firebase
        
        // URLs des endpoints d'authentification Firebase
        // Ces URL sont les points d'accès à l'API REST de Firebase Authentication
        // SIGN_UP_URL : pour créer un nouveau compte utilisateur
        private const string SIGN_UP_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={0}";
        // SIGN_IN_URL : pour connecter un utilisateur existant
        private const string SIGN_IN_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={0}";
        
        // Client HTTP pour les requêtes d'authentification
        // Un seul client HTTP est utilisé pour toutes les requêtes, ce qui est plus efficace
        private static readonly HttpClient client = new HttpClient();
        
        // Token d'authentification actuel
        // Ce token est stocké en mémoire pendant la session de l'utilisateur
        // Il est utilisé pour authentifier les requêtes vers Firebase
        private static string idToken;
        
        /// <summary>
        /// Token d'authentification Firebase actuel
        /// </summary>
        /// <remarks>
        /// Propriété en lecture seule qui retourne le token d'authentification actuel
        /// Ce token est utilisé pour authentifier les requêtes vers Firebase
        /// </remarks>
        public static string IdToken => idToken;
        
        /// <summary>
        /// Indique si un utilisateur est actuellement connecté
        /// </summary>
        /// <remarks>
        /// Vérifie simplement si un token d'authentification existe
        /// Utilisé pour déterminer si l'utilisateur est connecté ou non
        /// </remarks>
        public static bool IsAuthenticated => !string.IsNullOrEmpty(idToken);
        
        /// <summary>
        /// Crée un nouvel utilisateur dans Firebase Authentication
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Token d'authentification si réussi</returns>
        /// <remarks>
        /// Cette méthode envoie une requête à l'API Firebase pour créer un nouveau compte
        /// Si la création réussit, un token d'authentification est retourné et stocké
        /// </remarks>
        public static async Task<string> SignUpAsync(string email, string password)
        {
            // Prépare les données à envoyer à l'API Firebase
            var payload = new Dictionary<string, object>
            {
                { "email", email },                // Adresse email de l'utilisateur
                { "password", password },          // Mot de passe de l'utilisateur
                { "returnSecureToken", true }     // Demande à Firebase de retourner un token d'authentification
            };
            
            // Envoie la requête d'inscription à Firebase et attend la réponse
            var response = await SendAuthRequestAsync(string.Format(SIGN_UP_URL, API_KEY), payload);
            // Extrait le token d'authentification de la réponse et le stocke
            idToken = response["idToken"].ToString();
            return idToken;
        }
        
        /// <summary>
        /// Connecte un utilisateur existant via Firebase Authentication
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Token d'authentification si réussi</returns>
        /// <remarks>
        /// Cette méthode vérifie les identifiants de l'utilisateur auprès de Firebase
        /// Si les identifiants sont corrects, un token d'authentification est retourné et stocké
        /// </remarks>
        public static async Task<string> SignInAsync(string email, string password)
        {
            // Prépare les données à envoyer à l'API Firebase
            var payload = new Dictionary<string, object>
            {
                { "email", email },                // Adresse email de l'utilisateur
                { "password", password },          // Mot de passe de l'utilisateur
                { "returnSecureToken", true }     // Demande à Firebase de retourner un token d'authentification
            };
            
            // Envoie la requête de connexion à Firebase et attend la réponse
            var response = await SendAuthRequestAsync(string.Format(SIGN_IN_URL, API_KEY), payload);
            // Extrait le token d'authentification de la réponse et le stocke
            idToken = response["idToken"].ToString();
            return idToken;
        }
        
        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        /// <remarks>
        /// Cette méthode est très simple : elle efface simplement le token d'authentification
        /// Firebase n'a pas besoin d'être notifié de la déconnexion, car les tokens sont gérés côté client
        /// Une fois le token effacé, l'utilisateur n'est plus authentifié pour les futures requêtes
        /// </remarks>
        public static void SignOut()
        {
            // Efface le token d'authentification, ce qui déconnecte effectivement l'utilisateur
            idToken = null;
        }
        
        /// <summary>
        /// Envoie une requête d'authentification à Firebase
        /// </summary>
        /// <param name="url">URL de l'endpoint d'authentification Firebase</param>
        /// <param name="payload">Données à envoyer dans la requête</param>
        /// <returns>Réponse de Firebase sous forme de dictionnaire</returns>
        /// <remarks>
        /// Cette méthode privée gère les détails techniques de l'envoi des requêtes HTTP à Firebase
        /// Elle est utilisée par les méthodes SignUpAsync et SignInAsync
        /// </remarks>
        private static async Task<Dictionary<string, object>> SendAuthRequestAsync(string url, Dictionary<string, object> payload)
        {
            // Convertit les données en JSON et prépare le contenu de la requête HTTP
            var content = new StringContent(
                JsonConvert.SerializeObject(payload),  // Convertit le dictionnaire en chaîne JSON
                Encoding.UTF8,                        // Utilise l'encodage UTF-8 pour le texte
                "application/json");                  // Définit le type de contenu comme JSON
                
            // Envoie la requête POST à l'URL spécifiée avec les données JSON
            var response = await client.PostAsync(url, content);
            // Récupère le contenu de la réponse sous forme de texte
            var responseContent = await response.Content.ReadAsStringAsync();
            
            // Vérifie si la requête a réussi (code HTTP 200-299)
            if (!response.IsSuccessStatusCode)
            {
                // En cas d'échec, extrait les détails de l'erreur de la réponse
                var error = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                var errorDetails = (error["error"] as Newtonsoft.Json.Linq.JObject).ToObject<Dictionary<string, object>>();
                // Lance une exception avec le message d'erreur de Firebase
                throw new Exception($"Erreur d'authentification Firebase: {errorDetails["message"]}");
            }
            
            // Si tout s'est bien passé, convertit la réponse JSON en dictionnaire et la retourne
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
        }
    }
}
