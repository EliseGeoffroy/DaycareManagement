using System.Reflection;
using ChildrenManagement.Classes;
using ChildrenManagement.staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class TrustedPersonTest
{

    #region Form
    private Identity _identity = new(7894561237894L, "Dupont", "Vincent", Nationalities.French);

    #region RightCases

    [TestMethod]
    public void ValidForm_CreateRightObject()
    {
        string[] testSet = ["1", "+33678451278", "15/06/1989"];
        TrustedPerson trustedPerson = new(_identity);

        Utilities.InputDataTest(trustedPerson, testSet);

        Assert.AreEqual(RelationshipToChild.Father, trustedPerson.RelationshipToChild);
        Assert.AreEqual("+33678451278", trustedPerson.PhoneNumber);
        Assert.AreEqual(new DateTime(1989, 06, 15), trustedPerson.BirthDate);

    }


    [DataTestMethod]
    [DataRow("+33673398541", DisplayName = "French phoneNumber")]
    [DataRow("+493012345678", DisplayName = "German phoneNumber")]
    [DataRow("+3221234567", DisplayName = "Belgian phoneNumber")]
    [DataRow("+35226123456", DisplayName = "Luxembourgish phoneNumber")]
    public void FormWithValidPhoneNumber_ShouldCreateRightObject(string phoneNumber)
    {
        string[] testSet = ["1", phoneNumber, "15/06/1989"];
        TrustedPerson trustedPerson = new(_identity);

        Utilities.InputDataTest(trustedPerson, testSet);

        Assert.AreEqual(phoneNumber, trustedPerson.PhoneNumber);
    }

    #endregion

    #region Wrong Cases

    #region Lack of datas

    [TestMethod]
    public void FormWithoutRelationship_ShouldThrowAnException()
    {

        string[] testSet = ["", "+33678451278", "15/06/1989"];
        TrustedPerson trustedPerson = new(_identity);

        var exception = Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(trustedPerson, testSet));
    }

    [TestMethod]
    public void FormWithoutPhonenumber_ShouldThrowAnException()
    {

        string[] testSet = ["1", "", "15/06/1989"];
        TrustedPerson trustedPerson = new(_identity);

        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(trustedPerson, testSet));
        Assert.AreEqual("Ce champ est obligatoire", exception.InnerException?.Message);
    }

    [TestMethod]
    public void FormWithoutBirthDate_ShouldThrowAnException()
    {

        string[] testSet = ["1", "+33678451278", ""];
        TrustedPerson trustedPerson = new(_identity);

        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(trustedPerson, testSet));
    }

    #endregion

    #region Invalid relationship

    [TestMethod]
    public void WrongRelationship_NumberTooBig_ShouldThrowAnException()
    {
        string[] testSet = ["10", "+33678451278", "15/06/2015"];

        TrustedPerson trustedPerson = new(_identity);

        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(trustedPerson, testSet));
        Assert.AreEqual("Le numéro rentré ne correspond à aucune proposition", exception.InnerException?.Message);
    }

    [TestMethod]
    public void WrongRelationship_Description_ShouldThrowAnException()
    {
        string[] testSet = ["Papa", "+33678451278", "15/06/2015"];

        TrustedPerson trustedPerson = new(_identity);

        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(trustedPerson, testSet));
    }
    #endregion

    #region Invalid PhoneNumber

    [DataTestMethod]
    [DataRow("0673398541", DisplayName = "PhoneNumber without countrycode")]
    [DataRow("+34673398541", DisplayName = "PhoneNumber with wrong countrycode")]
    [DataRow("+336733985", DisplayName = "Wrong French phoneNumber")]
    [DataRow("+4930123456", DisplayName = "Wrong German phoneNumber")]
    [DataRow("+3221234567669", DisplayName = "Wrong Belgian phoneNumber")]
    [DataRow("+352261234567", DisplayName = "WrongLuxembourgish phoneNumber")]
    public void InValidPhoneNumber_ShouldThrowAnException(string phoneNumber)
    {
        string[] testSet = ["1", phoneNumber, "15/06/1989"];
        TrustedPerson trustedPerson = new(_identity);

        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(trustedPerson, testSet));
        Assert.AreEqual("Ce numéro de téléphone n'est pas valide. Seuls les numéros luxembourgeois, français, allemands et belges sont acceptés", exception.InnerException?.Message);
    }

    #endregion

    #region Invalid Birthdate

    [TestMethod]
    public void WrongBirthDate_TooYoung_ShouldThrowAnException()
    {
        string[] testSet = ["1", "+33678451278", "15/06/2015"];

        TrustedPerson trustedPerson = new(_identity);

        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(trustedPerson, testSet));
        Assert.AreEqual("Seul un adulte peut venir récupérer l'enfant.", exception.InnerException?.Message);

    }

    [TestMethod]
    public void WrongBirthDate_InvalidFormat_ShouldThrowAnException()
    {
        string[] testSet = ["1", "+33678451278", "20150615"];

        TrustedPerson trustedPerson = new(_identity);

        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(trustedPerson, testSet));
    }

    #endregion

    #endregion
    #endregion
}