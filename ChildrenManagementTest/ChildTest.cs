using System.Reflection;
using ChildrenManagementClasses;
using staticClasses;

namespace ChildrenManagementTest;

[TestClass]

public class ChildTest
{

    #region Form
    private readonly Identity _identity = new(7894561237894L, "Oliveira", "Yasmine", Nationalities.Portuguese);

    #region RightCases
    [TestMethod]
    public void CorrectFormularChild_ShouldCreateCorrectChild()
    {

        string[] testSet = ["02/10/2024", "https://www.pexels.com/fr-fr/photo/nature-dehors-mignon-etre-assis-27772486/"];

        Child child = new(_identity);
        Utilities.InputDataTest<Child>(child, testSet);

        Assert.AreEqual(new DateTime(2024, 10, 02), child.BirthDate);
        Assert.AreEqual(6, child.AgeInMonth);
        Assert.AreEqual("https://www.pexels.com/fr-fr/photo/nature-dehors-mignon-etre-assis-27772486/", child.PicturePath);
    }

    [TestMethod]
    public void FormularChildWithOutPicturePath_IsCorrect()
    {

        string?[] testSet = ["02/10/2024", null];

        Child child = new(_identity);
        Utilities.InputDataTest(child, testSet);

        Assert.AreEqual(new DateTime(2024, 10, 02), child.BirthDate);
        Assert.AreEqual(6, child.AgeInMonth);
    }

    [DataTestMethod]
    [DataRow(2023, 06, 23, 21, DisplayName = "More than a year ago + day> day Today")]
    [DataRow(2023, 06, 01, 22, DisplayName = "More than a year ago + day< day Today")]
    [DataRow(2025, 01, 01, 3, DisplayName = "In the same year + day< day Today")]
    [DataRow(2025, 01, 10, 2, DisplayName = "In the same year + day> day Today")]
    [DataRow(2024, 01, 10, 14, DisplayName = "One year ago + day> day Today")]
    [DataRow(2024, 01, 01, 15, DisplayName = "One year ago + day< day Today")]
    public void CalculationAgeInMonthTest(int year, int month, int day, int ageInMonth)
    {


        DateTime birthDate = new(year, month, day);
        DateTime refDate = new(2025, 04, 03);
        int calculatedAge = Utilities.CalculateAgeInMonth(birthDate, refDate);

        Assert.AreEqual(ageInMonth, calculatedAge);

    }

    [DataTestMethod]
    [DataRow(2022, 06, 23, ChildTypes.Kid, DisplayName = "Kid")]
    [DataRow(2023, 06, 23, ChildTypes.Toddler, DisplayName = "Toddler")]
    [DataRow(2025, 03, 23, ChildTypes.Baby, DisplayName = "Baby")]

    public void ChildTypesTest(int year, int month, int day, ChildTypes childTypes)
    {


        string?[] testSet = [$"{day}/{month}/{year}", null];

        Child child = new(_identity);
        Utilities.InputDataTest(child, testSet);

        Assert.AreEqual(childTypes, child.ChildType);

    }

    #endregion

    #region WrongCases

    #region Lack of Data

    [TestMethod]
    public void NoBirthDate_ShouldThrowAnException()
    {
        string[] testSet = ["", ""];

        Child child = new(_identity);
        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(child, testSet));
    }

    #endregion

    #region Invalid BirthDate

    [TestMethod]
    public void InvalidFormatBirthDate_ShouldThrowAnException()
    {
        string[] testSet = ["20241023", ""];

        Child child = new(_identity);
        Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(child, testSet));
    }

    [TestMethod]
    public void InvalidBirthDate_TooOld_ShouldThrowAnException()
    {
        string[] testSet = ["17/07/1992", ""];

        Child child = new(_identity);
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(child, testSet));
        Assert.AreEqual("Votre enfant est trop âgé pour être gardé en crèche. Veuillez vous rapprocher du foyer de jour.", exception.InnerException?.Message);
    }

    [TestMethod]
    public void InvalidBirthDate_NotBorn_ShouldThrowAnException()
    {
        string[] testSet = ["17/05/2025", ""];

        Child child = new(_identity);
        var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(child, testSet));
        Assert.AreEqual("Nous n'acceptons pas l'inscription des enfants avant leur naissance.", exception.InnerException?.Message);
    }
    #endregion

    #endregion

    #endregion

    #region AddTrustedPerson
    private readonly Child _child = new(new Identity(1234567894561, "Martin", "Hugo", Nationalities.Belgian), new DateTime(2024, 03, 30));
    private readonly TrustedPerson _trustedPerson = new(new Identity(1234567894562, "Martin", "Cécilia", Nationalities.Luxembourgish), RelationshipToChild.Sister, "+35246788912", new DateTime(2006, 02, 25));
    private readonly TrustedPerson _trustedPerson1 = new(new Identity(1234567894563, "Martin", "Laurent", Nationalities.French), RelationshipToChild.Father, "+33654788912", new DateTime(1989, 02, 12));
    private readonly TrustedPerson _trustedPerson2 = new(new Identity(1234567894564, "Dos Santos", "Maria", Nationalities.Portuguese), RelationshipToChild.GrandParent, "+35246788913", new DateTime(1962, 05, 25));
    private readonly TrustedPerson _trustedPerson3 = new(new Identity(1234567894565, "Martin", "Emile", Nationalities.Luxembourgish), RelationshipToChild.GrandParent, "+35246788914", new DateTime(1955, 02, 25));
    private readonly TrustedPerson _trustedPerson4 = new(new Identity(1234567894566, "Martin", "Fabiana", Nationalities.Portuguese), RelationshipToChild.Mother, "+35246788912", new DateTime(1989, 10, 25));
    private readonly TrustedPerson _trustedPerson5 = new(new Identity(1234567894567, "Van Houten", "Berthe", Nationalities.Belgian), RelationshipToChild.GodParent, "+3256895689", new DateTime(1992, 07, 25));



    [TestCleanup]
    public void TestCleanupMethod()
    {
        _child.ContactList.Clear();
    }


    [TestMethod]
    public void AddATrustedPersonToAChildWithLessThan5People_ShouldBeOk()
    {
        _child.AddATrustedPerson(_trustedPerson);

        Assert.AreEqual(1234567894562, _child.ContactList[0].Identity.Id);
    }

    [TestMethod]
    public void AddATrustedPersonToAChildWithMoreThan5People_ShouldThrowAnException()
    {
        _child.AddATrustedPerson(_trustedPerson);
        _child.AddATrustedPerson(_trustedPerson1);
        _child.AddATrustedPerson(_trustedPerson2);
        _child.AddATrustedPerson(_trustedPerson3);
        _child.AddATrustedPerson(_trustedPerson4);


        var exception = Assert.ThrowsException<InvalidOperationException>(() => _child.AddATrustedPerson(_trustedPerson5));
        Assert.AreEqual("Vous ne pouvez pas renseigner plus de 5 contacts d'urgence pour votre enfant", exception.Message);
    }

    #endregion
}
