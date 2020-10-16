using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SwaggerEg.Controllers;
using SwaggerEg.Models;
using System.Collections.Generic;
using System.Linq;

namespace SwaggerEgTesting
{
    [TestFixture]
    public class Tests
    {
        private static OrganizationContext db;

        [SetUp]
        public void Setup()
        {
            var loan = new List<Product>
            {
                new Product{Id = 1, Name="Dummy1", Price=30,Category="Toy"},
                new Product{Id = 2, Name="Dummy1", Price=60,Category="Toy"},
                new Product{Id = 4, Name="Dummy1", Price=30,Category="Toy"}
            };
            var loandata = loan.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(loandata.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(loandata.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(loandata.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(loandata.GetEnumerator());
            var mockContext = new Mock<OrganizationContext>();
            mockContext.Setup(c => c.Product).Returns(mockSet.Object);
            db = mockContext.Object;
        }

        private static readonly Product p1 = new Product();

        private static readonly Prd p2 = new Prd(new OrganizationContext());
        private readonly ProductsController controller = new ProductsController(p2);
        public Tests()
        {

        }

        [Test]
        public void GetAllProductsTest()
        {
            // Prd P = new Prd(db);
            //   ProductsController obj = new ProductsController(P);
            // var data = obj.GetProduct();
            var data = controller.GetProduct();
            OkObjectResult okResult = data as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void Test2pass()
        {
            Prd P = new Prd(db);
            ProductsController obj = new ProductsController(P);
            ObjectResult data = (ObjectResult)obj.GetProduct(2);
            // ObjectResult data = (ObjectResult)controller.GetProduct(2);
            Assert.AreEqual(200, data.StatusCode);

        }

        [Test]
        public void Test2Fail()
        {
            Prd P = new Prd(db);
            ProductsController obj = new ProductsController(P);
            BadRequestResult data = (BadRequestResult)obj.GetProduct(89);
            //BadRequestResult data = (BadRequestResult)controller.GetProduct(89);
            Assert.AreEqual(400, data.StatusCode);

        }

        [Test]
        public void DeletePass()
        {
            Prd P = new Prd(db);
            ProductsController obj = new ProductsController(P);
            ObjectResult data = (ObjectResult)obj.DeleteProduct(2);
            //  ObjectResult data = (ObjectResult)controller.DeleteProduct(2);
            Assert.AreEqual(200, data.StatusCode);

        }

        [Test]
        public void DeleteFail()
        {
            Prd P = new Prd(db);
            ProductsController obj = new ProductsController(P);
            BadRequestResult data = (BadRequestResult)obj.DeleteProduct(290);
            //BadRequestResult data = (BadRequestResult)controller.DeleteProduct(290);
            Assert.AreEqual(400, data.StatusCode);

        }
    }
}