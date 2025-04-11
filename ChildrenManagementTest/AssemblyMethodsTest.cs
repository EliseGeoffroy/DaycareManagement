using Moq;

namespace ChildrenManagementTest;

[TestClass]
public class AssemblyMethodsTest
{

    [AssemblyInitialize]
    public static void AssemblyInitializeMethod(TestContext context)
    {
        Directory.SetCurrentDirectory(@"C:\Users\geoff\Desktop\C#.Net\DaycareManagement\ChildrenManagementTest\data\files");
    }

}