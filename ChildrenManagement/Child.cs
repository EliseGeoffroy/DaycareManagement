using System.ComponentModel.DataAnnotations;
using ValidationBase;
using General;


namespace ChildrenManagementClasses;

/// <summary>
/// represents a Child who is also a Person 
/// 
///properties :
///Identity Identity 
///DateTime BirthDate (Required, between Today-4years et Today)
///string PicturePath :  URL to child's picture
///int AgeInMonth (in month) : calculated from the birthdate
///List<TrustedPerson> ContactList : list with trustedAdults enough to come picking the child
///
/// 2 constructors : with Identity, BirthDate and pictuyre path or with only Identity
/// 1 ToString()
/// </summary>
public class Child : Person
{
    private DateTime _birthDate;

    [Required(ErrorMessage = "Merci de bien vouloir renseigner la date de naissance de votre enfant")]
    [DataType(DataType.DateTime, ErrorMessage = "Le format de la date n'est pas valide.")]
    [CustomValidation(typeof(ValidationRules), nameof(ValidationRules.ValidateBirthDate))]
    [Display(Prompt = "Date de Naissance (AAAA/MM/JJ)")]
    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            ValidateProperty(value);
            _birthDate = value;
        }
    }

    private string? _picturePath;

    [DataType(DataType.ImageUrl, ErrorMessage = "L'url n'est pas valide.")]
    [Display(Prompt = "Lien vers la photo de l'enfant")]
    public string? PicturePath
    {
        get => _picturePath;
        set
        {
            if (value != null)
            {
                ValidateProperty(value);
                _picturePath = value;
            }

        }
    }

    public int AgeInMonth => Utilities.CalculateAgeInMonth(BirthDate);
    public List<TrustedPerson>? ContactList { get; set; }

    public Child(Identity identity, DateTime birthDate, string? picturePath = null) : base(identity)
    {
        BirthDate = birthDate;
        PicturePath = picturePath;
    }

    public Child(Identity identity) : base(identity) { }


    public override string ToString()
    {
        return $"Id:{Identity.Id},Nom:{Identity.Name}, Prénom: {Identity.Firstname}, Nationalité:{Identity.Nationality}, Date de naissance:{BirthDate}, Age:{AgeInMonth} mois";
    }



}




