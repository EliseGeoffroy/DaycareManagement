using System.Text.Json;
using ChildrenManagementClasses;
using staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class DALTest
{
    [TestInitialize]
    public void TestInitialize()
    {
        Datas.ChildrenDictionary.Clear();
        Datas.EducatorsDictionary.Clear();
        Datas.TrustedPeopleDictionary.Clear();
        Datas.GroupDictionary.Clear();
    }

    #region RegistrationIntoFile
    [TestMethod]
    public void RegisterEducatorsInAFile()
    {
        Educator educator = new(new Identity(1472583693692, "Deschamps", "Céleste", Nationalities.French), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);

        DAL.RegisterEducatorsInAFile();
        string text = File.ReadAllText(DAL._educatorFilePath);
        var contenu = JsonSerializer.Deserialize<List<JSONEducator>>(text) ?? [];

        Assert.AreEqual(1, contenu.Count);
        Assert.AreEqual(1472583693692, contenu[0].ID);
        Assert.AreEqual("Deschamps", contenu[0].Name);
        Assert.AreEqual("Céleste", contenu[0].Firstname);
        Assert.AreEqual(Nationalities.French, contenu[0].Nationality);
        Assert.AreEqual(ChildTypes.Baby, contenu[0].PreferenceType);
    }

    [TestMethod]
    public void RegisterTrustedPeopleInAFile()
    {
        TrustedPerson trustedPerson = new(new Identity(7894561234562, "Martin", "Lily", Nationalities.French), RelationshipToChild.Mother, "+33673398550", new DateTime(1992, 10, 17));
        Datas.TrustedPeopleDictionary.Add(7894561234562, trustedPerson);

        DAL.RegisterTrustedPeopleInAFile();
        string text = File.ReadAllText(DAL._trustedPeopleFilePath);
        var contenu = JsonSerializer.Deserialize<List<JSONTrustedPerson>>(text) ?? [];

        Assert.AreEqual(1, contenu.Count);
        Assert.AreEqual(7894561234562, contenu[0].ID);
        Assert.AreEqual("Martin", contenu[0].Name);
        Assert.AreEqual("Lily", contenu[0].Firstname);
        Assert.AreEqual(Nationalities.French, contenu[0].Nationality);
        Assert.AreEqual(RelationshipToChild.Mother, contenu[0].RelationshipToChild);
        Assert.AreEqual("+33673398550", contenu[0].PhoneNumber);
        Assert.AreEqual(new DateTime(1992, 10, 17), contenu[0].BirthDate);
    }

    [TestMethod]
    public void RegisterChildrenInAFile()
    {
        TrustedPerson trustedPerson = new(new Identity(7894561234562, "Martin", "Lily", Nationalities.French), RelationshipToChild.Mother, "+33673398550", new DateTime(1992, 10, 17));
        Datas.TrustedPeopleDictionary.Add(7894561234562, trustedPerson);

        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        child.AddATrustedPerson(trustedPerson);
        Datas.ChildrenDictionary.Add(1234567894561, child);

        DAL.RegisterChildrenInAFile();
        string text = File.ReadAllText(DAL._childrenFilePath);
        var contenu = JsonSerializer.Deserialize<List<JSONChild>>(text) ?? [];

        Assert.AreEqual(1, contenu.Count);
        Assert.AreEqual(1234567894561, contenu[0].ID);
        Assert.AreEqual("Martin", contenu[0].Name);
        Assert.AreEqual("Amy", contenu[0].Firstname);
        Assert.AreEqual(Nationalities.French, contenu[0].Nationality);
        Assert.AreEqual(new DateTime(2023, 10, 24), contenu[0].BirthDate);
        Assert.AreEqual(1, contenu[0].TrustedPeopleIds.Length);
        Assert.AreEqual(7894561234562, contenu[0].TrustedPeopleIds[0]);
    }

    [TestMethod]
    public void RegisterGroupsInAFile()
    {

        Educator educator = new(new Identity(1472583693692, "Deschamps", "Céleste", Nationalities.French), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);


        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        Datas.ChildrenDictionary.Add(1234567894561, child);

        Group group = new("Les pouet-pouet", 10, ChildTypes.Toddler, educator);
        group.AddAChild(child);
        Datas.GroupDictionary.Add("Les pouet-pouet", group);

        DAL.RegisterGroupsInAFile();
        string text = File.ReadAllText(DAL._groupFilePath);
        var contenu = JsonSerializer.Deserialize<List<JSONGroup>>(text) ?? [];

        Assert.AreEqual(1, contenu.Count);
        Assert.AreEqual("Les pouet-pouet", contenu[0].Name);
        Assert.AreEqual(10, contenu[0].FullCapacity);
        Assert.AreEqual(1, contenu[0].CurrentCapacity);
        Assert.AreEqual(ChildTypes.Toddler, contenu[0].ChildType);
        Assert.AreEqual(1, contenu[0].ChildrenID.Length);
        Assert.AreEqual(1234567894561, contenu[0].ChildrenID[0]);
        Assert.AreEqual(1, contenu[0].EducatorsID.Length);
        Assert.AreEqual(1472583693692, contenu[0].EducatorsID[0]);
    }

    #endregion
}