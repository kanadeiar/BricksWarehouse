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

    [TestMethod]
    public void Init_ChangeBricksType_ShouldChangesValues()
    {
        using var context = new Bunit.TestContext();
        var component = context.RenderComponent<BricksWarehouse.Blazor.Components.BricksCalcComponent>();

        component.Find(".form-group .form-select").Change(1);

        var lengthInput = component.Find(".form-group .form-control");
        Assert
            .IsTrue(lengthInput.ToMarkup().Contains("250"));
    }

    [TestMethod]
    public void Init_Calculate_ShouldCorrectResults()
    {
        using var context = new Bunit.TestContext();
        var component = context.RenderComponent<BricksWarehouse.Blazor.Components.BricksCalcComponent>();

        component.Find(".form-group .form-select").Change(1);
        component.Find(".btn,.btn-success").Click();

        var calcResult = component.Find("#calc-result");
        Assert
            .IsTrue(calcResult.ToMarkup().Contains("12"));
    }
}

