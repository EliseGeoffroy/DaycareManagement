using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChildrenManagement.Classes;

/// <summary>
/// Educator
/// </summary>
/// <param name="identity"> Identituy (Id, Name, Firstname, Nationality)</param>
/// <param name="preferenceType">ChildType</param>
/// <param name="picturePath"> string url to educator picture, can be null</param>
public class Educator : PersonPicturable
{
    public ChildTypes PreferenceType { get; set; }

    public Educator(Identity identity, ChildTypes preferenceType) : base(identity)
    {
        PreferenceType = preferenceType;
    }

    public Educator(Identity identity, ChildTypes preferenceType, string picturePath) : base(identity, picturePath)
    {
        PreferenceType = preferenceType;
    }

    public override string ToString()
    {
        return $"{Identity.Firstname} {Identity.Name}";
    }
}


public enum ChildTypes { Baby, Toddler, Kid }