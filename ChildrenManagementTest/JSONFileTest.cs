using System.Text.Json;
using ChildrenManagement;
using staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class JSONFileTest
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Directory.SetCurrentDirectory(@"C:\Users\geoff\Desktop\C#.Net\DaycareManagement\ChildrenManagementTest\data\files");
    }

    [TestInitialize]
    public void TestInitialize()
    {
        Datas.ChildrenDictionary.Clear();
        Datas.EducatorsDictionary.Clear();
        Datas.TrustedPeopleDictionary.Clear();
        Datas.GroupDictionary.Clear();

        List<string> filesPath = Directory.EnumerateFiles("refFiles/").ToList();

        foreach (string path in filesPath)
        {
            File.Copy(path, Path.GetFileName(path), true);
        }

    }

    [TestMethod]
    public void HandleEducatorFile_ShouldCreateEducatorInstances()
    {
        Datas.InitializeEducators();

        Assert.AreEqual(6, Datas.EducatorsDictionary.Count);
        Assert.AreEqual("DaSilva", Datas.EducatorsDictionary[1472583693692].Identity.Name);
        Assert.AreEqual("Aurélie", Datas.EducatorsDictionary[1472583693693].Identity.Firstname);
        Assert.AreEqual(Nationalities.Luxembourgish, Datas.EducatorsDictionary[1472583693694].Identity.Nationality);
        Assert.AreEqual(ChildTypes.Baby, Datas.EducatorsDictionary[1472583693695].PreferenceType);
        Assert.AreEqual("https://cdn.pixabay.com/photo/2016/09/24/03/20/man-1690965_640.jpg", Datas.EducatorsDictionary[1472583693696].PicturePath);
    }

    [TestMethod]
    public void HandleTrustedPeopleFile_ShouldCreateTrustedPersonInstances()
    {
        Datas.InitializeTrustedPeople();

        Assert.AreEqual(1, Datas.TrustedPeopleDictionary.Count);
        Assert.AreEqual("Martin", Datas.TrustedPeopleDictionary[7894561234562].Identity.Name);
        Assert.AreEqual("Lily", Datas.TrustedPeopleDictionary[7894561234562].Identity.Firstname);
        Assert.AreEqual(Nationalities.French, Datas.TrustedPeopleDictionary[7894561234562].Identity.Nationality);
        Assert.AreEqual(RelationshipToChild.Mother, Datas.TrustedPeopleDictionary[7894561234562].RelationshipToChild);
        Assert.AreEqual("+33673398550", Datas.TrustedPeopleDictionary[7894561234562].PhoneNumber);
        Assert.AreEqual(new DateTime(1992, 10, 17), Datas.TrustedPeopleDictionary[7894561234562].BirthDate);

    }

    [TestMethod]
    public void HandleChildrenFile_ShouldCreateChildInstances()
    {
        TrustedPerson trustedPerson = new(new Identity(7894561234562, "Martin", "Lily", Nationalities.French), RelationshipToChild.Mother, "+33673398550", new DateTime(1992, 10, 17));
        Datas.TrustedPeopleDictionary.Add(7894561234562, trustedPerson);

        Datas.InitializeChildren();

        Assert.AreEqual(1, Datas.ChildrenDictionary.Count);
        Assert.AreEqual("Martin", Datas.ChildrenDictionary[1234567894561].Identity.Name);
        Assert.AreEqual("Amy", Datas.ChildrenDictionary[1234567894561].Identity.Firstname);
        Assert.AreEqual(Nationalities.French, Datas.ChildrenDictionary[1234567894561].Identity.Nationality);
        Assert.AreEqual(new DateTime(2023, 10, 24), Datas.ChildrenDictionary[1234567894561].BirthDate);
        Assert.AreEqual("https://cdn.pixabay.com/photo/2015/06/12/21/58/child-807547_640.jpg", Datas.ChildrenDictionary[1234567894561].PicturePath);
        Assert.AreEqual(trustedPerson, Datas.ChildrenDictionary[1234567894561].ContactList[0]);

    }

    [TestMethod]
    public void HandleGroupFile_ShouldCreateGroupInstances()
    {
        Educator educator = new(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);

        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        Datas.ChildrenDictionary.Add(1234567894561, child);


        Datas.InitializeGroup();

        Assert.AreEqual(2, Datas.GroupDictionary.Count);
        Assert.AreEqual(ChildTypes.Baby, Datas.GroupDictionary["Les cacahouètes"].ChildType);
        Assert.AreEqual(educator, Datas.GroupDictionary["Les cacahouètes"].Educators[0]);
        Assert.AreEqual(10, Datas.GroupDictionary["Les cacahouètes"].FullCapacity);
        Assert.AreEqual(0, Datas.GroupDictionary["Les cacahouètes"].CurrentCapacity);

        Assert.AreEqual(ChildTypes.Toddler, Datas.GroupDictionary["Les Nooooooon!"].ChildType);
        Assert.AreEqual(child, Datas.GroupDictionary["Les Nooooooon!"].Children[0]);
        Assert.AreEqual(1, Datas.GroupDictionary["Les Nooooooon!"].FullCapacity);
        Assert.AreEqual(1, Datas.GroupDictionary["Les Nooooooon!"].CurrentCapacity);
        Assert.AreEqual(0, Datas.GroupDictionary["Les Nooooooon!"].AvailableCapacity);
    }


    [TestMethod]
    public void HandleGroupFile_ShouldCreateAGroupListInstance()
    {
        Educator educator = new(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);

        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        Datas.ChildrenDictionary.Add(1234567894561, child);


        GroupList groupList = Datas.InitializeGroup();

        Assert.AreEqual(2, groupList.Groups.Count);
        Assert.AreEqual(10, groupList.NbPlaces[0]);
        Assert.AreEqual(0, groupList.NbPlaces[1]);
        Assert.AreEqual(0, groupList.NbPlaces[2]);

    }

    [TestMethod]
    public void HandleGroupFile_ShouldFillPropGroupOfChild()
    {
        Educator educator = new(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);

        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        Datas.ChildrenDictionary.Add(1234567894561, child);


        Datas.InitializeGroup();

        Assert.AreEqual("Les Nooooooon!", child.Group?.Name);

    }


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
}