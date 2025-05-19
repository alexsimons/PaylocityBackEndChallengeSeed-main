using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
    [Fact]
    // updated to support new test cases
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
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
                Id = 3,
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
                Id = 4,
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
                Id = 5,
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
                Id = 6,
                FirstName = "Employee6",
                LastName = "Last6",
                Salary = 80000.00m,
                DateOfBirth = new DateTime(1988, 3, 14),
                Dependents = new List<GetDependentDto>()
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    //task: make test pass
    // OK
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }

    [Fact]
    //task: make test pass
    // OK
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PaycheckCalculation_NoDependents_NoHighEarner()
    {
        // no dependents, no high earner 
        var response = await HttpClient.GetAsync("/api/v1/employees/1/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 1,
            EmployeeName = "LeBron James",
            GrossPay = 2900.81m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 0m,
            DependentOver50Deductions = 0m,
            HighEarnerDeductions = 0m,
            TotalDeductions = 461.54m,
            NetPay = 2439.27m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task PaycheckCalculation_WithDependents_HighEarner()
    {
        // dependents under 50, high earner 
        var response = await HttpClient.GetAsync("/api/v1/employees/2/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 2,
            EmployeeName = "Ja Morant",
            GrossPay = 3552.51m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 830.77m,
            DependentOver50Deductions = 0m,
            HighEarnerDeductions = 71.05m,
            TotalDeductions = 1363.36m,
            NetPay = 2189.15m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task PaycheckCalculation_WithDependentsOver50_HighEarner()
    {
        // multiple dependents over 50, one under 50, high earner 
        var response = await HttpClient.GetAsync("/api/v1/employees/5/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 5,
            EmployeeName = "Employee5 Last5",
            GrossPay = 4615.38m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 1107.69m,
            DependentOver50Deductions = 276.92m,
            HighEarnerDeductions = 92.31m,
            TotalDeductions = 1938.46m,
            NetPay = 2676.92m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task PaycheckCalculation_WithDependents_NoHighEarner()
    {
        // dependents under 50, no high earner
        var response = await HttpClient.GetAsync("/api/v1/employees/4/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 4,
            EmployeeName = "Employee4 Last4",
            GrossPay = 2576.92m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 830.77m,
            DependentOver50Deductions = 0m,
            HighEarnerDeductions = 0m,
            TotalDeductions = 1292.31m,
            NetPay = 1284.61m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task PaycheckCalculation_Exactly50Dependent()
    {
        // dependent is exactly 50 (instructions say over 50), high earner 
        var response = await HttpClient.GetAsync("/api/v1/employees/3/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 3,
            EmployeeName = "Michael Jordan",
            GrossPay = 5508.12m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 276.92m,
            DependentOver50Deductions = 0m,
            HighEarnerDeductions = 110.16m,
            TotalDeductions = 848.62m,
            NetPay = 4659.50m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }
    
    [Fact]
    public async Task PaycheckCalculation_Exactly80Salary()
    {
        // no dependents, no high earner (salary is $80,000 - instructions say over $80,000)
        var response = await HttpClient.GetAsync("/api/v1/employees/6/paycheck");
        var expected = new PaycheckDto
        {
            EmployeeId = 6,
            EmployeeName = "Employee6 Last6",
            GrossPay = 3076.92m,
            EmployeeDeductions = 461.54m,
            DependentDeductions = 0m,
            DependentOver50Deductions = 0m,
            HighEarnerDeductions = 0m,
            TotalDeductions = 461.54m,
            NetPay = 2615.38m
        };
        await response.ShouldReturn(System.Net.HttpStatusCode.OK, expected);
    }
}

