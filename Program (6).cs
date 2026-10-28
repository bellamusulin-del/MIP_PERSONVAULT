using PersonnelVault.Models;
using PersonnelVault.Services;
using System;
using System.IO;
using System.Text;

namespace PersonnelVault
{
    internal class Program
    {
        private static void Main()
        {
            // So that accented letters (ő, ű, é...) are shown correctly in the console
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Vault vault = new Vault();

            // The CSV is next to the program (in the bin folder), so we read it from there
            string csvPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "sample_people.csv"
            );

            try
            {
                vault.ImportFromCsv(csvPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nem sikerült beolvasni a CSV-t: " + ex.Message);
                Console.WriteLine("Nyomj Entert a kilépéshez...");
                Console.ReadLine();
                return;
            }

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("PERSONNEL VAULT");
                Console.WriteLine("1. Bejelentkezés");
                Console.WriteLine("2. GDPR törlés");
                Console.WriteLine("3. Username/password módosítása");
                Console.WriteLine("4. Utolsó mentés megtekintése");
                Console.WriteLine("5. Személyek megtekintése");
                Console.WriteLine("6. Kilépés");
                Console.Write("Választás: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    // login
                    case "1":
                        Login(vault);
                        break;

                    // GDPR deletion
                    case "2":
                        DeletePerson(vault);
                        break;

                    // change username/password
                    case "3":
                        ChangeCredentials(vault);
                        break;

                    // last password save
                    case "4":
                        ShowLastSaved(vault);
                        break;

                    // show people
                    case "5":
                        ShowPeople(vault);
                        break;

                    // exit
                    case "6":
                        running = false;
                        Console.WriteLine("Kilépés...");
                        break;

                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Nyomj Entert a menühöz...");
                    Console.ReadLine();
                }
            }
        }

        // login
        private static void Login(Vault vault)
        {
            Console.WriteLine();
            Console.WriteLine("BEJELENTKEZÉS");

            Console.Write("Felhasználónév: ");
            string username = Console.ReadLine();

            Console.Write("Jelszó: ");
            string password = Console.ReadLine();

            bool success = vault.TryLogin(username, password);

            if (success)
            {
                Console.WriteLine("Sikeres bejelentkezés!");
            }
            else
            {
                Console.WriteLine("Hibás felhasználónév vagy jelszó!");
            }
        }

        // GDPR deletion
        private static void DeletePerson(Vault vault)
        {
            Console.WriteLine();
            Console.WriteLine("GDPR TÖRLÉS");

            ShowPeople(vault);

            Console.Write("Melyik személyt szeretnéd törölni? ID: ");

            if (!int.TryParse(Console.ReadLine(), out int personId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.Write("Biztosan törölni szeretnéd? (i/n): ");

            string answer = Console.ReadLine();

            if (answer == null || answer.ToLower() != "i")
            {
                Console.WriteLine("A törlés megszakítva.");
                return;
            }

            bool deleted = vault.ForgetPerson(personId);

            if (deleted)
            {
                Console.WriteLine("A személy sikeresen törölve lett.");
            }
            else
            {
                Console.WriteLine("Nem található ilyen ID-jú személy.");
            }
        }

        // change username/password
        private static void ChangeCredentials(Vault vault)
        {
            Console.WriteLine();
            Console.WriteLine("USERNAME/PASSWORD MÓDOSÍTÁSA");

            ShowPeople(vault);

            Console.Write("Személy ID: ");

            if (!int.TryParse(Console.ReadLine(), out int personId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Console.Write("Új username: ");
            string username = Console.ReadLine();

            Console.Write("Új password: ");
            string password = Console.ReadLine();

            try
            {
                vault.RegisterCredentials(
                    personId,
                    username,
                    password
                );

                Console.WriteLine("Az adatok módosítva lettek.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // last password save
        private static void ShowLastSaved(Vault vault)
        {
            Console.WriteLine();
            Console.WriteLine("UTOLSÓ PASSWORD MENTÉS");

            ShowPeople(vault);

            Console.Write("Személy ID: ");

            if (!int.TryParse(Console.ReadLine(), out int personId))
            {
                Console.WriteLine("Érvénytelen ID!");
                return;
            }

            Person person = vault.GetPerson(personId);

            if (person == null)
            {
                Console.WriteLine("Nem található ilyen személy.");
                return;
            }

            Console.WriteLine(
                $"Username: {person.Username}"
            );

            Console.WriteLine(
                $"Password utolsó mentése: {person.PasswordSavedAt}"
            );
        }

        // show people
        private static void ShowPeople(Vault vault)
        {
            Console.WriteLine();
            Console.WriteLine("SZEMÉLYEK");

            if (vault.People.Count == 0)
            {
                Console.WriteLine("Nincs személy a rendszerben.");
                return;
            }

            foreach (Person person in vault.People)
            {
                Console.WriteLine(person);
            }
        }
    }
}