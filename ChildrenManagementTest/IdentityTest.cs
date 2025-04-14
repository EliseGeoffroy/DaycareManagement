using System.Reflection;
using ChildrenManagement.staticClasses;
using ChildrenManagement.Classes;

namespace ChildrenManagementTest;

[TestClass]
public class IdentityTest
{

    #region Form
    #region Right Cases
    [TestMethod]
    public void CorrectFormularIdentity_ShouldCreateCorrectIdentity()
    {

        string[] testSet = ["1234567891234", "Dupont", "Anna", "2"];

        Identity identity = new();
        Utilities.InputDataTest(identity, testSet);

        Assert.AreEqual(1234567891234L, identity.Id);
        Assert.AreEqual("Dupont", identity.Name);
        Assert.AreEqual("Anna", identity.Firstname);
        Assert.AreEqual(Nationalities.German, identity.Nationality);
    }
    #endregion
    #region Wrong cases

    #region LackOfData

    [DataTestMethod]
    [DataRow("1234567891234", "", "Anna", "2", DisplayName = "Without Name")]
    [DataRow("1234567891234", "Dupont", "", "2", DisplayName = "Without Firstname")]
    //   [ExpectedException(typeof(Exception))]
    public void FormularWithoutAData_ShouldThrowAnException(string id, string name, string firstname, string nationality)
    {
        string?[] testSet = [id, name, firstname, nationality];
        Identity identity = new();

        Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
    }

    [DataTestMethod]
    [DataRow("", "Dupont", "Anna", "2", DisplayName = "Without Id")]
    [DataRow("1234567891234", "Dupont", "Anna", "", DisplayName = "Without Nationality")]
    public void FormularWithoutAData_ShouldThrowAnException2(string id, string name, string firstname, string nationality)
    {
        string?[] testSet = [id, name, firstname, nationality];
        Identity identity = new();

        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));
    }

    #endregion

    #region InvalidId

    [TestMethod]
    public void FormularWithInvalidIDWithoutEnoughDigit_ShouldThrownAnException()
    {

        string[] testSet = ["123456789", "Dupont", "Anna", "2"];

        Identity identity = new();
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
        Assert.AreEqual("Un matricule luxembourgeois doit comporter 13 chiffres.", exception.InnerException?.Message);
    }

    [TestMethod]
    public void FormularWithInvalidIDWithChar_ShouldThrownAnException()
    {

        string[] testSet = ["ABCDEFGHIJKLM", "Dupont", "Anna", "2"];

        Identity identity = new();
        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));
    }

    #endregion

    #region Invalid Name/Firstname

    [DataTestMethod]
    [DataRow("A", DisplayName = "Not enough char")]
    [DataRow("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", DisplayName = "Too much char")]
    public void FormularWithInvalidName_ShouldThrownAnException(string name)
    {

        string[] testSet = ["1234567891234", name, "Anna", "2"];

        Identity identity = new();
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
        Assert.AreEqual("Le nom doit comporter de 2 à 70 caractères", exception.InnerException?.Message);
    }

    [DataTestMethod]
    [DataRow("A", DisplayName = "Not enough char")]
    [DataRow("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", DisplayName = "Too much char")]
    public void FormularWithInvalidFirstname_ShouldThrownAnException(string firstname)
    {

        string[] testSet = ["1234567891234", "Dupont", firstname, "2"];

        Identity identity = new();
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
        Assert.AreEqual("Le prénom doit comporter de 2 à 70 caractères", exception.InnerException?.Message);
    }

    #endregion

    #region Invalid Nationality

    [TestMethod]
    public void FormularWithInvalidNationalityNumber_ShouldThrownAnException()
    {

        string[] testSet = ["1234567891234", "Dupont", "Anna", "8"];

        Identity identity = new();
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
        Assert.AreEqual("Votre réponse ne correspond à aucune Nationalité connue", exception.InnerException?.Message);
    }

    [TestMethod]
    public void FormularWithInvalidNationality_ShouldThrownAnException()
    {

        string[] testSet = ["1234567891234", "Dupont", "Anna", "Française"];

        Identity identity = new();
        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));

    }
    #endregion
    #endregion
    #endregion
}