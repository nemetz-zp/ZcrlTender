using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using TenderLibrary;

namespace ZcrlTender
{
    public static class UserSession
    {
        private static string authenticatedUsername;

        public static string AuthenticatedUsername
        {
            get
            {
                return authenticatedUsername;
            }
        }

        public static bool IsAuthorized
        {
            get
            {
                return !string.IsNullOrWhiteSpace(authenticatedUsername);
            }
        }

        private static byte[] GetPasswordHash(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(passwordBytes);

            return hash;
        }

        public static void CreateUser(string username, string password, string description)
        {
            User newUser = new User
            {
                Name = username,
                PasswordHash = Convert.ToBase64String(GetPasswordHash(password)),
                Description = description
            };

            using(TenderContext tc = new TenderContext())
            {
                tc.Users.Add(newUser);
                tc.SaveChanges();
            }
        }

        public static bool ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            // Проверяем старый пароль
            if (AuthorizeUser(username, oldPassword))
            {
                using(TenderContext tc = new TenderContext())
                {
                    User target = tc.Users.Where(p => p.Name.Equals(username)).FirstOrDefault();
                    target.PasswordHash = Convert.ToBase64String(GetPasswordHash(newPassword));
                    tc.Entry<User>(target).State = System.Data.Entity.EntityState.Modified;

                    tc.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool AuthorizeUser(string username, string password)
        {
            using(TenderContext tc = new TenderContext())
            {
                User target = tc.Users.Where(p => p.Name.Equals(username)).FirstOrDefault();

                if(target == null)
                {
                    return false;
                }

                byte[] hashOfEnteredPass = GetPasswordHash(password);
                byte[] hashOfUserInDb = Convert.FromBase64String(target.PasswordHash);

                if(hashOfEnteredPass.Length != hashOfUserInDb.Length)
                {
                    return false;
                }

                for(int i = 0; i < hashOfEnteredPass.Length; i++)
                {
                    if(hashOfEnteredPass[i] != hashOfUserInDb[i])
                    {
                        return false;
                    }
                }

                authenticatedUsername = target.Name;

                return true;
            }            
        }
    }
}
