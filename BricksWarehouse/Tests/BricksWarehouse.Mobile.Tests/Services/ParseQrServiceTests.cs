using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Mobile.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BricksWarehouse.Mobile.Tests.Services
{
    [TestClass]
    public class ParseQrServiceTests
    {
        [TestMethod]
        public void Get_CallIncorrectCodeOutTask_ShouldNonResult()
        {
            const string expectedMessage = "QR-код не распознан";
            var parseQrService = new ParseQrService();

            var (errorQr, datas) = parseQrService.Get(TypeQrCode.OutTask, "non-correct");

            Assert
                .AreEqual(expectedMessage, errorQr);
        }

    }
}
