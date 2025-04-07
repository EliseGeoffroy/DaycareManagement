using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace ChildrenManagementTest;

[TestClass]
public sealed class AddTrustedPeopleTest
{

    private Child _child = new Child(new Identity(1234567894561, "Martin", "Hugo", Nationalities.Belgian), new DateTime(2024, 03, 30));
    private TrustedPerson _trustedPerson = new TrustedPerson(new Identity(1234567894562, "Martin", "Cécilia", Nationalities.Luxembourgish), RelationshipToChild.Sister, "+35246788912", new DateTime(2006, 02, 25));
    private TrustedPerson _trustedPerson1 = new TrustedPerson(new Identity(1234567894563, "Martin", "Laurent", Nationalities.French), RelationshipToChild.Father, "+33654788912", new DateTime(1989, 02, 12));
    private TrustedPerson _trustedPerson2 = new TrustedPerson(new Identity(1234567894564, "Dos Santos", "Maria", Nationalities.Portuguese), RelationshipToChild.GrandParent, "+35246788913", new DateTime(1962, 05, 25));
    private TrustedPerson _trustedPerson3 = new TrustedPerson(new Identity(1234567894565, "Martin", "Emile", Nationalities.Luxembourgish), RelationshipToChild.GrandParent, "+35246788914", new DateTime(1955, 02, 25));
    private TrustedPerson _trustedPerson4 = new TrustedPerson(new Identity(1234567894566, "Martin", "Fabiana", Nationalities.Portuguese), RelationshipToChild.Mother, "+35246788912", new DateTime(1989, 10, 25));
    private TrustedPerson _trustedPerson5 = new TrustedPerson(new Identity(1234567894567, "Van Houten", "Berthe", Nationalities.Belgian), RelationshipToChild.GodParent, "+3256895689", new DateTime(1992, 07, 25));

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

}

[TestClass]
public class GroupTest
{

    private Group _groupBabyOK = new Group("Les cacahouètes", 10, ChildTypes.Baby, new Educator(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby));
    private Group _groupToddlerOK = new Group("Les Nooooooon!", 1, ChildTypes.Toddler, new Educator(new Identity(1472583693696, "Cacciatore", "Nicola", Nationalities.Italian), ChildTypes.Toddler));
    private Group _groupKidOK = new Group("Les Pourquoi ?", 10, ChildTypes.Kid, new Educator(new Identity(1472583693694, "VanRichter", "Anna", Nationalities.Luxembourgish), ChildTypes.Kid));
    private Group _groupWithJustOnePlace = new Group("La crevette", 1, ChildTypes.Baby, new Educator(new Identity(1472583693695, "Schmidt", "Klaus", Nationalities.German), ChildTypes.Baby));
    private Group _groupWithNoMorePlace = new Group("Les coquins", 0, ChildTypes.Toddler, new Educator(new Identity(1472583693693, "Roussel", "Aurélie", Nationalities.Belgian), ChildTypes.Toddler));

    [DataTestMethod]
    [DataRow(25, 05, 2024, "Les cacahouètes", DisplayName = "Baby")]
    [DataRow(25, 05, 2023, "Les Nooooooon!", DisplayName = "Toddler")]
    [DataRow(25, 05, 2022, "Les Pourquoi ?", DisplayName = "Kid")]
    public void FindAGroup_ShouldRegisterChildInRightGroup(int day, int month, int year, string groupName)
    {

        Child child = new(new Identity(1234567894561, "Poussin", "Côme", Nationalities.Luxembourgish), new DateTime(year, month, day));
        GroupList groupList = new([_groupBabyOK, _groupToddlerOK, _groupKidOK]);

        groupList.FindAGroup(child);

        Assert.AreEqual(groupName, child.Group?.Name);

    }

    [TestMethod]
    public void IfLastPlaceInAgeRange_ShouldSendAnEvent()
    {
        Child child = new(new Identity(1234567894561, "Poussin", "Côme", Nationalities.Luxembourgish), new DateTime(2024, 05, 25));
        GroupList groupList = new([_groupWithJustOnePlace, _groupToddlerOK, _groupKidOK]);

        bool eventRaised = false;

        GroupList.JustOnePlaceAvailableEvent += (sender, childType) => eventRaised = true;

        groupList.FindAGroup(child);

        Assert.IsTrue(eventRaised);

    }

    [TestMethod]
    public void IfNoPlaceInAgeRange_ShouldSendAnException()
    {
        Child child = new(new Identity(1234567894562, "VanBoost", "Alex", Nationalities.Belgian), new DateTime(2023, 05, 25));
        GroupList groupList = new([_groupBabyOK, _groupWithNoMorePlace, _groupKidOK]);

        Assert.ThrowsException<InvalidOperationException>(() => groupList.FindAGroup(child));
    }

}
