namespace ChildrenManagementTest;

[TestClass]
public sealed class ChildTest
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
