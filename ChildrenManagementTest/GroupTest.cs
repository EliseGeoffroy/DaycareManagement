
using ChildrenManagementClasses;
using staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class GroupTest
{

    private readonly Group _groupBabyOK = new("Les cacahouètes", 10, ChildTypes.Baby, new Educator(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby));
    private readonly Group _groupToddlerOK = new("Les Cascadeurs", 1, ChildTypes.Toddler, new Educator(new Identity(1472583693696, "Cacciatore", "Nicola", Nationalities.Italian), ChildTypes.Toddler));
    private readonly Group _groupKidOK = new("Les Pourquoi ?", 10, ChildTypes.Kid, new Educator(new Identity(1472583693694, "VanRichter", "Anna", Nationalities.Luxembourgish), ChildTypes.Kid));
    private readonly Group _groupWithJustOnePlace = new("La crevette", 1, ChildTypes.Baby, new Educator(new Identity(1472583693695, "Schmidt", "Klaus", Nationalities.German), ChildTypes.Baby));
    private readonly Group _groupWithNoMorePlace = new("Les coquins", 0, ChildTypes.Toddler, new Educator(new Identity(1472583693693, "Roussel", "Aurélie", Nationalities.Belgian), ChildTypes.Toddler));

    [TestInitialize]
    public void TestInitialize()
    {
        Datas.GroupDictionary.Clear();
    }


    [DataTestMethod]
    [DataRow(25, 05, 2024, "Les cacahouètes", DisplayName = "Baby")]
    [DataRow(25, 05, 2023, "Les Cascadeurs", DisplayName = "Toddler")]
    [DataRow(25, 05, 2022, "Les Pourquoi ?", DisplayName = "Kid")]
    public void FindAGroup_ShouldRegisterChildInRightGroup(int day, int month, int year, string groupName)
    {

        Child child = new(new Identity(1234567894561, "Poussin", "Côme", Nationalities.Luxembourgish), new DateTime(year, month, day));

        Datas.GroupDictionary.Add("Les cacahouètes", _groupBabyOK);
        Datas.GroupDictionary.Add("Les Cascadeurs", _groupToddlerOK);
        Datas.GroupDictionary.Add("Les Pourquoi ?", _groupKidOK);


        Group.FindAGroup(child);

        Assert.AreEqual(groupName, child.Group?.Name);

    }

    [TestMethod]
    public void IfLastPlaceInAgeRange_ShouldSendAnEvent()
    {
        Child child = new(new Identity(1234567894561, "Poussin", "Côme", Nationalities.Luxembourgish), new DateTime(2024, 05, 25));
        Datas.GroupDictionary.Add("La crevette", _groupWithJustOnePlace);
        Datas.GroupDictionary.Add("Les Cascadeurs", _groupToddlerOK);
        Datas.GroupDictionary.Add("Les Pourquoi ?", _groupKidOK);


        bool eventRaised = false;

        Group.JustOnePlaceAvailableEvent += (sender, childType) => eventRaised = true;

        Group.FindAGroup(child);

        Assert.IsTrue(eventRaised);

    }

    [TestMethod]
    public void IfNoPlaceInAgeRange_ShouldSendAnException()
    {
        Child child = new(new Identity(1234567894562, "VanBoost", "Alex", Nationalities.Belgian), new DateTime(2023, 05, 25));
        Datas.GroupDictionary.Add("Les cacahouètes", _groupBabyOK);
        Datas.GroupDictionary.Add("Les coquins", _groupWithNoMorePlace);
        Datas.GroupDictionary.Add("Les Pourquoi ?", _groupKidOK);

        var exception = Assert.ThrowsException<InvalidOperationException>(() => Group.FindAGroup(child));
        Assert.AreEqual("Aucune place n'est disponible pour cet âge. L'inscription est annulée.", exception.Message);
    }

    [TestMethod]
    public void IfTryToAddChildInaGroupWithNoLeftPlaces_ShouldThrowAnEception()
    {
        Child child = new(new Identity(1234567894562, "VanBoost", "Alex", Nationalities.Belgian), new DateTime(2023, 05, 25));


        var exception = Assert.ThrowsException<InvalidOperationException>(() => _groupWithNoMorePlace.AddAChild(child));
        Assert.AreEqual("On ne peut plus ajouter d'enfants dans ce groupe.", exception.Message);
    }

}