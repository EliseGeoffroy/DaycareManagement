
using ChildrenManagement.Classes;


namespace ChildrenManagement.staticClasses;

/// <summary>
/// Class corresponding to Registrationpage
/// </summary>
public static class Registration
{

    /// <summary>
    /// Registers as many children as user desires
    /// </summary>
    public static async void Register()
    {
        Group.JustOnePlaceAvailableEvent += (sender, childType) => System.Console.WriteLine($"Attention, il ne reste plus de place pour cette catégorie d'âge : {childType}");

        bool registerAChild = true;
        while (registerAChild)
        {
            System.Console.WriteLine("Voulez inscrire un nouvel enfant ? O/N");
            try
            {
                registerAChild = Utilities.HandleUserAnswerToContinueRegistering(Console.ReadLine() ?? "", () => RegisterAChild());
            }
            catch (ArgumentException ae)
            {
                System.Console.WriteLine(ae.Message);
            }
        }
        await Navigation.ReturnHomePage();
    }
    /// <summary>
    /// Registers a child in 3 steps :
    /// - Retrieves information
    /// - Adds eventually TrustedPeople to the child
    /// - Finds a group for the child
    /// </summary>
    public static async void RegisterAChild()
    {
        Child child = await RegisterChildInformationsAsync();
        AskToAddTrustedPeople(child);
        FindGroup(child);
    }

    /// <summary>
    /// Retrieves child information by a form, create object and add it to dictionary
    /// </summary>
    /// <returns> created Child</returns>
    public static async Task<Child> RegisterChildInformationsAsync()
    {
        Identity childIdentity = new();
        Utilities.InputData<Identity>(childIdentity);

        Child child = new(childIdentity);
        Utilities.InputData<Child>(child);

        try
        {
            Datas.AddAnEntryPersonToDictionary(Datas.ChildrenDictionary, child);
        }

        //Id already used by an another child
        catch (ArgumentException ae)
        {
            System.Console.WriteLine(ae.Message);
            await Navigation.ReturnHomePage();
        }

        return child;
    }

    /// <summary>
    /// Adds as many trustedpeople as user wants
    /// </summary>
    /// <param name="child"></param>
    public static void AskToAddTrustedPeople(Child child)
    {
        bool addATrustedPerson = true;

        while (addATrustedPerson)
        {
            System.Console.WriteLine("Voulez-vous ajouter des personnes à contacter en cas d'urgence et habilitées à venir chercher l'enfant ? O/N");
            try
            {
                addATrustedPerson = Utilities.HandleUserAnswerToContinueRegistering(Console.ReadLine() ?? "", () => AddATrustedPerson(child));
            }
            catch (ArgumentException ae)
            {
                System.Console.WriteLine(ae.Message);
            }
        }
    }

    /// <summary>
    /// Retrieve trustedPerson information and add it to child
    /// </summary>
    /// <param name="child"></param>
    public static async void AddATrustedPerson(Child child)
    {
        TrustedPerson trustedPerson = await RegisterTrustedPeopleInformationAsync();
        try
        {
            child.AddATrustedPerson(trustedPerson);
        }

        catch (InvalidOperationException ioe)
        {
            System.Console.WriteLine(ioe.Message);
        }

    }

    /// <summary>
    /// Retrieve TrustedPeople information, create the object and add it to dictionary
    /// </summary>
    /// <returns></returns>
    public static async Task<TrustedPerson> RegisterTrustedPeopleInformationAsync()
    {
        Identity adultIdentity = new();
        Utilities.InputData<Identity>(adultIdentity);

        TrustedPerson trustedPerson = new(adultIdentity);
        Utilities.InputData(trustedPerson);

        try
        {
            Datas.AddAnEntryPersonToDictionary(Datas.TrustedPeopleDictionary, trustedPerson);
        }
        catch (ArgumentException ae)
        {
            System.Console.WriteLine(ae.Message);
            await Navigation.ReturnHomePage();
        }

        return trustedPerson;
    }

    /// <summary>
    /// Search a group for the child, if no left places, exit the application (to be changed...)
    /// </summary>
    /// <param name="child"></param>
    public static void FindGroup(Child child)
    {
        try
        {
            Group.FindAGroup(child);
        }
        catch (InvalidOperationException ioe)
        {
            System.Console.WriteLine(ioe.Message);
            _ = Navigation.ReturnHomePage();
        }
    }

}