using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Interfaces.Services;
using BricksWarehouse.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BricksWarehouse.Tests.Services
{
    [TestClass]
    public class CountsInfoServiceTests
    {
        [TestMethod]
        public void GetCounts_CallZero_ShouldCorrectType()
        {
            var productTypeStub = Mock
                .Of<IProductTypeData>();
            var service = new CountsInfoService(productTypeStub);

            var actual = service.GetCounts().Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<CountsInfoWebModel>));
            Assert
                .AreEqual(0, actual.Count());
        }
    }
}
