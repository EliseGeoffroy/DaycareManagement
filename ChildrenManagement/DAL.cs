namespace staticClasses;

public static class DAL
{

    public static void HandleFileInformation(string filepath)
    {
        using StreamReader reader = new(filepath);

        string? line;

        while ((line = reader.ReadLine()) != null)
        {

        }
    }
    public static void HandleChildInformation(string filepath)
    {

    }
}