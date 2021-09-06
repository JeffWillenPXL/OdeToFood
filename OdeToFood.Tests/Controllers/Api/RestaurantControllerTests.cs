using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;
using OdeToFood.Web.Controllers.Api;
using System;
using System.Collections.Generic;

namespace OdeToFood.Tests.Controllers.Api
{
    [TestFixture]
    public class RestaurantControllerTests
    {
        private RestaurantController _sut;
        private Mock<IRestaurantRepository> _mock = new Mock<IRestaurantRepository>();

        [SetUp]
        public void Setup()
        {
            
            _sut = new RestaurantController(_mock.Object);
        }

        [Test]
        public void Get_ReturnsAllRestaurantsFromRepository()
        {
            //Arrange
            _mock.Setup(x => x.GetAll()).Returns(new List<Restaurant>()).Verifiable();

            //Act
            var result = _sut.Get() as OkObjectResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsInstanceOf<List<Restaurant>>(result.Value);
            

        }

        [Test]
        public void Get_ReturnsRestaurantIfItExists()
        {
            //Arrange
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Restaurant()).Verifiable();

            //Act
            var id = new Random().Next();
            var result = _sut.Get(id) as OkObjectResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsInstanceOf<Restaurant>(result.Value);
        }

        [Test]
        public void Get_ReturnsNotFoundIfItDoesNotExists()
        {
            //Arrange
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            var id = new Random().Next();
            var result = _sut.Get(id) as NotFoundResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            
        }
    }
}