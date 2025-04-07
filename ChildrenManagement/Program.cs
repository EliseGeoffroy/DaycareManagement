// See https://aka.ms/new-console-template for more information
using ChildrenManagementClasses;
using General;

GroupList groupList = InitializeGroups();
GroupList.JustOnePlaceAvailableEvent += (sender, childType) => System.Console.WriteLine($"Attention, il ne reste plus de place pour cette catégorie d'âge : {childType}");

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

try
{
    groupList.FindAGroup(child);
}
catch (InvalidOperationException ioe)
{
    System.Console.WriteLine(ioe.Message);
}

System.Console.WriteLine(child);



static GroupList InitializeGroups()
{
    List<Educator> educatorList = [
        new Educator(new Identity(1472583693692,"DaSilva","Maria", Nationalities.Portuguese),ChildTypes.Baby),
    new Educator(new Identity(1472583693693,"Roussel","Aurélie", Nationalities.Belgian),ChildTypes.Toddler),
    new Educator(new Identity(1472583693694,"VanRichter","Anna", Nationalities.Luxembourgish),ChildTypes.Kid),
    new Educator(new Identity(1472583693695,"Schmidt","Klaus", Nationalities.German),ChildTypes.Baby),
    new Educator(new Identity(1472583693696,"Cacciatore","Nicola", Nationalities.Italian),ChildTypes.Toddler),
    new Educator(new Identity(1472583693692,"Dujardin","Laurent", Nationalities.French),ChildTypes.Kid)
];

    return new GroupList([
        new Group("Les cacahouètes",10,ChildTypes.Baby,educatorList[0],educatorList[3]),
        new Group("Les Nooooooon!",1,ChildTypes.Toddler,educatorList[1],educatorList[4]),
        new Group("Les Pourquoi ?",10,ChildTypes.Kid,educatorList[2],educatorList[5])
    ]);

}