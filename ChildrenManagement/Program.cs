// See https://aka.ms/new-console-template for more information
using ChildrenManagementClasses;
using General;

Identity childIdentity = new();
Utilities.InputData<Identity>(childIdentity);

Child child = new(childIdentity);
Utilities.InputData<Child>(child);

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
                child.AddATrustedPerson(trustedPerson);
            }
            catch (InvalidOperationException ioe)
            {
                System.Console.WriteLine(ioe.Message);
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



System.Console.WriteLine(child);


