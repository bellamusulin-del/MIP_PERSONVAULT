using System;
using PersonnelVault.Services;

namespace PersonnelVault.Models
{
    // The base "Person" class: every role (Employee, Manager, Director) inherits from it.
    // It is abstract because we do not want to create a plain "Person" - everyone always
    // exists in the system as a CONCRETE role.
    //
    // The login data (username, encoded password, save time) also lives here -
    // there is no separate class linked with a "PersonId", because in a project
    // of this size that would only be an unnecessary indirect reference.
    public abstract class Person
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public string Username { get; private set; }
        public string EncodedPassword { get; private set; }
        public DateTime PasswordSavedAt { get; private set; }

        // The shift of the Caesar cipher - in a real system it could differ per user,
        // here it is a shared constant to keep things simple.
        private const int CipherShift = 5;

        // Simple auto-increment Id, so it does not have to be typed in and Ids do not collide.
        private static int _nextId = 1;

        protected Person(string name)
        {
            Id = _nextId;
            _nextId = _nextId + 1;
            Name = name;
        }

        // Sets/replaces the login data. The password is NEVER stored in its original
        // form - it is always Caesar-encoded before saving.
        public void SetCredentials(string username, string plainPassword)
        {
            Username = username;
            EncodedPassword = CaesarCipher.Encode(plainPassword, CipherShift);
            PasswordSavedAt = DateTime.Now;
        }

        // We decode the stored (encoded) password and compare it with the typed password.
        public bool PasswordMatches(string attemptedPassword)
        {
            if (EncodedPassword == null)
            {
                return false; // no password is set
            }

            string decodedPassword = CaesarCipher.Decode(EncodedPassword, CipherShift);
            return decodedPassword == attemptedPassword;
        }

        // Every derived class "introduces itself" differently -
        // this is classic polymorphism: the same method name, different behavior per class.
        public abstract string DescribeRole();

        public override string ToString()
        {
            return "[" + Id + "] " + Name + " - " + DescribeRole();
        }
    }
}
