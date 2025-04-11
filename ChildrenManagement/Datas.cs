using ChildrenManagement.Classes;

namespace ChildrenManagement.staticClasses;

/// <summary>
/// Contains DictionaryForAllObjects (waiting a DB) :
/// -Educator (key= Id)
/// -TrustedPerson(key= Id)
/// -Child(key= Id)
/// -Group(key= name)
/// 
/// Methods :
/// - InitializeAllDatas() which calls :
///     -InitializeEducators()
///     -InitializeTrustedPeople()
///     -InitializeChildren()
///     -InitializeGroups()
/// Each method read information into a file, build objects and register them into a dictionary
/// </summary>
public static class Datas
{

    public static Dictionary<long, Educator> EducatorsDictionary { get; set; } = [];
    public static Dictionary<long, TrustedPerson> TrustedPeopleDictionary { get; set; } = [];
    public static Dictionary<long, Child> ChildrenDictionary { get; set; } = [];

    public static Dictionary<string, Group> GroupDictionary { get; set; } = [];

    public static void InitializeAllDatas()
    {
        InitializeEducators();
        InitializeTrustedPeople();
        InitializeChildren();
        InitializeGroups();
    }

    public static void InitializeEducators()
    {
        List<JSONEducator> jsonEducatorsList = DAL.HandleFileInformation<JSONEducator>(DAL._educatorFilePath);

        foreach (JSONEducator jsonEducator in jsonEducatorsList)
        {
            EducatorsDictionary.Add(jsonEducator.ID, new Educator(new Identity(jsonEducator.ID,
                                                                               jsonEducator.Name,
                                                                               jsonEducator.Firstname,
                                                                               jsonEducator.Nationality),
                                                                  jsonEducator.PreferenceType,
                                                                  jsonEducator.PicturePath));
        }

    }

    public static void InitializeTrustedPeople()
    {
        List<JSONTrustedPerson> jsonTrustedPeopleList = DAL.HandleFileInformation<JSONTrustedPerson>(DAL._trustedPeopleFilePath);

        foreach (JSONTrustedPerson jsonTrustedPerson in jsonTrustedPeopleList)
        {
            TrustedPeopleDictionary.Add(jsonTrustedPerson.ID, new TrustedPerson(new Identity(jsonTrustedPerson.ID,
                                                                                            jsonTrustedPerson.Name,
                                                                                            jsonTrustedPerson.Firstname,
                                                                                            jsonTrustedPerson.Nationality),
                                                                                jsonTrustedPerson.RelationshipToChild,
                                                                                jsonTrustedPerson.PhoneNumber,
                                                                                jsonTrustedPerson.BirthDate));
        }
    }

    public static void InitializeChildren()
    {
        List<JSONChild> jsonChildrenList = DAL.HandleFileInformation<JSONChild>(DAL._childrenFilePath);

        foreach (JSONChild jsonChild in jsonChildrenList)
        {
            Child child = new(new Identity(jsonChild.ID, jsonChild.Name, jsonChild.Firstname, jsonChild.Nationality), jsonChild.BirthDate, jsonChild.PicturePath);

            foreach (long id in jsonChild.TrustedPeopleIds)
            {
                child.AddATrustedPerson(TrustedPeopleDictionary[id]);
            }

            ChildrenDictionary.Add(child.Identity.Id, child);
        }

    }


    public static void InitializeGroups()
    {

        List<JSONGroup> jsonGroupsList = DAL.HandleFileInformation<JSONGroup>(DAL._groupFilePath);

        foreach (JSONGroup jsonGroup in jsonGroupsList)
        {
            Group group = new(jsonGroup.Name, jsonGroup.FullCapacity, jsonGroup.ChildType);
            foreach (long id in jsonGroup.EducatorsID)
            {
                group.AddAnEducator(EducatorsDictionary[id]);
            }

            foreach (long id in jsonGroup.ChildrenID)
            {
                group.AddAChild(ChildrenDictionary[id]);
                ChildrenDictionary[id].Group = group;
            }
            GroupDictionary.Add(group.Name, group);
        }

    }

    public static void AddAnEntryPersonToDictionary<T>(Dictionary<long, T> dictionary, T entry) where T : Person
    {
        try
        {
            dictionary.Add(entry.Identity.Id, entry);
        }

        catch (ArgumentException)
        {
            if (!entry.Equals(dictionary[entry.Identity.Id]))
            {
                throw new ArgumentException("Une autre personne est enregistrée avec le même Id. Veuillez réitérez l'inscription en vérifiant le matricule.");
            }
            else
            {
                System.Console.WriteLine("La personne a déjà été enregistrée. Les informations précédentes sont conservées. Pour les modifier, merci d'aller sur l'onglet de modifications (Fonctionnalité à venir).");
            }
        }
    }

}