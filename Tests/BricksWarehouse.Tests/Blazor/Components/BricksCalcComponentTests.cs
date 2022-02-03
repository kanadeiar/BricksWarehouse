namespace BricksWarehouse.Tests.Blazor.Components;

[TestClass]
public class BricksCalcComponentTests
{
    [TestMethod]
    public void Init_Initializes_ShouldCorrect()
    {
        using var context = new Bunit.TestContext();

        var component = context.RenderComponent<BricksWarehouse.Blazor.Components.BricksCalcComponent>();

        Assert
            .IsTrue(component.Markup.Contains("Калькулятор кирпичей"));
    }
}

