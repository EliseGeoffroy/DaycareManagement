using System.ComponentModel.DataAnnotations;
using ValidationBase;
using System.Runtime.CompilerServices;
using General;
using System.Text.Json.Serialization;

namespace ChildrenManagementClasses;

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




