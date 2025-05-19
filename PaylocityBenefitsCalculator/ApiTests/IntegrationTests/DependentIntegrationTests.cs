using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    [Fact]
    //task: make test pass
    // OK
    // updated to support new test cases
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
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
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1975, 5, 15)
            },
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
            },
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
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    //task: make test pass
    // OK
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    //task: make test pass
    // OK
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

