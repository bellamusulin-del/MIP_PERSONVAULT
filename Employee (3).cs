namespace PersonnelVault.Models
{
    // A regular employee. Has a department and a salary.
    public class Employee : Person
    {
        public string Department { get; set; }
        public decimal Salary { get; set; }

        public Employee(string name, string department, decimal salary)
            : base(name)
        {
            Department = department;
            Salary = salary;
        }

        public override string DescribeRole()
        {
            return "Employee @ " + Department + " (" + Salary.ToString("0") + " RON)";
        }
    }
}
