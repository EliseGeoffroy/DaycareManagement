using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace ChildrenManagement.staticClasses;

public static class Navigation
{
    public const string MENU = """
                            Menu :
                            0 - Accueil
                            1 - Inscription d'un enfant
                            2 - Impression d'une fiche groupe
                            """;

    public static async Task ReturnHomePage()
    {
        await DAL.DownloadAllPictures();
        DAL.RegisterAllInFilesAtTheEnd();

        DisplayHomePage();
    }

    public static void DisplayHomePage()
    {
        bool entryOK = false;
        string entry = string.Empty;
        while (!entryOK)
        {
            System.Console.WriteLine(MENU);

            (entryOK, entry) = ValidateMenuChoice();

            if (!entryOK) System.Console.WriteLine("Aucun menu ne correspond à votre réponse. Veuillez réitérer votre saisie.");
        }


        switch (entry)
        {
            case "0":
                break;
            case "1":
                Registration.Register();
                break;
            case "2":
                HMTLAbstractMenu.CreateHTMLAbstractAsync();
                break;
        }


    }

    public static (bool, string) ValidateMenuChoice()
    {
        string entry;
        bool entryOK = Regex.IsMatch(entry = Console.ReadLine() ?? "", @"0|1|2");

        return (entryOK, entry);

    }


}