using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChildrenManagement;

namespace ChildrenManagementClasses;

/// <summary>
/// Educator
/// </summary>
/// <param name="identity"> Identituy (Id, Name, Firstname, Nationality)</param>
/// <param name="preferenceType">ChildType</param>
/// <param name="picturePath"> string url to educator picture, can be null</param>
public class Educator(Identity identity, ChildTypes preferenceType, string? picturePath = null) : Person(identity)
{
    public string? PicturePath { get; set; } = picturePath;

    public ChildTypes PreferenceType { get; set; } = preferenceType;

    public override string ToString()
    {
        return $"{Identity.Firstname} {Identity.Name}";
    }
}


public enum ChildTypes { Baby, Toddler, Kid }