namespace BricksWarehouse.Tests.Blazor.Components;

[TestClass]
public class IndexComponentTests
{
    [TestMethod]
    public void Init_Initializes_ShouldCorrect()
    {
        using var context = new Bunit.TestContext();

        var component = context.RenderComponent<BricksWarehouse.Blazor.Components.IndexComponent>();
        
        Assert
            .IsTrue(component.Markup.Contains("<p>Приложение управления складом кирпичей.</p>"));
    }
}

