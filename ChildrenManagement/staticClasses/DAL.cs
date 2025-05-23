using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ChildrenManagement.Classes;
using SixLabors.ImageSharp;

namespace ChildrenManagement.staticClasses;

public static class DAL
{

    public const string EDUCATOR_FILEPATH = @"files\educators.json";
    public const string GROUP_FILEPATH = @"files\groups.json";
    public const string TRUSTEDPEOPLE_FILEPATH = @"files\trustedPeople.json";
    public const string CHILDREN_FILEPATH = @"files\children.json";

    public const string CHILDREN_PIC_DIRECTORY_PATH = @"pictures\children";
    public const string EDUCATORS_PIC_DIRECTORY_PATH = @"pictures\educators";
    public const string PIC_DIRECTORY_PATH = @"pictures";

    private readonly static HttpClient client = new();


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
        WriteInformationIntoAFile(GROUP_FILEPATH, jsonGroupList);

    }

    public static void RegisterChildrenInAFile()
    {

        List<JSONChild> jsonChildrenList = [];
        foreach (KeyValuePair<long, Child> item in Datas.ChildrenDictionary)
        {
            jsonChildrenList.Add(TurnIntoJSONFormat.ConvertChild(item.Value));
        }
        WriteInformationIntoAFile(CHILDREN_FILEPATH, jsonChildrenList);

    }

    public static void RegisterEducatorsInAFile()
    {
        List<JSONEducator> jsonEducatorsList = [];
        foreach (KeyValuePair<long, Educator> item in Datas.EducatorsDictionary)
        {
            jsonEducatorsList.Add(TurnIntoJSONFormat.ConvertEducator(item.Value));
        }
        WriteInformationIntoAFile(EDUCATOR_FILEPATH, jsonEducatorsList);
    }
    public static void RegisterTrustedPeopleInAFile()
    {
        List<JSONTrustedPerson> jsonTrustedPeopleList = [];
        foreach (KeyValuePair<long, TrustedPerson> item in Datas.TrustedPeopleDictionary)
        {
            jsonTrustedPeopleList.Add(TurnIntoJSONFormat.ConvertTrustedPerson(item.Value));
        }
        WriteInformationIntoAFile(TRUSTEDPEOPLE_FILEPATH, jsonTrustedPeopleList);
    }

    public static async Task DownloadAllPictures()
    {
        List<Task<(string, PersonPicturable)>>? childrenTasksList = CreateTaskDownloading(Datas.ChildrenDictionary);
        List<Task<(string, PersonPicturable)>>? educatorsTasksList = CreateTaskDownloading(Datas.EducatorsDictionary);

        List<Task<(string, PersonPicturable)>> tasksList = [.. educatorsTasksList, .. childrenTasksList];


        while (tasksList.Any())
        {
            Task<(string, PersonPicturable)> finishedTask = await Task.WhenAny(tasksList);
            try
            {
                (string picturePath, PersonPicturable person) = finishedTask.Result;
                if (person is Educator educator)
                {
                    Datas.EducatorsDictionary[educator.Identity.Id].PicturePath = picturePath;
                }
                else if (person is Child child)
                {
                    Datas.ChildrenDictionary[child.Identity.Id].PicturePath = picturePath;
                }
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
            tasksList.Remove(finishedTask);
        }



    }

    /// <summary>
    /// Launch downloading of the pictures of either Child or Educator, according to the dictionary in param.
    /// Only not already downloaded pictures (whose PicturePath in object is not an url) are downloaded.
    /// </summary>
    /// <typeparam name="T"> PersonPicturable : either Child or Educator</typeparam>
    /// <param name="dictionary"> Dats Dictionary</param>
    /// <returns> TaskList</returns>
    public static List<Task<(string, PersonPicturable)>> CreateTaskDownloading<T>(Dictionary<long, T> dictionary) where T : PersonPicturable
    {

        List<Task<(string, PersonPicturable)>> tasksList = [];
        foreach (KeyValuePair<long, T> keyValuePair in dictionary)
        {
            PersonPicturable person = keyValuePair.Value;
            if ((person.PicturePath != "") && !System.Text.RegularExpressions.Regex.IsMatch(person.PicturePath, @"^profilePic_"))
            {

                var task = DownloadProfilePicture(person.PicturePath, person);
                tasksList.Add(task);

            }
        }
        return tasksList;
    }


    /// <summary>
    /// Download pictures from url and register it into a file in data/pictures/children|educators
    /// </summary>
    /// <param name="pictureURL"> current URL of the picture to download</param>
    /// <param name="person"> object whom picture is linked</param>
    /// <returns></returns>
    public static async Task<(string, PersonPicturable)> DownloadProfilePicture(string pictureURL, PersonPicturable person)
    {
        Stream streamPicture = await client.GetStreamAsync(pictureURL);
        Image picture = await Image.LoadAsync(streamPicture);
        string pictureName = $"profilePic_{person.Identity.Id}_{person.Identity.Name}-{person.Identity.Firstname}.jpg";
        string directoryPath;

        directoryPath = (person is Child) ? CHILDREN_PIC_DIRECTORY_PATH : EDUCATORS_PIC_DIRECTORY_PATH;


        string picturePath = Path.Combine(directoryPath, pictureName);

        using FileStream fs = File.Create(picturePath);
        await ImageExtensions.SaveAsJpegAsync(picture, fs);

        return (pictureName, person);
    }

    public static string RegisterHTMLIntoAFile(string HTML, string groupName)
    {
        using var writer = new StringWriter();
        writer.WriteLine(HTML);
        string filename = HTMLAbstractFileName(groupName);
        File.WriteAllText(Path.Combine("abstracts", "group", filename), writer.ToString());

        return filename;
    }

    public static string HTMLAbstractFileName(string groupName)
    {
        return $"Extraction_{groupName.Replace(" ", "-")}_{DateTime.Now:yyyyMMdd-HHmmss}.html";
    }

}
