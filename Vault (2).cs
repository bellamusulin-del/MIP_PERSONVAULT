using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PersonnelVault.Models;

namespace PersonnelVault.Services
{
    // This replaces the database: it keeps the people in memory.
    public class Vault
    {
        private readonly List<Person> _people = new List<Person>();

        // For a GDPR deletion we only store the time.
        private readonly List<DateTime> _deletionLog = new List<DateTime>();

        public IReadOnlyList<Person> People
        {
            get { return _people; }
        }

        public int DeletionCount
        {
            get { return _deletionLog.Count; }
        }

        public void AddPerson(Person person)
        {
            _people.Add(person);
        }

        // Reading the CSV
        public void ImportFromCsv(string filePath)
        {
            string[] dataLines = File.ReadAllLines(filePath);

            bool firstLine = true;

            foreach (string line in dataLines)
            {
                // The first line is the header, we skip it
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                // We also skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] columns = line.Split(',');

                if (columns.Length < 8)
                {
                    throw new FormatException(
                        "Hibás CSV sor (8 oszlop kell): '" + line + "'"
                    );
                }

                string name = columns[0].Trim();
                string role = columns[1].Trim().ToLowerInvariant();
                string department = columns[2].Trim();
                string salaryText = columns[3].Trim();

                string teamSizeText = columns[4].Trim();
                string company = columns[5].Trim();

                string username = columns[6].Trim();
                string password = columns[7].Trim();

                decimal salary = decimal.Parse(
                    salaryText,
                    CultureInfo.InvariantCulture
                );

                Person person;

                if (role == "employee")
                {
                    person = new Employee(
                        name,
                        department,
                        salary
                    );
                }
                else if (role == "manager")
                {
                    int teamSize = int.Parse(
                        teamSizeText,
                        CultureInfo.InvariantCulture
                    );

                    person = new Manager(
                        name,
                        department,
                        salary,
                        teamSize
                    );
                }
                else if (role == "director")
                {
                    person = new Director(
                        name,
                        department,
                        salary,
                        company
                    );
                }
                else
                {
                    throw new NotSupportedException(
                        "Ismeretlen szerepkör a CSV-ben: '" + role + "'"
                    );
                }

                person.SetCredentials(
                    username,
                    password
                );

                AddPerson(person);
            }
        }

        // changing username and password
        public void RegisterCredentials(
            int personId,
            string username,
            string plainPassword)
        {
            Person person = FindPersonById(personId);

            if (person == null)
            {
                throw new ArgumentException(
                    "Nincs ilyen Id-jú személy a rendszerben."
                );
            }

            person.SetCredentials(username, plainPassword);
        }

        // finding a person by ID
        public Person GetPerson(int personId)
        {
            return FindPersonById(personId);
        }

        // checking the login
        public bool TryLogin(
            string username,
            string attemptedPassword)
        {
            Person person = null;

            foreach (Person currentPerson in _people)
            {
                if (currentPerson.Username == username)
                {
                    person = currentPerson;
                    break;
                }
            }

            if (person == null)
            {
                return false;
            }

            return person.PasswordMatches(attemptedPassword);
        }

        // GDPR deletion
        public bool ForgetPerson(int personId)
        {
            Person person = FindPersonById(personId);

            if (person == null)
            {
                return false;
            }

            _people.Remove(person);

            // We only store that a deletion happened.
            _deletionLog.Add(DateTime.Now);

            return true;
        }

        private Person FindPersonById(int personId)
        {
            foreach (Person currentPerson in _people)
            {
                if (currentPerson.Id == personId)
                {
                    return currentPerson;
                }
            }

            return null;
        }
    }
}
