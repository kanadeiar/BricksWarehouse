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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
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
                .Setup(_ => _.GetAllAsync(It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(places));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.GetRecommendedShipmentPlaces(1).Result;

            Assert
                .AreEqual(expectedCount, result.Count());
            Assert
                .AreEqual(expectedDateTime, result.First().LastDateTime);
            Assert
                .AreEqual(expectedDateTime.AddHours(expectedCount - 1), result.Last().LastDateTime);
        }


        [TestMethod]
        public void LoadProductToPlace_CallWithNull_ShouldNull()
        {
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(null));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.LoadProductToPlace(1, 1, 1).Result;

            Assert
                .IsNull(result);
            placeDataMock
                .Verify(_ => _.GetAsync(1), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void LoadProductToPlace_CallWithProductTypeNull_ShouldCorrectNewValueProductTypeAndCount()
        {
            const int expectedProdutTypeId = 1;
            const int expectedCount = 1;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var place = new Place
            {
                Id = 1,
                Name = "Test_1",
                Number = 1,
                ProductTypeId = null,
                Count = 0,
                Size = 10,
            };
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(place));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.LoadProductToPlace(expectedProdutTypeId, 1, expectedCount).Result;

            Assert
                .IsInstanceOfType(result, typeof(Place));
            Assert
                .AreEqual(expectedProdutTypeId, result.ProductTypeId);
            Assert
                .AreEqual(expectedCount, result.Count);
            placeDataMock
                .Verify(_ => _.GetAsync(1), Times.AtLeast(2));
            placeDataMock
                .Verify(_ => _.UpdateAsync(It.IsAny<Place>()), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void LoadProductToPlace_CallWithIncorrectProductType2_ShouldNull()
        {
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var place = new Place
            {
                Id = 1,
                Name = $"Test_1",
                Number = 1,
                ProductTypeId = 1,
                Count = 0,
                Size = 10,
            };
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(place));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.LoadProductToPlace(2, 1, 1).Result;

            Assert
                .IsNull(result);
            placeDataMock
                .Verify(_ => _.GetAsync(1), Times.Once);
            placeDataMock
                .Verify(_ => _.UpdateAsync(It.IsAny<Place>()), Times.Never);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [DataTestMethod]
        [DataRow(1, 1, 0, 1, 1)]
        [DataRow(1, 1, 5, 1, 6)]
        [DataRow(1, 1, 0, 2, 2)]
        [DataRow(1, 1, 9, 1, 10)]
        [DataRow(1, 1, 10, 1, 10)]
        [DataRow(1, 1, 2, 3, 5)]
        [DataRow(2, 1, 0, 1, 1)]
        [DataRow(1, 2, 0, 1, 1)]
        [DataRow(5, 5, 5, 2, 7)]
        [DataRow(10, 10, 9, 2, 10)]
        public void LoadProductToPlace_CallCorrectData_ShouldCorrect(int productTypeId, int placeId, int count, int add, int expected)
        {
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var place = new Place
            {
                Id = placeId,
                Name = "Test_1",
                Number = 1,
                ProductTypeId = productTypeId,
                Count = count,
                Size = 10,
            };
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(place));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.LoadProductToPlace(productTypeId, placeId, add).Result;

            Assert
                .IsInstanceOfType(result, typeof(Place));
            Assert
                .AreEqual(expected, result.Count);
            placeDataMock
                .Verify(_ => _.GetAsync(placeId), Times.AtLeast(2));
            placeDataMock
                .Verify(_ => _.UpdateAsync(It.IsAny<Place>()), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void LoadProductToPlace_CallCorrectData_ShouldUpdateLastDateTime()
        {
            DateTime expected = DateTime.Now;
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var place = new Place
            {
                Id = 1,
                Name = "Test_1",
                Number = 1,
                ProductTypeId = null,
                Count = 0,
                Size = 10,
                LastDateTime = DateTime.Today,
            };
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(place));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var result = taskDataService.LoadProductToPlace(1, 1, 1).Result;

            Assert
                .IsInstanceOfType(result, typeof(Place));
            Assert
                .IsTrue((result.LastDateTime - expected).Milliseconds < 1000);
        }
        [TestMethod]
        public void LoadProductToPlace_CallCorrectData_ShouldCallbackCorrectData()
        {
            const int expectedCount = 1;
            Place callback = new Place();
            var outTaskDataStub = Mock
                .Of<IOutTaskData>();
            var placeDataMock = new Mock<IPlaceData>();
            var place = new Place
            {
                Id = 1,
                Name = "Test_1",
                Number = 1,
                ProductTypeId = null,
                Count = 0,
                Size = 10,
            };
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(place));
            placeDataMock
                .Setup(_ => _.UpdateAsync(It.IsAny<Place>()))
                .Callback((Place p) => callback = p);
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataStub);

            var _ = taskDataService.LoadProductToPlace(1, 1, 1).Result;

            Assert
                .AreEqual(place, callback);
            Assert
                .AreEqual(expectedCount, callback.Count);
        }


        [TestMethod]
        public void ShipmentProductFromPlace_CallWithNullTask_ShouldNull()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(null));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(new Place { ProductTypeId = 1 }));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;

            Assert
                .IsNull(result);
            outTaskDataMock
                .Verify(_ => _.GetAsync(expectedTaskId), Times.Once);
            outTaskDataMock
                .VerifyNoOtherCalls();
            placeDataMock
                .Verify(_ => _.GetAsync(expectedPlaceId), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void ShipmentProductFromPlace_CallWithNullPlace_ShouldNull()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(new OutTask { ProductTypeId = 1 }));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(null));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;

            Assert
                .IsNull(result);
            outTaskDataMock
                .Verify(_ => _.GetAsync(expectedTaskId), Times.Once);
            outTaskDataMock
                .VerifyNoOtherCalls();
            placeDataMock
                .Verify(_ => _.GetAsync(expectedPlaceId), Times.Once);
            placeDataMock
                .VerifyNoOtherCalls();
        }
        [TestMethod]
        public void ShipmentProductFromPlace_CallWithNullProductTypeInTask_ShouldNull()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(new OutTask { ProductTypeId = null }));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(new Place { ProductTypeId = 1 }));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;

            Assert
                .IsNull(result);
        }
        [TestMethod]
        public void ShipmentProductFromPlace_CallWithNullProductTypeInPlace_ShouldNull()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(new OutTask { ProductTypeId = 1 }));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(new Place { ProductTypeId = null }));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;

            Assert
                .IsNull(result);
        }
        [TestMethod]
        public void ShipmentProductFromPlace_CallWithNullProductTypeNotEqual_ShouldNull()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(new OutTask { ProductTypeId = 1 }));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(new Place { ProductTypeId = 2 }));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;

            Assert
                .IsNull(result);
        }
        [TestMethod]
        public void ShipmentProductFromPlace_CallWithMoreTaskEmpty_ShouldCorrectedValue()
        {
            const int expectedPlaceId = 1;
            const int expectedTaskId = 1;
            var outTaskDataMock = new Mock<IOutTaskData>();
            outTaskDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OutTask?>(new OutTask { ProductTypeId = 1, Loaded = 0, Count = 10 }));
            var placeDataMock = new Mock<IPlaceData>();
            placeDataMock
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Place?>(new Place { ProductTypeId = 1, Count = 5, Size = 10 }));
            var taskDataService = new TaskDataService(placeDataMock.Object, outTaskDataMock.Object);

            var result = taskDataService.ShipmentProductFromPlace(expectedPlaceId, expectedTaskId, 1).Result;
        }

    }
}
