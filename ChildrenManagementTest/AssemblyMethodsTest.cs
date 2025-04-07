using Moq;

namespace ChildrenManagementTest;

[TestClass]
public class AssemblyMethodsTest
{

    [AssemblyInitialize]
    public static void AssemblyInitializeMethod(TestContext context)
    {
        //Initialization TodayDate
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(m => m.Today).Returns(new DateTime(2024, 4, 3));

    }

}