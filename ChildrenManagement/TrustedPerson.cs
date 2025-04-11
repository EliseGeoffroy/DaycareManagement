using System.ComponentModel.DataAnnotations;
using ValidationBase;

namespace ChildrenManagementClasses;

/// <summary>
///Adult who can pick up the child and whom we can contact
///Instances used combinated with Child herited classes
///
///Properties :
///-RelationshipToChild RelationshipToChild : required enum value
///-string PhoneNumber : required, with country code, only French, German, Belgian and Luxembourgish formates are allowed
///-DateTime BirthDate : only allows age checking => just an adult (>18 yo) can be a Trusted Person.
///
/// Methods:
/// -2 constructors
/// - Override of ToString()
/// 
/// </summary>
public class TrustedPerson : Person
{
    private RelationshipToChild _relationshipToChild;
    [Display(Prompt = "Lien avec l'enfant : 0-Mère, 1-Père, 2-Frère, 3-Soeur, 4-Grand-Parent, 5-Tante, 6-Oncle, 7-Cousin(e), 8-Parrain/Marraine, 9-Autre")]
    [EnumDataType(typeof(RelationshipToChild), ErrorMessage = "Le numéro rentré ne correspond à aucune proposition")]
    [Required(ErrorMessage = "Ce champ est obligatoire.")]
    public RelationshipToChild RelationshipToChild
    {
        get => _relationshipToChild;
        set
        {
            ValidateProperty(value);
            _relationshipToChild = value;
        }
    }

    private string _phoneNumber = string.Empty;
    [RegularExpression(@"\+33\d{9}|\+352\d{8}|\+32\d{8}|\+49\d{10,14}", ErrorMessage = "Ce numéro de téléphone n'est pas valide. Seuls les numéros luxembourgeois, français, allemands et belges sont acceptés")]
    [Required(ErrorMessage = "Ce champ est obligatoire")]
    [Display(Prompt = "Numéro de téléphone (ne pas oublier d'indiquer l'identifiant avec +XX)")]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (value != null)
            {
                ValidateProperty(value);
                _phoneNumber = value;
            }

        }
    }

    private DateTime _birthDate;

    [Required(ErrorMessage = "Ce champ est obligatoire")]
    [DataType(DataType.DateTime, ErrorMessage = "Le format de la date n'est pas valide.")]
    [CustomValidation(typeof(ValidationRules), nameof(ValidationRules.ValidateTrustedPersonBirthDate))]
    [Display(Prompt = "Date de Naissance (JJ/MM/AAAA)")]
    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            ValidateProperty(value);
            _birthDate = value;
        }
    }

    public TrustedPerson(Identity identity, RelationshipToChild relationshipToChild, string phoneNumber, DateTime birthDate) : base(identity)
    {
        RelationshipToChild = relationshipToChild;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }
    public TrustedPerson(Identity identity) : base(identity)
    {
    }

    public override string ToString()
    {
        return $"{Identity.Id,-13} ; {Identity.Name,-25} ; {Identity.Firstname,-15} ;  {Identity.Nationality,10} ; {RelationshipToChild,12} ; {PhoneNumber,17}";
    }
}

public enum RelationshipToChild { Mother, Father, Brother, Sister, GrandParent, Aunt, Uncle, Cousin, GodParent, Other }