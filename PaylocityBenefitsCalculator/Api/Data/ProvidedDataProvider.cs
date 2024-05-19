using Api.Models;

namespace Api.Data
{
    public class ProvidedDataProvider : IDataProvider
    {
        /*
        * This is the provided list of employees and dependents that was previously in the controller.
        * I did separate the dependents from the employees and added the EmployeeId to each dependent.
        * I realize ProvidedDataProvider sounds... weird.
        * 
        */

        public List<Employee> GetEmployees()
        {
            return new List<Employee>
            {
                new()
                {
                    Id = 1,
                    FirstName = "LeBron",
                    LastName = "James",
                    Salary = 75420.99m,
                    DateOfBirth = new DateTime(1984, 12, 30),
                },
                new()
                {
                    Id = 2,
                    FirstName = "Ja",
                    LastName = "Morant",
                    Salary = 92365.22m,
                    DateOfBirth = new DateTime(1999, 8, 10),
                },
                new()
                {
                    Id = 3,
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Salary = 143211.12m,
                    DateOfBirth = new DateTime(1963, 2, 17),
                }
            };
        }

        public List<Dependent> GetDependents()
        {
            return new List<Dependent>
            {
               new()
                {
                    Id = 1,
                    EmployeeId = 2,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3),
                },
                new()
                {
                    Id = 2,
                    EmployeeId = 2,
                    FirstName = "Child1",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2020, 6, 23),
                },
                new()
                {
                    Id = 3,
                    EmployeeId = 2,
                    FirstName = "Child2",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2021, 5, 18),
                },
                new()
                {
                    Id = 4,
                    EmployeeId = 3,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2),
                }
            };
        }
    }
}
