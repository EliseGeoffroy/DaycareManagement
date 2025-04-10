// See https://aka.ms/new-console-template for more information
using System.Text.Encodings.Web;
using System.Text.Json;
using ChildrenManagement;
using ChildrenManagementClasses;
using General;
using staticClasses;

Directory.SetCurrentDirectory(@"C:\Users\geoff\Desktop\C#.Net\DaycareManagement\ChildrenManagement\data\files");

GroupList groupList = Datas.InitializeAllDatas();
GroupList.JustOnePlaceAvailableEvent += (sender, childType) => System.Console.WriteLine($"Attention, il ne reste plus de place pour cette catégorie d'âge : {childType}");



Identity childIdentity = new();
Utilities.InputData<Identity>(childIdentity);

Child child = new(childIdentity);
Utilities.InputData<Child>(child);

try
{
    Datas.ChildrenDictionary.Add(child.Identity.Id, child);

}

//Id already used by an another child
catch (ArgumentException)
{
    if (!child.Equals(Datas.ChildrenDictionary[child.Identity.Id]))
    {
        System.Console.WriteLine("Un autre enfant est enregistré avec le même Id. Veuillez réitérez l'inscription en vérifiant le matricule.");
    }
}


bool addATrustedPerson = true;

while (addATrustedPerson)
{
    System.Console.WriteLine("Voulez-vous ajouter des personnes à contacter en cas d'urgence et habilitées à venir chercher l'enfant ? O/N");

    switch (Console.ReadLine())
    {
        case "O":
        case "o":
            Identity adultIdentity = new();
            Utilities.InputData<Identity>(adultIdentity);

            TrustedPerson trustedPerson = new(adultIdentity);
            Utilities.InputData(trustedPerson);

            try
            {
                Datas.TrustedPeopleDictionary.Add(trustedPerson.Identity.Id, trustedPerson);
                child.AddATrustedPerson(trustedPerson);

            }
            catch (InvalidOperationException ioe)
            {
                System.Console.WriteLine(ioe.Message);
            }
            //Id already used by an another TrustedPerson
            catch (ArgumentException)
            {
                if (!trustedPerson.Equals(Datas.TrustedPeopleDictionary[trustedPerson.Identity.Id]))
                {
                    System.Console.WriteLine("Un autre adulte s'est enregistré avec le même Id. Veuillez réitérez l'inscription en vérifiant le matricule.");
                }
            }
            break;

        case "N":
        case "n":
            addATrustedPerson = false;
            break;

        default:
            System.Console.WriteLine("La saisie ne correspond ni à Oui (O/o), ni à Non (N/n). Veuillez entrer de nouveau votre réponse.");
            break;
    }
}

try
{
    groupList.FindAGroup(child);
}
catch (InvalidOperationException ioe)
{
    System.Console.WriteLine(ioe.Message);
    Environment.Exit(0);
}


DAL.RegisterAllInFilesAtTheEnd();


System.Console.WriteLine(child);





