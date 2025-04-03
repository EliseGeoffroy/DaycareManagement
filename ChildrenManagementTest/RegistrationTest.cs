using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Moq;

namespace ChildrenManagementTest
{

    [TestClass]
    public class IdentityTest
    {

        [TestMethod]
        public void CorrectFormularIdentity_MustCreateCorrectIdentity()
        {

            string[] testSet = ["1234567891234", "Dupont", "Anna", "2"];

            Identity identity = new();
            Utilities.InputDataTest(identity, testSet);

            Assert.AreEqual(1234567891234L, identity.Id);
            Assert.AreEqual("Dupont", identity.Name);
            Assert.AreEqual("Anna", identity.Firstname);
            Assert.AreEqual(Nationalities.German, identity.Nationality);
        }

        [DataTestMethod]
        [DataRow("1234567891234", "", "Anna", "2", DisplayName = "Without Name")]
        [DataRow("1234567891234", "Dupont", "", "2", DisplayName = "Without Firstname")]
        //   [ExpectedException(typeof(Exception))]
        public void FormularWithoutAData_MustThrowAnException(string id, string name, string firstname, string nationality)
        {
            string?[] testSet = [id, name, firstname, nationality];
            Identity identity = new();

            Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
        }

        [DataTestMethod]
        [DataRow("", "Dupont", "Anna", "2", DisplayName = "Without Id")]
        [DataRow("1234567891234", "Dupont", "Anna", "", DisplayName = "Without Nationality")]
        public void FormularWithoutAData_MustThrowAnException2(string id, string name, string firstname, string nationality)
        {
            string?[] testSet = [id, name, firstname, nationality];
            Identity identity = new();

            Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));
        }

        [TestMethod]
        public void FormularWithInvalidIDWithoutEnoughDigit_MustThrownAnException()
        {

            string[] testSet = ["123456789", "Dupont", "Anna", "2"];

            Identity identity = new();
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
            Assert.AreEqual("Un matricule luxembourgeois doit comporter 13 chiffres.", exception.InnerException?.Message);
        }

        [TestMethod]
        public void FormularWithInvalidIDWithChar_MustThrownAnException()
        {

            string[] testSet = ["ABCDEFGHIJKLM", "Dupont", "Anna", "2"];

            Identity identity = new();
            Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));
        }

        [DataTestMethod]
        [DataRow("A", DisplayName = "Not enough char")]
        [DataRow("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", DisplayName = "Too much char")]
        public void FormularWithInvalidName_MustThrownAnException(string name)
        {

            string[] testSet = ["1234567891234", name, "Anna", "2"];

            Identity identity = new();
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
            Assert.AreEqual("Le nom doit comporter de 2 à 70 caractères", exception.InnerException?.Message);
        }

        [DataTestMethod]
        [DataRow("A", DisplayName = "Not enough char")]
        [DataRow("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", DisplayName = "Too much char")]
        public void FormularWithInvalidFirstname_MustThrownAnException(string firstname)
        {

            string[] testSet = ["1234567891234", "Dupont", firstname, "2"];

            Identity identity = new();
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
            Assert.AreEqual("Le prénom doit comporter de 2 à 70 caractères", exception.InnerException?.Message);
        }

        [TestMethod]
        public void FormularWithInvalidNationalityNumber_MustThrownAnException()
        {

            string[] testSet = ["1234567891234", "Dupont", "Anna", "8"];

            Identity identity = new();
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(identity, testSet));
            Assert.AreEqual("Votre réponse ne correspond à aucune Nationalité connue", exception.InnerException?.Message);
        }

        [TestMethod]
        public void FormularWithInvalidNationality_MustThrownAnException()
        {

            string[] testSet = ["1234567891234", "Dupont", "Anna", "Française"];

            Identity identity = new();
            Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(identity, testSet));

        }





    }

    [TestClass]
    public class ChildTest
    {


        private Identity _identity = new(7894561237894L, "Oliveira", "Yasmine", Nationalities.Portuguese);

        [AssemblyInitialize]
        public static void AssemblyInitializeMethod(TestContext context)
        {
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(m => m.Today).Returns(new DateTime(2024, 4, 3));
        }

        #region RightCases
        [TestMethod]
        public void CorrectFormularChild_MustCreateCorrectChild()
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


            string?[] testSet = [$"{day}/{month}/{year}", null];

            Child child = new(_identity);
            Utilities.InputDataTest(child, testSet);

            Assert.AreEqual(ageInMonth, child.AgeInMonth);

        }

        #endregion

        #region WrongCases

        [TestMethod]
        public void InvalidFormatBirthDate_MustThrowAnException()
        {
            string[] testSet = ["20241023", ""];

            Child child = new(_identity);
            Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(child, testSet));
        }

        [TestMethod]
        public void InvalidBirthDate_TooOld_MustThrowAnException()
        {
            string[] testSet = ["17/07/1992", ""];

            Child child = new(_identity);
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(child, testSet));
            Assert.AreEqual("Votre enfant est trop âgé pour être gardé en crèche. Veuillez vous rapprocher du foyer de jour.", exception.InnerException?.Message);
        }

        [TestMethod]
        public void InvalidBirthDate_NotBorn_MustThrowAnException()
        {
            string[] testSet = ["17/05/2025", ""];

            Child child = new(_identity);
            var exception = Assert.ThrowsException<TargetInvocationException>(() => Utilities.InputDataTest(child, testSet));
            Assert.AreEqual("Nous n'acceptons pas l'inscription des enfants avant leur naissance.", exception.InnerException?.Message);
        }

        [TestMethod]
        public void NoBirthDate_MustThrowAnException()
        {
            string[] testSet = ["", ""];

            Child child = new(_identity);
            Assert.ThrowsException<FormatException>(() => Utilities.InputDataTest(child, testSet));
        }


        #endregion

    }

    public interface IDateTimeProvider
    {
        DateTime Today { get; }
    }
}