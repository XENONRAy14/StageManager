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
        private const string API_KEY = "AIzaSyDpxeGgqlDYuLkULqAFQILea7V22F4eMvQ"; // Remplacez par votre clé API Web Firebase
        
        // URLs des endpoints d'authentification Firebase
        private const string SIGN_UP_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={0}";
        private const string SIGN_IN_URL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={0}";
        
        // Client HTTP pour les requêtes d'authentification
        private static readonly HttpClient client = new HttpClient();
        
        // Token d'authentification actuel
        private static string idToken;
        
        /// <summary>
        /// Token d'authentification Firebase actuel
        /// </summary>
        public static string IdToken => idToken;
        
        /// <summary>
        /// Indique si un utilisateur est actuellement connecté
        /// </summary>
        public static bool IsAuthenticated => !string.IsNullOrEmpty(idToken);
        
        /// <summary>
        /// Crée un nouvel utilisateur dans Firebase Authentication
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Token d'authentification si réussi</returns>
        public static async Task<string> SignUpAsync(string email, string password)
        {
            var payload = new Dictionary<string, object>
            {
                { "email", email },
                { "password", password },
                { "returnSecureToken", true }
            };
            
            var response = await SendAuthRequestAsync(string.Format(SIGN_UP_URL, API_KEY), payload);
            idToken = response["idToken"].ToString();
            return idToken;
        }
        
        /// <summary>
        /// Connecte un utilisateur existant via Firebase Authentication
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Token d'authentification si réussi</returns>
        public static async Task<string> SignInAsync(string email, string password)
        {
            var payload = new Dictionary<string, object>
            {
                { "email", email },
                { "password", password },
                { "returnSecureToken", true }
            };
            
            var response = await SendAuthRequestAsync(string.Format(SIGN_IN_URL, API_KEY), payload);
            idToken = response["idToken"].ToString();
            return idToken;
        }
        
        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        public static void SignOut()
        {
            idToken = null;
        }
        
        /// <summary>
        /// Envoie une requête d'authentification à Firebase
        /// </summary>
        private static async Task<Dictionary<string, object>> SendAuthRequestAsync(string url, Dictionary<string, object> payload)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");
                
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                var errorDetails = (error["error"] as Newtonsoft.Json.Linq.JObject).ToObject<Dictionary<string, object>>();
                throw new Exception($"Erreur d'authentification Firebase: {errorDetails["message"]}");
            }
            
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
        }
    }
}
