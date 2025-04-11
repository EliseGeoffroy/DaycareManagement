using System.Reflection.Metadata;
using System.Text.Encodings.Web;
using System.Text.Json;
using ChildrenManagement;
using ChildrenManagementClasses;

namespace staticClasses;

public static class DAL
{

    public const string _educatorFilePath = "educators.json";
    public const string _groupFilePath = "groups.json";
    public const string _trustedPeopleFilePath = "trustedPeople.json";
    public const string _childrenFilePath = "children.json";

    private static readonly JsonSerializerOptions _options = new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public static List<T> HandleFileInformation<T>(string filepath)
    {
        string txt = File.ReadAllText(filepath);
        List<T> deserializeText = [];

        if (txt != "")
        {
            deserializeText = JsonSerializer.Deserialize<List<T>>(txt) ?? [];
        }

        return deserializeText;
    }

    public static void WriteInformationIntoAFile<T>(string filepath, List<T> objectList)
    {
        string jsonString = JsonSerializer.Serialize(objectList, _options);
        File.WriteAllText(filepath, jsonString);
    }

    /// <summary>
    /// At the end of a session, all datas are registered into files to be reused next time.
    /// => each method turn each object registered in Datas' Dictionaries into a JSON serializable one and write it into a JSON File
    /// </summary>
    public static void RegisterAllInFilesAtTheEnd()
    {
        RegisterGroupsInAFile();
        RegisterChildrenInAFile();
        RegisterEducatorsInAFile();
        RegisterTrustedPeopleInAFile();
    }
    public static void RegisterGroupsInAFile()
    {

        List<JSONGroup> jsonGroupList = [];
        foreach (KeyValuePair<string, Group> item in Datas.GroupDictionary)
        {
            jsonGroupList.Add(TurnIntoJSONFormat.ConvertGroup(item.Value));
        }
        WriteInformationIntoAFile(_groupFilePath, jsonGroupList);

    }

    public static void RegisterChildrenInAFile()
    {

        List<JSONChild> jsonChildrenList = [];
        foreach (KeyValuePair<long, Child> item in Datas.ChildrenDictionary)
        {
            jsonChildrenList.Add(TurnIntoJSONFormat.ConvertChild(item.Value));
        }
        WriteInformationIntoAFile(_childrenFilePath, jsonChildrenList);

    }

    public static void RegisterEducatorsInAFile()
    {
        List<JSONEducator> jsonEducatorsList = [];
        foreach (KeyValuePair<long, Educator> item in Datas.EducatorsDictionary)
        {
            jsonEducatorsList.Add(TurnIntoJSONFormat.ConvertEducator(item.Value));
        }
        WriteInformationIntoAFile(_educatorFilePath, jsonEducatorsList);
    }
    public static void RegisterTrustedPeopleInAFile()
    {
        List<JSONTrustedPerson> jsonTrustedPeopleList = [];
        foreach (KeyValuePair<long, TrustedPerson> item in Datas.TrustedPeopleDictionary)
        {
            jsonTrustedPeopleList.Add(TurnIntoJSONFormat.ConvertTrustedPerson(item.Value));
        }
        WriteInformationIntoAFile(_trustedPeopleFilePath, jsonTrustedPeopleList);
    }

}
