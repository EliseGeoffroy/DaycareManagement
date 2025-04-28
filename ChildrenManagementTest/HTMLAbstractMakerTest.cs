using System.Text.RegularExpressions;
using ChildrenManagement.Classes;
using ChildrenManagement.staticClasses;

namespace ChildrenManagementTest;

[TestClass]
public class HTMLAbstractMakerTest
{

    private const string MODELS_DIRECTORY = "Models";
    private const string HEADER_MODEL = "header.txt";
    private const string EDUCATORSDIV_MODEL = "educatorsDiv.txt";
    private const string CHILDRENDIV_MODEL = "childrenDiv.txt";
    private const string WHOLEHTML_MODEL = "wholeHTML.html";

    private static readonly Educator _educator = new(new Identity(1472583693696, "Cacciatore", "Nicola", Nationalities.Italian), ChildTypes.Toddler);
    private static readonly Educator _educator1 = new(new Identity(1472583693692, "Du Château", "Céleste", Nationalities.French), ChildTypes.Toddler, "profilePic_1472583693692_Du-Château-Céleste.jpg");
    private static readonly ChildrenManagement.Classes.Group _group = new("Les Cascadeurs", 5, ChildTypes.Toddler, _educator, _educator1);
    private static readonly Child _child = new(new Identity(1234567894561, "Martin", "Amy", Nationalities.French), new DateTime(2023, 10, 24), "profilePic_1234567894561_Martin-Amy.jpg");
    private static readonly Child _child1 = new(new Identity(1234567894563, "Boulanger", "Théo", Nationalities.Belgian), new DateTime(2023, 12, 13), "");


    private const RegexOptions options = RegexOptions.Singleline;
    private const string HEADER_REGEX = @"^\s+<header.+<h1>(?<groupName>.+)</h1>\s+<span>\((?<childType>\w+)\)</span>\s+</header>";

    private const string EDUCATORSDIV_REGEX = @"^\s*<div class=""educators.+?(?<educator><div class=""educator.+?<img\s+src=""(?<picture>[\w\.\\/-]*\.jpg)"".+?<p class=""firstname"">(?<firstname>[\w -]*)</p>\s+<p class=""name"">(?<name>[\w -]*)</p>\s+</div>\s+){2}</div>";

    private const string EDUCATOR_REGEX = @"<div class=""educator.+?<img\s+src=""(?<picture>[\w\.\\/-]*\.jpg)"".+?<p class=""firstname"">(?<firstname>[\w -]*)</p>\s+<p class=""name"">(?<name>[\w -]*)</p>\s+</div>\s+";
    private const string CHILDRENDIV_REGEX = @"^\s*<div class=""children.+?<h2>Enfants</h2>\s+<span>(?<currentcapacity>\d+)/(?<fullcapacity>\d+).+?(?<child><div class=""child.+?src=""(?<picture>[\w\.\\/-]*\.jpg)"".+?<p class=""firstname"">(?<firstname>[\w- ]*)</p>\s+<p class=""name"">(?<name>[\w- ]*)</p>\s+<p class=""birthDate"">(?<birthdate>\d{2}/\d{2}/\d{4})</p>\s+<p class=""age"">(?<age>\d+) mois</p>(\s+</div>){2}\s+){2}</div>$";

    private const string CHILD_REGEX = @"<div class=""child.+?src=""(?<picture>[\w\.\\/-]*\.jpg)"".+?<p class=""firstname"">(?<firstname>[\w- ]*)</p>\s+<p class=""name"">(?<name>[\w- ]*)</p>\s+<p class=""birthDate"">(?<birthdate>\d{2}/\d{2}/\d{4})</p>\s+<p class=""age"">(?<age>\d+) mois</p>(\s+</div>){2}\s+";


    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _group.AddAChild(_child);
        _group.AddAChild(_child1);
    }

    [TestMethod]
    public void CreatePicturePath_ForEducators_ShouldUseEducatorsDirectory()
    {
        string picturePath = HMTLAbstractMaker.CreatePicturePath(_educator1);

        Assert.AreEqual(@"..\..\pictures\educators\profilePic_1472583693692_Du-Château-Céleste.jpg", picturePath);
    }

    [TestMethod]
    public void CreatePicturePath_ForChildren_ShouldUseChildrenDirectory()
    {
        string picturePath = HMTLAbstractMaker.CreatePicturePath(_child);

        Assert.AreEqual(@"..\..\pictures\children\profilePic_1234567894561_Martin-Amy.jpg", picturePath);
    }

    [TestMethod]
    public void CreatePicturePath_WhenNoPicturePath_ShouldUseDefaultPic()
    {
        string picturePath = HMTLAbstractMaker.CreatePicturePath(_child1);

        Assert.AreEqual(@"..\..\pictures\profile-pic-default.jpg", picturePath);
    }

    [TestMethod]
    public void CreateHeaderTest_Structure()
    {
        string txt = HMTLAbstractMaker.CreateHeader(_group);

        var match = Regex.Match(txt, HEADER_REGEX, options);

        Assert.IsTrue(match.Success);
    }

    [TestMethod]
    public void CreateHeader_ShouldProvideRightGroupName()
    {
        string txt = HMTLAbstractMaker.CreateHeader(_group);

        var match = Regex.Match(txt, HEADER_REGEX, options);

        Assert.AreEqual("Les Cascadeurs", match.Groups["groupName"].Value);
    }

    [TestMethod]
    public void CreateHeader_ShouldProvideRightchildType()
    {
        string txt = HMTLAbstractMaker.CreateHeader(_group);

        var match = Regex.Match(txt, HEADER_REGEX, options);

        Assert.AreEqual("Bambin", match.Groups["childType"].Value);
    }



    [TestMethod]
    public void CreateEducatorsDivTest_Structure()
    {

        string txt = HMTLAbstractMaker.CreateEducatorsDiv(_group);

        var match = Regex.Match(txt, EDUCATORSDIV_REGEX, options);

        Assert.IsTrue(match.Success);
    }

    [TestMethod]
    public void CreateEducatorsDiv_ShouldProvideEducatorPicture()
    {

        string txt = HMTLAbstractMaker.CreateEducatorsDiv(_group);

        var match = Regex.Matches(txt, EDUCATOR_REGEX, options);

        Assert.AreEqual(@"..\..\pictures\profile-pic-default.jpg", match[0].Groups["picture"].Value);
        Assert.AreEqual(@"..\..\pictures\educators\profilePic_1472583693692_Du-Château-Céleste.jpg", match[1].Groups["picture"].Value);
    }

    [TestMethod]
    public void CreateEducatorsDiv_ShouldProvideEducatorFirstname()
    {

        string txt = HMTLAbstractMaker.CreateEducatorsDiv(_group);

        var match = Regex.Matches(txt, EDUCATOR_REGEX, options);

        Assert.AreEqual(@"Nicola", match[0].Groups["firstname"].Value);
        Assert.AreEqual(@"Céleste", match[1].Groups["firstname"].Value);
    }

    [TestMethod]
    public void CreateEducatorsDiv_ShouldProvideEducatorName()
    {

        string txt = HMTLAbstractMaker.CreateEducatorsDiv(_group);

        var match = Regex.Matches(txt, EDUCATOR_REGEX, options);

        Assert.AreEqual(@"Cacciatore", match[0].Groups["name"].Value);
        Assert.AreEqual(@"Du Château", match[1].Groups["name"].Value);
    }

    [TestMethod]
    public void CreateChildrenDivTest_Structure()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Match(txt, CHILDRENDIV_REGEX, options);

        Assert.IsTrue(match.Success);
    }

    [TestMethod]
    public void CreateChildrenDivTest_ShouldProvideCapacities()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Match(txt, CHILDRENDIV_REGEX, options);

        Assert.AreEqual(_group.CurrentCapacity.ToString(), match.Groups["currentcapacity"].Value);
        Assert.AreEqual(_group.FullCapacity.ToString(), match.Groups["fullcapacity"].Value);
    }

    [TestMethod]
    public void CreateChildrenDiv_ShouldProvideChildPicture()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Matches(txt, CHILD_REGEX, options);


        Assert.AreEqual(@"..\..\pictures\children\profilePic_1234567894561_Martin-Amy.jpg", match[0].Groups["picture"].Value);
        Assert.AreEqual(@"..\..\pictures\profile-pic-default.jpg", match[1].Groups["picture"].Value);
    }

    [TestMethod]
    public void CreateChildrenDiv_ShouldProvideChildFirstname()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Matches(txt, CHILD_REGEX, options);

        Assert.AreEqual(@"Amy", match[0].Groups["firstname"].Value);
        Assert.AreEqual(@"Théo", match[1].Groups["firstname"].Value);
    }

    [TestMethod]
    public void CreateChildrenDiv_ShouldProvideChildName()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Matches(txt, CHILD_REGEX, options);

        Assert.AreEqual(@"Martin", match[0].Groups["name"].Value);
        Assert.AreEqual(@"Boulanger", match[1].Groups["name"].Value);
    }

    [TestMethod]
    public void CreateChildrenDiv_ShouldProvideChildBirthDate()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Matches(txt, CHILD_REGEX, options);

        Assert.AreEqual("24/10/2023", match[0].Groups["birthdate"].Value);
        Assert.AreEqual("13/12/2023", match[1].Groups["birthdate"].Value);
    }

    [TestMethod]
    public void CreateChildrenDiv_ShouldProvideChildAge()
    {

        string txt = HMTLAbstractMaker.CreateChildrenDiv(_group);

        var match = Regex.Matches(txt, CHILD_REGEX, options);

        Assert.AreEqual("17", match[0].Groups["age"].Value);
        Assert.AreEqual("16", match[1].Groups["age"].Value);
    }




}