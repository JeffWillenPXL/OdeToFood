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
        private Mock<IRestaurantRepository> _mock;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRestaurantRepository>();
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
            var id = _random.Next();
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
            var id = _random.Next();
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Verifiable();

            //Act
            
            var result = _sut.Get(id) as NotFoundResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            
        }

        [Test]
        public void Post_ValidRestaurantIsSavedInRepository()
        {
            //Arrange
            var name = Guid.NewGuid().ToString();
            var restaurant = new Restaurant { Name = name };
            _mock.Setup(x => x.Create(It.IsAny<Restaurant>())).Returns(restaurant).Verifiable();

            //Act
            var result = _sut.Post(restaurant) as CreatedAtActionResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.That(result.Value, Is.SameAs(restaurant));
            Assert.That(result.RouteValues["id"], Is.EqualTo(restaurant.Id));
        }

        [Test]
        public void Post_InvalidRestaurantCausesBadRequest()
        {
            //Arrange
            _sut.ModelState.AddModelError("Name", "Name is required");
            var restaurant = new Restaurant();
            //_mock.Setup(x => x.Create(It.IsAny<Restaurant>())).Returns(() => restaurant);


            //Act
            var result = _sut.Post(restaurant) as BadRequestResult;

            //Assert
            _mock.Verify(x => x.Create(It.IsAny<Restaurant>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
                        
        }

        public void Put_ExistingRestaurantIsSavedInRepository()
        {
            //Arrange
            var id = _random.Next();
            var restaurant = new Restaurant();
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(restaurant).Verifiable();
            _mock.Setup(x => x.Update(It.IsAny<Restaurant>())).Returns(() => restaurant).Verifiable();

            //Act

            var result = _sut.Put(restaurant, id) as OkObjectResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.SameAs(restaurant));


        }

        [Test]
        public void Put_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            var id = _random.Next();
            var restaurant = new Restaurant();
   

            //Act
            
            var result = _sut.Put(restaurant, id) as NotFoundResult;

            //Assert
            _mock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _mock.Verify(x => x.Update(restaurant), Times.Never);
            Assert.IsNotNull(result);
           
        }

        [Test]
        public void Put_InvalidRestaurantModelStateCausesBadRequest()
        {
            //Arrange
            _sut.ModelState.AddModelError("Name", "Name is required");
            var id = _random.Next();
            var restaurant = new Restaurant();
            

            //Act
            var result = _sut.Put(restaurant, id) as BadRequestResult;

            //Assert
            _mock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _mock.Verify(x => x.Update(restaurant), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Put_MismatchBetweenUrlIdAndRestaurantIdCausesBadRequest()
        {
            //Arrange
            var id = _random.Next();
            var restaurant = new Restaurant
            {
                Id = _random.Next()
            };
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(restaurant).Verifiable();
            

            //Act
            var result = _sut.Put(restaurant, id) as BadRequestResult;

            //Assert
            _mock.Verify();
            _mock.Verify(x => x.Update(It.IsAny<Restaurant>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Delete_ExistingRestaurantIsDeletedFromRepository()
        {
            //Arrange
            var id = _random.Next();
            var restaurant = new Restaurant();
            _mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(restaurant).Verifiable();
            _mock.Setup(x => x.Delete(It.IsAny<Restaurant>())).Verifiable();

            //Act
            var result = _sut.Delete(id) as OkResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);

        }

        [Test]
        public void Delete_NonExistingRestaurantReturnsNotFound()
        {
            //Arrange
            var id = _random.Next();
            
          

            //Act
            var result = _sut.Delete(id) as NotFoundResult;

            //Assert
            _mock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _mock.Verify(x => x.Delete(It.IsAny<Restaurant>()), Times.Never);
            Assert.IsNotNull(result);
        }
    }
}