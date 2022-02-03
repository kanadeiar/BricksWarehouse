using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Interfaces.Services;
using BricksWarehouse.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BricksWarehouse.Tests.Services
{
    [TestClass]
    public class FillingsInfoServiceTests
    {
        [TestMethod]
        public void GetFillings_CorrectCallZero_ShouldCorrectType()
        {
            var placeDataStub = Mock
                .Of<IPlaceData>();
            var service = new FillingsInfoService(placeDataStub);

            var actual = service.GetFillings().Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<FillingInfoWebModel>));
            Assert
                .AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void GetFillings_CorrectCall_ShouldCorrectValues()
        {
            const int expectedCount = 10;
            const string expectedName = "Test_1";
            const int expectedNumber = 1;
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, expectedCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = null,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
            var service = new FillingsInfoService(placeDataMock.Object);

            var actual = service.GetFillings().Result;

            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedName, actual.First().Name);
            Assert
                .AreEqual(expectedNumber, actual.First().Number);
            Assert
                .AreEqual("Пусто", actual.First().Sost);
            placeDataMock
                .Verify(_ => _.GetAllAsync(true, false), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void GetFillings_CallFreeMinus_ShouldFree0()
        {
            const int expectedFree = 0;
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, 10).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = null,
                Count = 15,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
            var service = new FillingsInfoService(placeDataMock.Object);
            
            var actual = service.GetFillings().Result;

            Assert
                .AreEqual(expectedFree, actual.First().Free);
            Assert
                .AreEqual("Переполнен", actual.First().Sost);
            Assert
                .AreEqual(150, actual.First().Filling);
            Assert
                .AreEqual(0, actual.First().Freeing);
        }
        [TestMethod]
        public void GetFillings_CorrectCountCall_ShouldCorrectCounts()
        {
            const int expectedFree = 5;
            const int expectedCount = 10;
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, expectedCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = null,
                Count = 5,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
            var service = new FillingsInfoService(placeDataMock.Object);

            var actual = service.GetFillings().Result;

            Assert
                .AreEqual(expectedFree, actual.First().Free);
            Assert
                .AreEqual("Норма", actual.First().Sost);
            Assert
                .AreEqual(50, actual.First().Filling);
            Assert
                .AreEqual(50, actual.First().Freeing);
        }
        [TestMethod]
        public void GetFillings_FullCountCall_ShouldFull()
        {
            const int expectedFree = 0;
            const int expectedCount = 10;
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, expectedCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = null,
                Count = 10,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
            var service = new FillingsInfoService(placeDataMock.Object);

            var actual = service.GetFillings().Result;

            Assert
                .AreEqual(expectedFree, actual.First().Free);
            Assert
                .AreEqual("Заполнен", actual.First().Sost);
            Assert
                .AreEqual(100, actual.First().Filling);
            Assert
                .AreEqual(0, actual.First().Freeing);
        }
    }
}
