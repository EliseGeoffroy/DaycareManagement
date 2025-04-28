using ChildrenManagement.staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class NavigationTest
{
    [DataTestMethod]
    [DataRow("0", true, DisplayName = "entryOK")]
    [DataRow("3", false, DisplayName = "entryKO")]
    [DataRow("", false, DisplayName = "No entry")]
    public void ValidateMenuChoice(string entry, bool validationExpectedResult)
    {
        StringReader stringReader = new(entry);
        Console.SetIn(stringReader);

        (bool validationResult, _) = Navigation.ValidateMenuChoice();

        Assert.AreEqual(validationExpectedResult, validationResult);
    }

}