using System.Net;
using ChildrenManagement.Classes;
using ChildrenManagement.staticClasses;
using Moq;

namespace ChildrenManagementTest;


[TestClass]
public class HTMLAbstractMenuTest
{
    private readonly Group _group = new("Les crevettes", 10, ChildTypes.Baby);
    private readonly Group _group1 = new("Les Cascadeurs", 1, ChildTypes.Toddler, new Educator(new Identity(1472583693696, "Cacciatore", "Nicola", Nationalities.Italian), ChildTypes.Toddler));
    private readonly Group _group2 = new("Les Pourquoi ?", 10, ChildTypes.Kid, new Educator(new Identity(1472583693694, "VanRichter", "Anna", Nationalities.Luxembourgish), ChildTypes.Kid));

    private readonly string _displayGroup = "De quel groupe voulez-vous avoir les informations :\n"
    + "- 00 : Les crevettes\n"
    + "- 01 : Les Cascadeurs\n"
    + "- 02 : Les Pourquoi ?\n";


    [TestInitialize]
    public void TestInitialize()
    {
        Datas.GroupDictionary.Clear();
    }

    [TestMethod]
    public void DisplayGroupsList()
    {
        Datas.GroupDictionary.Add(_group.Name, _group);
        Datas.GroupDictionary.Add(_group1.Name, _group1);
        Datas.GroupDictionary.Add(_group2.Name, _group2);
        List<string> groupList = Datas.GroupDictionary.Select(a => a.Value.Name).ToList();

        string output = HMTLAbstractMenu.DisplayGroupsList(groupList);

        Assert.AreEqual(_displayGroup, output);
    }

    [DataTestMethod]
    [DataRow("01", 1, DisplayName = "entryOK-2digit")]
    [DataRow("1", 1, DisplayName = "entryOK-1digit")]
    public void ValidateEntryGroup_IfNumChosenExists_EvenOn2Digits_ShouldbeOK(string entry, int output)
    {
        Datas.GroupDictionary.Add(_group.Name, _group);
        Datas.GroupDictionary.Add(_group1.Name, _group1);
        Datas.GroupDictionary.Add(_group2.Name, _group2);

        var stringReader = new StringReader(entry);
        Console.SetIn(stringReader);


        (bool entryOK, int validatedEntry) = HMTLAbstractMenu.ValidateEntryGroup(Datas.GroupDictionary.Count);

        Assert.IsTrue(entryOK);
        Assert.AreEqual(output, validatedEntry);

    }

    [DataTestMethod]
    [DataRow("", DisplayName = "no entry")]
    [DataRow("05", DisplayName = "invalid entry")]
    public void ValidateEntryGroup_IfNumChosenExists_EvenOn2Digits_ShouldbeOK(string entry)
    {
        Datas.GroupDictionary.Add(_group.Name, _group);
        Datas.GroupDictionary.Add(_group1.Name, _group1);
        Datas.GroupDictionary.Add(_group2.Name, _group2);

        var stringReader = new StringReader(entry);
        Console.SetIn(stringReader);


        var exception = Assert.ThrowsException<ArgumentException>(() => HMTLAbstractMenu.ValidateEntryGroup(Datas.GroupDictionary.Count));

        Assert.AreEqual("Aucun groupe ne correspond à votre réponse. Veuillez recommencer.", exception.Message);

    }

    [TestMethod]
    public void ChooseTheGroup_IfNumChosenIsValid_ShouldReturnGroupName()
    {

        Datas.GroupDictionary.Add(_group.Name, _group);
        Datas.GroupDictionary.Add(_group1.Name, _group1);
        Datas.GroupDictionary.Add(_group2.Name, _group2);

        var stringReader = new StringReader("01");
        Console.SetIn(stringReader);

        string chosenGroupName = HMTLAbstractMenu.ChooseTheGroup();

        Assert.AreEqual("Les Cascadeurs", chosenGroupName);

    }



}