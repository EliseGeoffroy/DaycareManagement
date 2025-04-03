using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ChildrenManagementClasses;

/// <summary>
/// class abstract
/// Mother of Child, TrustedPerson and Educator
/// </summary>
/// <param name="identity"> Identity class (look below)</param>
public abstract class Person(Identity identity)
{

    public Identity Identity { get; set; } = identity;

    public void ValidateProperty(object value, [CallerMemberName] string? propertyName = null)
    {
        ValidationContext context = new(this);
        context.MemberName = propertyName;
        Validator.ValidateProperty(value, context);
    }
}

/// <summary>
/// Formal Identity of a Person with required properties :
/// 
/// -Id (long with 13 digits)
/// -Name (string between 2 and 70 char)
/// -Firstname (string between 2 and 70 char)
/// -Nationality (enum Nationalities (see below))
/// 
/// 2 constructors (one empty, one with all the above properties)
/// method ValidateProperty which permits validation with attributes in a Form
/// </summary>
public class Identity
{


    private long _id;
    [Display(Prompt = "Matricule luxembourgeois (s'il y en a un')")]
    [RegularExpression(@"\d{13}", ErrorMessage = "Un matricule luxembourgeois doit comporter 13 chiffres.")]
    [Required(ErrorMessage = "Ce champ est obligatoire")]
    public long Id
    {
        get => _id;
        set
        {
            ValidateProperty(value);
            _id = value;
        }
    }

    private string _name = string.Empty;
    [Display(Prompt = "Nom")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "Le nom doit comporter de 2 à 70 caractères")]
    [Required(ErrorMessage = "Ce champ est obligatoire")]
    public string Name
    {
        get => _name;
        set
        {
            ValidateProperty(value);
            _name = value;
        }
    }

    private string _firstname = string.Empty;
    [Display(Prompt = "Prénom")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "Le prénom doit comporter de 2 à 70 caractères")]
    [Required(ErrorMessage = "Ce champ est obligatoire")]
    public string Firstname
    {
        get => _firstname;
        set
        {
            ValidateProperty(value);
            _firstname = value;
        }
    }

    private Nationalities _nationality;
    [Display(Prompt = "Nationalité : 0-Français, 1-Luxembourgeois, 2-Allemand, 3-Belge, 4-Portugais, 5-Italien, 6-Autres(UE), 7-Autres(Hors UE)")]
    [Required(ErrorMessage = "Ce champ est obligatoire")]
    [EnumDataType(typeof(Nationalities), ErrorMessage = "Votre réponse ne correspond à aucune Nationalité connue")]
    public Nationalities Nationality
    {
        get => _nationality;
        set
        {
            ValidateProperty(value);
            _nationality = value;
        }
    }

    public Identity()
    {

    }
    public Identity(long id, string name, string firstname, Nationalities nationality)
    {
        Id = id;
        Name = name;
        Firstname = firstname;
        Nationality = nationality;
    }

    public void ValidateProperty(object value, [CallerMemberName] string? propertyName = null)
    {
        ValidationContext context = new(this);
        context.MemberName = propertyName;
        Validator.ValidateProperty(value, context);
    }

};

public enum Nationalities
{
    French, Luxembourgish, German, Belgian, Portuguese, Italian, OtherEuropean, NotEuropean
}

