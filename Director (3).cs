using System;

namespace PersonnelVault.Models
{
    // The company can have only ONE director (Director) - we enforce this in the code too,
    // not just agree on it, so that nobody creates a second one.
    // This is related to the Singleton pattern, but here it is used as a business rule:
    // if someone still tries to create a second one, the program throws an exception.
    public class Director : Employee
    {
        // Shows whether the director "seat" is already taken
        private static bool _seatIsTaken = false;

        public string Company { get; set; }

        public Director(
            string name,
            string department,
            decimal salary,
            string company)
            : base(name, department, salary)
        {
            if (_seatIsTaken)
            {
                throw new InvalidOperationException(
                    "Ebből a cégből csak egyetlen Director lehet - a hely már foglalt."
                );
            }

            Company = company;
            _seatIsTaken = true;
        }

        public override string DescribeRole()
        {
            return "Director of " + Company;
        }
    }
}
