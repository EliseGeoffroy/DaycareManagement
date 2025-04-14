using ChildrenManagement.Classes;

namespace ChildrenManagement.staticClasses;

/// <summary>
/// Static class which build HTMLAbstract of a given Group
/// Method :
/// - CreateWholeHTML
/// - CreateHeader
/// - CreateEducatorsDiv
/// - CreateChildrenDiv
/// - CreatePicturePath
/// </summary>
public static class HMTLAbstractMaker
{
    private readonly static string[] _childtypesFrenchTranslation = ["Bébé", "Bambin", "Enfant"];
    private const string HTMLstart = """
    <!DOCTYPE html>
    <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <link rel="stylesheet" href="../style/group-style.css" />
            <title>Abstract</title>
        </head>
        <body>
    """;

    private const string HTMLend = """
        </body>
    </html>
    """;

    public static string CreateWholeHTML(Group group)
    {
        return HTMLstart + CreateHeader(group) + CreateEducatorsDiv(group) + CreateChildrenDiv(group) + HTMLend;
    }

    public static string CreateHeader(Group group)
    {
        return $"""
                <header class="flex-row-center">
                    <h1>{group.Name}</h1>
                    <span>({_childtypesFrenchTranslation[(int)group.ChildType]})</span>
                </header>
        """;
    }

    public static string CreateEducatorsDiv(Group group)
    {
        string HTMLEducatorsDiv = """
                <div class="educators flex-row-center">
                    <h2>Educateurs/Educatrices</h2>
        
        """;

        foreach (Educator educator in group.Educators)
        {
            HTMLEducatorsDiv += $"""
                        <div class="educator flex-column-center">
                            <div class="img">
                                <img
                                    src="{CreatePicturePath(educator)}"
                                    alt="educator picture"
                                />
                            </div>
                            <p class="firstname">{educator.Identity.Firstname}</p>
                            <p class="name">{educator.Identity.Name}</p>
                        </div>
            
            """;
        }
        HTMLEducatorsDiv += """
                </div>
        """;

        return HTMLEducatorsDiv;

    }

    public static string CreateChildrenDiv(Group group)
    {
        string HMTLChildrenDiv = $"""
                <div class="children flex-column-baseline">
                    <div class="descr flex-row-center">
                        <h2>Enfants</h2>
                        <span>{group.CurrentCapacity}/{group.FullCapacity}</span>
                    </div>
        """;
        foreach (Child child in group.Children)
        {
            HMTLChildrenDiv += $"""
                        <div class="child flex-row-center">
                            <div class="img">
                                <img
                                    src="{CreatePicturePath(child)}"
                                    alt="educator profile"
                                />
                            </div>
                            <div class="info flex-column-center">
                                <p class="firstname">{child.Identity.Firstname}</p>
                                <p class="name">{child.Identity.Name}</p>
                                <p class="birthDate">{child.BirthDate:d}</p>
                                <p class="age">{child.AgeInMonth} mois</p>
                            </div>
                        </div>
            """;
        }

        HMTLChildrenDiv += """
                </div>
        """;

        return HMTLChildrenDiv;
    }
    /// <summary>
    /// Create the relative path between resulted HTML and various pictures. The directory where the pictures are stocked depends on the type of the object whom belongs the picture.
    /// </summary>
    /// <param name="person">child or educator</param>
    /// <returns> string relative path</returns>
    public static string CreatePicturePath(PersonPicturable person)
    {
        string directory = (person is Child) ? DAL.CHILDREN_PIC_DIRECTORY_PATH : DAL.EDUCATORS_PIC_DIRECTORY_PATH;
        return (person.PicturePath != "") ? Path.Combine("..", "..", directory, person.PicturePath) : Path.Combine("..", "..", DAL.PIC_DIRECTORY_PATH, "profile-pic-default.jpg");
    }
}