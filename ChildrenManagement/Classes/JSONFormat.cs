using System.Reflection;

namespace ChildrenManagement.Classes;

/// <summary>
/// Information of a Child instance formatted to be JSON Serializable
/// </summary>
/// <param name="ID"></param>
/// <param name="Name"></param>
/// <param name="Firstname"></param>
/// <param name="Nationality"></param>
/// <param name="BirthDate"></param>
/// <param name="PicturePath"></param>
/// <param name="GroupName">string</param>
/// <param name="TrustedPeopleIds"> long[] table with all Ids of the trustedPeople</param>
public record JSONChild(long ID,
                        string Name,
                        string Firstname,
                        Nationalities Nationality,
                        DateTime BirthDate,
                        string PicturePath,
                        string GroupName,
                        long[] TrustedPeopleIds);

/// <summary>
/// Information of a Group instance formatted to be JSON Serializable
/// </summary>
/// <param name="Name"></param>
/// <param name="FullCapacity"></param>
/// <param name="CurrentCapacity">Number of Children in the group</param>
/// <param name="ChildType"></param>
/// <param name="EducatorsID">long[] table with all Ids of the educators</param>
/// <param name="ChildrenID">long[] table with all Ids of the children</param>
public record JSONGroup(string Name,
                        int FullCapacity,
                        int CurrentCapacity,
                        ChildTypes ChildType,
                        long[] EducatorsID,
                        long[] ChildrenID);

/// <summary>
/// Information of a TrustedPerson instance formatted to be JSON Serializable
/// </summary>
/// <param name="ID"></param>
/// <param name="Name"></param>
/// <param name="Firstname"></param>
/// <param name="Nationality"></param>
/// <param name="RelationshipToChild"></param>
/// <param name="PhoneNumber"></param>
/// <param name="BirthDate"></param>
public record JSONTrustedPerson(long ID,
                                string Name,
                                string Firstname,
                                Nationalities Nationality,
                                RelationshipToChild RelationshipToChild,
                                string PhoneNumber,
                                DateTime BirthDate);

/// <summary>
/// Information of a Educator instance formatted to be JSON Serializable
/// </summary>
/// <param name="ID"></param>
/// <param name="Name"></param>
/// <param name="Firstname"></param>
/// <param name="Nationality"></param>
/// <param name="PreferenceType"></param>
/// <param name="PicturePath"></param>
public record JSONEducator(long ID,
                            string Name,
                            string Firstname,
                            Nationalities Nationality,
                            ChildTypes PreferenceType,
                            string PicturePath);


/// <summary>
/// contains all method to convert an object into a JSON Serializable object
/// </summary>
public static class TurnIntoJSONFormat
{
    public static JSONChild ConvertChild(Child child)
    {
        int nbTrustedPeople = child.ContactList.Count;
        long[] TrustedPeopleIds = new long[nbTrustedPeople];
        for (int i = 0; i < nbTrustedPeople; i++)
        {
            TrustedPeopleIds[i] = child.ContactList[i].Identity.Id;
        }


        return new(
            child.Identity.Id,
            child.Identity.Name,
            child.Identity.Firstname,
            child.Identity.Nationality,
            child.BirthDate,
            child.PicturePath,
            child.Group?.Name ?? "",
            TrustedPeopleIds
        );
    }

    public static JSONGroup ConvertGroup(Group group)
    {
        int nbChildren = group.Children.Count;
        long[] ChildrenIds = new long[nbChildren];
        for (int i = 0; i < nbChildren; i++)
        {
            ChildrenIds[i] = group.Children[i].Identity.Id;
        }

        int nbEducators = group.Educators.Count;
        long[] EducatorsIds = new long[nbEducators];
        for (int i = 0; i < nbEducators; i++)
        {
            EducatorsIds[i] = group.Educators[i].Identity.Id;
        }


        return new(
            group.Name,
            group.FullCapacity,
            group.CurrentCapacity,
            group.ChildType,
            EducatorsIds,
            ChildrenIds
        );
    }

    public static JSONEducator ConvertEducator(Educator educator)
    {
        return new(
            educator.Identity.Id,
            educator.Identity.Name,
            educator.Identity.Firstname,
            educator.Identity.Nationality,
            educator.PreferenceType,
            educator.PicturePath
        );
    }

    public static JSONTrustedPerson ConvertTrustedPerson(TrustedPerson trustedPerson)
    {
        return new(
            trustedPerson.Identity.Id,
            trustedPerson.Identity.Name,
            trustedPerson.Identity.Firstname,
            trustedPerson.Identity.Nationality,
            trustedPerson.RelationshipToChild,
            trustedPerson.PhoneNumber,
            trustedPerson.BirthDate
        );
    }



}