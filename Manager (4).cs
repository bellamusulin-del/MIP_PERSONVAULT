namespace PersonnelVault.Models
{
    // A person who leads a team. Inherits from Employee (has a department and a salary),
    // and adds the team size (TeamSize) as its own extra data.
    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        public Manager(
            string name,
            string department,
            decimal salary,
            int teamSize)
            : base(name, department, salary)
        {
            TeamSize = teamSize;
        }

        public override string DescribeRole()
        {
            return "Manager of " + TeamSize + " people @ " + Department;
        }
    }
}
