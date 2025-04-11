using System.Text.Json;
using ChildrenManagement.Classes;
using ChildrenManagement.staticClasses;

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

        foreach (string filepath in Directory.EnumerateFiles("pictures/", "*.jpg", SearchOption.AllDirectories))
        {
            File.Delete(filepath);
        }


    }
    #region DownloadPictures

    private Child _child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24));
    private Educator _educator = new(new Identity(1472583693692, "Deschamps", "Céleste", Nationalities.French), ChildTypes.Baby, "");

    [TestMethod]
    public async Task DownloadProfilePictureWithChildren_ShouldCreateAFileInChildren()
    {
        _child.PicturePath = "https://cdn.pixabay.com/photo/2017/10/31/03/20/dominican-republic-2904164_640.jpg";

        (string pictureName, PersonPicturable person) = await DAL.DownloadProfilePicture(_child.PicturePath, _child);

        string expectedPictureName = "profilePic_1234567894561_Martin-Amy.jpg";

        Assert.AreEqual(expectedPictureName, pictureName);

        Assert.IsTrue(Path.Exists(Path.Combine(DAL._childrenPicDirectoryPath, pictureName)));
    }


    [TestMethod]
    public async Task DownloadProfilePictureWithEducator_ShouldCreateAFileInEducators()
    {
        _educator.PicturePath = "https://cdn.pixabay.com/photo/2017/03/02/20/25/woman-2112292_640.jpg";

        (string pictureName, PersonPicturable person) = await DAL.DownloadProfilePicture(_educator.PicturePath, _educator);

        string expectedPictureName = "profilePic_1472583693692_Deschamps-Céleste.jpg";

        Assert.AreEqual(expectedPictureName, pictureName);

        Assert.IsTrue(Path.Exists(Path.Combine(DAL._educatorsPicDirectoryPath, pictureName)));

    }

    [TestMethod]
    public async Task DownloadProfilePictureWithEducator_ShouldChangeEducatorPicturePath()
    {
        _educator.PicturePath = "https://cdn.pixabay.com/photo/2017/03/02/20/25/woman-2112292_640.jpg";
        Datas.EducatorsDictionary.Add(_educator.Identity.Id, _educator);

        await DAL.DownloadAllPictures();

        string expectedPictureName = "profilePic_1472583693692_Deschamps-Céleste.jpg";

        Assert.AreEqual(expectedPictureName, Datas.EducatorsDictionary[1472583693692].PicturePath);

    }

    [TestMethod]
    public void IfNoPicturePath_ShouldNotCreateTasksDownloading()
    {

        _child.PicturePath = "";

        List<Task<(string, PersonPicturable)>> tasksList = DAL.CreateTaskDownloading(Datas.ChildrenDictionary);

        Assert.AreEqual(0, tasksList.Count);

    }

    [TestMethod]
    public void IfAllPicturesAlreadyDownloaded_ShouldNotCreateTasksDownloading()
    {

        _child.PicturePath = @"pictures\children\profilePic_1234567894561_Martin-Amy.jpg";

        List<Task<(string, PersonPicturable)>> tasksList = DAL.CreateTaskDownloading(Datas.ChildrenDictionary);

        Assert.AreEqual(0, tasksList.Count);

    }

    #endregion

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