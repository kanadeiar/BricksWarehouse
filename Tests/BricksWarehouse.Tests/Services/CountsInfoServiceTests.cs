namespace BricksWarehouse.Tests.Services;

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

