using staticClasses;

namespace ChildrenManagementTest;

public class UtilitiesTest
{

    public int arg = 0;

    [TestInitialize]
    public void TestInitialize()
    {
        arg = 0;
    }

    #region HandleUserAnswer
    [DataTestMethod]
    [DataRow("o", 1, true)]
    [DataRow("o", 1, true)]
    [DataRow("N", 0, false)]
    [DataRow("N", 0, false)]
    public void HandleUserAnswer(string userAnswer, int argValue, bool expectedResult)
    {

        bool res = Utilities.HandleUserAnswerToContinueRegistering(userAnswer, () => ActionTest());

        Assert.AreEqual(argValue, arg);
        Assert.AreEqual(expectedResult, res);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("X")]
    public void AnswerNotHandling_ShouldThrowAnException(string userAnswer)
    {
        var exception = Assert.ThrowsException<ArgumentException>(() => Utilities.HandleUserAnswerToContinueRegistering(userAnswer, () => ActionTest()));
        Assert.AreEqual("La saisie ne correspond ni à Oui (O/o), ni à Non (N/n). Veuillez entrer de nouveau votre réponse.", exception.Message);
    }

    // ActionTest is a simple method to verify if the parameter Action is actually executed.
    public void ActionTest()
    {
        arg = 1;
    }

    #endregion

}