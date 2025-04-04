using System;

namespace StageManager
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; } = "company"; // Valeurs possibles : "company", "admin"

        public bool IsAdmin
        {
            get { return Role == "admin"; }
        }
    }
}
