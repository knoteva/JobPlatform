using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpiceHouse.Data;
using SpiceHouse.Data.Models;
using SpiceHouse.Data.Seeding;
using SpiceHouse.Web;
using SpiceHouse.Web.Areas.Customer.Controllers;
using SpiceHouse.Web.Areas.Administration.Controllers;
using SpiceHouse.Web.Controllers;
using Xunit;
using Xunit.Sdk;

namespace Tests.Test
{
    public class HomePageTests
    {
        [Fact]
        public async Task HomePageShouldNotHaveTitle()
        {
            var serverFactory = new WebApplicationFactory<Startup>();
        
            var client = serverFactory.CreateClient();

            var response = await client.GetAsync("/");
            var responseAsString = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("<h1", responseAsString);
        }

        [Fact]
        public void Method_ReturnsCorrect_WhenCorrectIsGiven()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Find_User_Database") // Give a Unique name to the DB
                .Options;

            var a = new Category
            {
                Name = "Burgers",
            };
            var b = new Category
            {
                Name = "Burgers",
            };
            using (var dbContext = new ApplicationDbContext(options)) 
            {
                var contr = new CategoryController(dbContext);
                var x = contr.Create();
                dbContext.Categories.AddRange(a);
                dbContext.Categories.AddRange(b);
                dbContext.SaveChanges();
            }

            
        }

    }
}
