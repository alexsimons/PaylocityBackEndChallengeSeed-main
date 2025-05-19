using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        // test data for employees and dependents
        // in a real application, this would come from the database
        // with a separate data access class
        private readonly List<GetEmployeeDto> _employees = new()
        {
            new()
            {
                Id = 1, // no dependents, no high earner
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2, // has dependents, high earner
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m, 
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3, // has dependents (exactly 50), high earner
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1975, 5, 15)
                    }
                }
            },
            new()
            {
                Id = 4, // has dependents, no high earner
                FirstName = "Employee4",
                LastName = "Last4",
                Salary = 67000.00m,
                DateOfBirth = new DateTime(1980, 10, 16),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 5,
                        FirstName = "Spouse",
                        LastName = "Last4",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1985, 7, 5)
                    },
                    new()
                    {
                        Id = 6,
                        FirstName = "Child1",
                        LastName = "Last4",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2012, 4, 10)
                    },
                    new()
                    {
                        Id = 7,
                        FirstName = "Child2",
                        LastName = "Last4",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2015, 8, 22)
                    }
                }
            },
            new()
            {
                Id = 5, // has dependents (multiple over 50, one under), high earner
                FirstName = "Employee5",
                LastName = "Last5",
                Salary = 120000.00m,
                DateOfBirth = new DateTime(1942, 6, 11),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 8,
                        FirstName = "DP",
                        LastName = "Last5",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1945, 5, 24)
                    },
                    new()
                    {
                        Id = 9,
                        FirstName = "Child1",
                        LastName = "Last5",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(1962, 3, 1)
                    },
                    new()
                    {
                        Id = 10,
                        FirstName = "Child2",
                        LastName = "Last5",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(1970, 7, 15)
                    },
                    new()
                    {
                        Id = 11,
                        FirstName = "Child3",
                        LastName = "Last5",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(1990, 2, 10)
                    }
                }
            },
            new()
            {
                Id = 6, // no dependents, no high earner (salary exactly 80,000)
                FirstName = "Employee6",
                LastName = "Last6",
                Salary = 80000.00m,
                DateOfBirth = new DateTime(1988, 3, 14),
                Dependents = new List<GetDependentDto>()
            }
        };

        // Service class to handle business logic for employees and dependents
        public Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            // get an employee by ID 
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }

        public Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            // get all employees
            return Task.FromResult(_employees);
        }

        public Task<GetDependentDto?> GetDependentByIdAsync(int dependentId)
        {
            // get a dependent by ID
            var dependent = _employees
                .SelectMany(e => e.Dependents ?? new List<GetDependentDto>())
                .FirstOrDefault(d => d.Id == dependentId);
            return Task.FromResult(dependent);
        }

        public Task<List<GetDependentDto>> GetAllDependentsAsync()
        {
            // get all dependents from all employees
            var dependents = _employees
                .SelectMany(e => e.Dependents ?? new List<GetDependentDto>())
                .ToList();
            return Task.FromResult(dependents);
        }

        public Task<PaycheckDto?> CalculatePaycheckAsync(int employeeId)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null) return Task.FromResult<PaycheckDto?>(null);

            // values for calculations from instructions
            // these could also come from a config file or database to allow for easy changes / client customizations
            int paychecks = 26;
            decimal employeeCost = 1000m;       // monthly cost for employee
            decimal dependentCost = 600m;       // monthly cost for each dependent
            decimal over50Cost = 200m;          // extra cost for dependents over 50
            decimal highEarnerLimit = 80000m;   // high earner limit
            decimal highEarnerCost = 0.02m;     // extra deduction percentage for high earners

            decimal employeeAnnualDeductions = employeeCost * 12;

            // calculate dependent costs
            decimal dependentAnnualDeductions = 0m;
            decimal over50AnnualDeductions = 0m;
            if (employee.Dependents != null)
            {
                foreach (var dep in employee.Dependents)
                {
                    dependentAnnualDeductions += dependentCost * 12;
                    // calculate age to check if over 50
                    var age = DateTime.Today.Year - dep.DateOfBirth.Year;
                    if (dep.DateOfBirth > DateTime.Today.AddYears(-age)) age--;
                    if (age > 50)
                    {
                        // add extra cost if over 50
                        over50AnnualDeductions += over50Cost * 12;
                    }
                }
            }

            // calculate extra deduction for high earners
            decimal highEarnerAnnualDeduction = 0m;
            if (employee.Salary > highEarnerLimit)
            {
                highEarnerAnnualDeduction = employee.Salary * highEarnerCost;
            }

            // calculate results by the number of paychecks
            decimal grossPay = Math.Round(employee.Salary / paychecks, 2);
            decimal employeeDeductions = Math.Round(employeeAnnualDeductions / paychecks, 2);
            decimal dependentDeductions = Math.Round(dependentAnnualDeductions / paychecks, 2);
            decimal over50Deductions = Math.Round(over50AnnualDeductions / paychecks, 2);
            decimal highEarnerDeductions = Math.Round(highEarnerAnnualDeduction / paychecks, 2);
            decimal totalDeductions = employeeDeductions + dependentDeductions + over50Deductions + highEarnerDeductions;
            decimal netPay = grossPay - totalDeductions;

            var dto = new PaycheckDto
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                GrossPay = grossPay,
                EmployeeDeductions = employeeDeductions,
                DependentDeductions = dependentDeductions,
                DependentOver50Deductions = over50Deductions,
                HighEarnerDeductions = highEarnerDeductions,
                TotalDeductions = totalDeductions,
                NetPay = netPay
            };

            return Task.FromResult<PaycheckDto?>(dto);
        }
    }
}
