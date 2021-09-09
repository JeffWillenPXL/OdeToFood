using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;
using OdeToFood.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Tests.Controllers.Api
{
    [TestFixture]
    public class ReviewControllerTests
    {
        private Mock<IReviewRepository> _mock;
        private ReviewController _sut;
        private readonly Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IReviewRepository>();
            _sut = new ReviewController(_mock.Object);
        }

        [Test]
        public async Task Get_ReturnsAllReviewsFromRepository()
        {
            //Arrange
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Review>());
            //Act
            var result = await _sut.Get() as OkObjectResult;

            //Assert
            _mock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.InstanceOf<List<Review>>());
        }

        [Test]
        public async Task Get_ReturnsReviewIfItExists()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review();
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(review).Verifiable();

            //Act
            var result = await _sut.Get(id) as OkObjectResult;

            //Assert
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.AreSame(result.Value, review);
        }

        [Test]
        public async Task Post_ValidReviewIsSavedInRepository()
        {
            //Arrange
            var review = new Review();
            _mock.Setup(x => x.AddAsync(It.IsAny<Review>())).ReturnsAsync(review).Verifiable();

            //Act
            var result = await _sut.Post(review) as CreatedAtActionResult;

            //Act
            _mock.Verify();
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.SameAs(review));
            Assert.That(result.ActionName, Is.EqualTo(nameof(_sut.Post)));

        }

        [Test]
        public async Task Post_InvalidReviewCausesBadRequest()
        {
            //Arrange
            var review = new Review();
            _sut.ModelState.AddModelError("ReviewerName", "ReviewerName is required");

            //Act
            var result = await _sut.Post(review) as BadRequestResult;

            //Assert
            _mock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Put_ExistingReviewIsSavedInRepository()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review
            {
                Id = id
            };
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(review);


            //Act
            var result = await _sut.Put(review, id) as OkResult;

            //Act
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Review>()), Times.Once);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            
            
        }

        [Test]
        public async Task Put_NonExistingReviewReturnsNotFound()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review
            {
                Id = id
            };
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()));


            //Act
            var result = await _sut.Put(review, id) as NotFoundResult;

            //Act
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Review>()), Times.Never);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            
        }

        [Test]
        public async Task Put_InvalidReviewModelStateCausesBadRequest()
        {
            //Arrange
            _sut.ModelState.AddModelError("ReviewerName", "ReviewerName is required");
            var id = _random.Next();
            var review = new Review
            {
                Id = id
            };
            


            //Act
            var result = await _sut.Put(review, id) as BadRequestResult;

            //Act
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Review>()), Times.Never);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Put_MismatchBetweenUrlIdAndReviewIdCausesBadRequest()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review
            {
                Id = _random.Next()
            };
            


            //Act
            var result = await _sut.Put(review, id) as BadRequestResult;

            //Act
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Review>()), Times.Never);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Delete_ExistingReviewIsDeletedFromRepository()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review
            {
                Id = id
            };
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(review);


            //Act
            var result = await _sut.Delete(id) as OkResult;

            //Act
            _mock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Delete_NonExistingReviewReturnsNotFound()
        {
            //Arrange
            var id = _random.Next();
            var review = new Review
            {
                Id = id
            };
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()));


            //Act
            var result = await _sut.Delete(id) as NotFoundResult;

            //Act
            _mock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
            _mock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
        }

    }
}
