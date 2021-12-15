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
    public class TaskDataServiceTests
    {
        [TestMethod]
        public void GetRecommendedLoadPlaces_GetForCorrectProductType_ShouldCorrectOneCall()
        {
            const int expectedCount = 10;
            const string expectedName = "Test_1";
            const int expectedNumber = 1;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, expectedCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = null,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedLoadPlaces(1).Result;

            Assert
                .IsInstanceOfType(result, typeof(IEnumerable<Place>));
            Assert
                .AreEqual(expectedCount, result.Count());
            Assert
                .AreEqual(expectedName, result.First().Name);
            Assert
                .AreEqual(expectedNumber, result.First().Number);
            placeDataMock
                .Verify(_ => _.GetAllAsync(true, false), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetRecommendedLoadPlaces_GetForCorrectProductTypeWith1_ShouldCorrect6()
        {
            const int totalCount = 10;
            const int expectedCount = 6;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = (i <= expectedCount) ? 1 : 2,
                Count = 0,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedLoadPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void GetRecommendedLoadPlaces_GetForCorrectProductTypeWith1AndCount_ShouldCorrect3()
        {
            const int totalCount = 10;
            const int expectedCount = 3;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = 1,
                Count = (i <= expectedCount) ? 0 : 10,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedLoadPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void GetRecommendedLoadPlaces_GetForCorrectProductTypeSorted_ShouldCorrectSorted()
        {
            const int totalCount = 10;
            const int expectedCount = 6;
            const int expectedCountInFirst = 6;
            const int expectedCountInLast = 1;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = 1,
                Count = (i <= expectedCount) ? i : 10,
                Size = 10,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedLoadPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
            Assert
                .AreEqual(expectedCountInFirst, result.First().Count);
            Assert
                .AreEqual(expectedCountInLast, result.Last().Count);
        }


        [TestMethod]
        public void GetRecommendedShipmentPlaces_GetForCorrectProductType_ShouldCorrectOneCall()
        {
            const int expectedCount = 10;
            const string expectedName = "Test_1";
            const int expectedNumber = 1;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, expectedCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = 1,
                Count = 1,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedShipmentPlaces(1).Result;

            Assert
                .IsInstanceOfType(result, typeof(IEnumerable<Place>));
            Assert
                .AreEqual(expectedCount, result.Count());
            Assert
                .AreEqual(expectedName, result.First().Name);
            Assert
                .AreEqual(expectedNumber, result.First().Number);
            placeDataMock
                .Verify(_ => _.GetAllAsync(true, false), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetRecommendedShipmentPlaces_GetForCorrectProductTypeWith1_ShouldCorrect6()
        {
            const int totalCount = 10;
            const int expectedCount = 6;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = (i <= expectedCount) ? 1 : 2,
                Count = 1,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedShipmentPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void GetRecommendedShipmentPlaces_GetForCorrectProductTypeWith1AndCount_ShouldCorrect3()
        {
            const int totalCount = 10;
            const int expectedCount = 3;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = 1,
                Count = (i <= expectedCount) ? 1 : 0,
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedShipmentPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void GetRecommendedShipmentPlaces_GetForCorrectProductTypeSorted_ShouldCorrectSorted()
        {
            const int totalCount = 10;
            const int expectedCount = 6;
            DateTime expectedDateTime = DateTime.Today.AddHours(8);
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var places = Enumerable.Range(1, totalCount).Select(i => new Place
            {
                Id = i,
                Name = $"Test_{i}",
                Number = i,
                ProductTypeId = 1,
                Count = (i <= expectedCount) ? 1 : 0,
                LastDateTime = expectedDateTime.AddHours(i - 1),
            });
            placeDataMock
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()).Result)
                .Returns(places);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedShipmentPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
            Assert
                .AreEqual(expectedDateTime, result.First().LastDateTime);
            Assert
                .AreEqual(expectedDateTime.AddHours(expectedCount - 1), result.Last().LastDateTime);
        }
    }
}
