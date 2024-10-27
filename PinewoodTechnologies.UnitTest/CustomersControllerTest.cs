using Microsoft.AspNetCore.Mvc;
using Moq;
using PinewoodTechnologies.Controllers;
using PinewoodTechnologies.Models;
using PinewoodTechnologies.ViewModels;
using System.Net;

namespace PinewoodTechnologies.UnitTest
{
    [TestClass]
    public class CustomersControllerTest
    {
        [TestMethod]
        public void GetCustomers_PageOne_Count()
        {
            var customercontroller = new CustomersController();
            var page = customercontroller.GetCustomers() as ObjectResult;
            var data = page?.Value as PagedDataModel<Customer> ?? new PagedDataModel<Customer>();

            Assert.AreEqual(1, data?.data.Count ?? 0);
        }


        [TestMethod]
        public async Task GetCustomer_UsingValidId_OkResult()
        {
            var customercontroller = new CustomersController();
            var result = await customercontroller.GetCustomer(4) as ObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode.GetValueOrDefault() ?? (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task GetCustomer_UsingInvalidId_OkResult()
        {
            var customercontroller = new CustomersController();
            var result = await customercontroller.GetCustomer(1) as ObjectResult;

            Assert.AreEqual((int)HttpStatusCode.NotFound, result?.StatusCode.GetValueOrDefault() ?? (int)HttpStatusCode.BadRequest);
        }
    }
}