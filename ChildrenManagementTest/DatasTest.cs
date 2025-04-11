using ChildrenManagementClasses;
using staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class DatasTest
{
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

    #region AddAnEntryPersonToDictionary
    [TestMethod]
    public void AddATrustedPersonAlreadyInDictionary_ShouldBeOK()
    {
        Datas.TrustedPeopleDictionary.Add(1234567894562, new TrustedPerson(new Identity(1234567894562, "Martin", "Cécilia", Nationalities.Luxembourgish), RelationshipToChild.Sister, "+35246788912", new DateTime(2006, 02, 25)));

        Datas.AddAnEntryPersonToDictionary(Datas.TrustedPeopleDictionary, new TrustedPerson(new Identity(1234567894562, "Martin", "Cécilia", Nationalities.Luxembourgish), RelationshipToChild.Sister, "+35246788912", new DateTime(2006, 02, 25)));

        Assert.AreEqual("Cécilia", Datas.TrustedPeopleDictionary[1234567894562].Identity.Firstname);

    }

    [TestMethod]
    public void AddATrustedPersonWithTheSameIDThanAnOther_ShouldThrowAnException()
    {
        Datas.TrustedPeopleDictionary.Add(1234567894562, new TrustedPerson(new Identity(1234567894562, "Martin", "Cécilia", Nationalities.Luxembourgish), RelationshipToChild.Sister, "+35246788912", new DateTime(2006, 02, 25)));
        TrustedPerson anotherOneWithSameId = new(new Identity(1234567894562, "VanHoenecker", "Charline", Nationalities.Belgian), RelationshipToChild.Mother, "+35246788912", new DateTime(1998, 02, 25));


        var exception = Assert.ThrowsException<ArgumentException>(() => Datas.AddAnEntryPersonToDictionary(Datas.TrustedPeopleDictionary, anotherOneWithSameId));
        Assert.AreEqual("Une autre personne est enregistrée avec le même Id. Veuillez réitérez l'inscription en vérifiant le matricule.", exception.Message);
    }
    #endregion
    #region Initialization
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


        Datas.InitializeGroups();

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
    public void HandleGroupFile_ShouldFillPropGroupOfChild()
    {
        Educator educator = new(new Identity(1472583693692, "DaSilva", "Maria", Nationalities.Portuguese), ChildTypes.Baby, "");
        Datas.EducatorsDictionary.Add(1472583693692, educator);

        Child child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "");
        Datas.ChildrenDictionary.Add(1234567894561, child);


        Datas.InitializeGroups();

        Assert.AreEqual("Les Nooooooon!", child.Group?.Name);

    }
    #endregion
}