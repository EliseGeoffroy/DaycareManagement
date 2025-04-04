using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Moq;

namespace ChildrenManagementTest
{

    [TestClass]
    public class IdentityFormTest
    {
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
    }

    [TestClass]
    public class ChildFormTest
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


            string?[] testSet = [$"{day}/{month}/{year}", null];

            Child child = new(_identity);
            Utilities.InputDataTest(child, testSet);

            Assert.AreEqual(ageInMonth, child.AgeInMonth);

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

    }

    [TestClass]
    public class TrustedPersonFormTest
    {
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
    }
    public interface IDateTimeProvider
    {
        DateTime Today { get; }
    }
}