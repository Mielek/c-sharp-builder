namespace Mielek.Test.Builder.Generator;

using Mielek.Test.Shop.Model;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        var item = new ItemBuilder().Name("name").Build();

        Assert.IsTrue(item is Item);
    }
}
