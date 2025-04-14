using ChildrenManagement.Classes;

namespace ChildrenManagement.staticClasses;

/// <summary>
/// static Class which permits 
/// _to ask to a user which group he needs to abstract, 
/// _to call static class HTMLAbstractMaker to create HTML abstract
/// _ to call static class DAL to register it into a file
/// _ to give a result back to the user (Message advertising whether everything went well or not )
/// </summary>
public static class HMTLAbstractMenu
{
    public static async void CreateHTMLAbstractAsync()
    {
        string groupName = ChooseTheGroup();
        Group group = Datas.GroupDictionary[groupName];
        string HTMLAbstract = HMTLAbstractMaker.CreateWholeHTML(group);
        try
        {
            string filename = DAL.RegisterHTMLIntoAFile(HTMLAbstract, group.Name);
            System.Console.WriteLine($"Le fichier d'extraction {filename} a été créé avec succès.");
        }
        catch (IOException ioe)
        {
            System.Console.WriteLine("Le fichier d'extraction n'a pas pu être créé.");
            System.Console.WriteLine($"Erreur : {ioe.Message}");
        }
        await Navigation.ReturnHomePage();

    }
    public static string ChooseTheGroup()
    {
        bool entryOK = false;

        List<string> groupList = Datas.GroupDictionary.Select(a => a.Value.Name).ToList();
        string output = DisplayGroupsList(groupList);
        int entry = 0;

        while (!entryOK)
        {

            System.Console.WriteLine(output);

            try
            {
                (entryOK, entry) = ValidateEntryGroup(groupList.Count);
            }
            catch (ArgumentException ae)
            {
                System.Console.WriteLine(ae.Message);
            }

        }

        return groupList[entry];

    }

    public static string DisplayGroupsList(List<string> groupList)
    {
        string output = "De quel groupe voulez-vous avoir les informations :\n";


        for (int i = 0; i < groupList.Count; i++)
        {
            output += $"- {i:d2} : {groupList[i]}\n";
        }

        return output;
    }

    public static (bool, int) ValidateEntryGroup(int length)
    {
        if (int.TryParse(Console.ReadLine(), out int entry) && (entry >= 0) && (entry < length))
        {
            return (true, entry);
        }
        else
        {
            throw new ArgumentException("Aucun groupe ne correspond à votre réponse. Veuillez recommencer.");

        }
    }
}