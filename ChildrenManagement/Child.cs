using System.ComponentModel.DataAnnotations;
using staticClasses;
using ValidationBase;

namespace ChildrenManagementClasses;

public interface IChild
{
    public DateTime BirthDate { get; set; }

    public string? PicturePath { get; set; }


    public int AgeInMonth => Utilities.CalculateAgeInMonth(BirthDate, DateTime.Today);
    public List<TrustedPerson> ContactList { get; set; }

    /// <summary>
    /// Allows the addition of a Trusted Person (an adult who can pick up the child and whom we can contact) to the Contact List, with a limit of 5 Trusted People.
    /// </summary>
    /// <param name="trustedPerson">Adult who can pick up the child and whom we can contact</param>
    /// <exception cref="InvalidOperationException">If there are already 5 TrustedPeople</exception>
    void AddATrustedPerson(TrustedPerson trustedPerson);

}

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
/// Methods:
/// 2 constructors : with Identity, BirthDate and pictuyre path or with only Identity
/// 1 ToString()
/// AddATrustedPerson(TrustedPerson) : adds a TrustedPerson in the ContactList
/// </summary>
public class Child : Person, IChild
{

    #region Fields & Properties
    private DateTime _birthDate;

    [Required(ErrorMessage = "Merci de bien vouloir renseigner la date de naissance de votre enfant")]
    [DataType(DataType.DateTime, ErrorMessage = "Le format de la date n'est pas valide.")]
    [CustomValidation(typeof(ValidationRules), nameof(ValidationRules.ValidateChildBirthDate))]
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

    public int AgeInMonth => Utilities.CalculateAgeInMonth(BirthDate, DateTime.Today);
    public List<TrustedPerson> ContactList { get; set; } = [];

    public ChildTypes ChildType
    {
        get
        {
            if (AgeInMonth < 12)
            {
                return ChildTypes.Baby;
            }
            else if (AgeInMonth < 24)
            {
                return ChildTypes.Toddler;
            }
            else
            {
                return ChildTypes.Kid;
            }
        }
    }

    public Group? Group { get; set; }

    #endregion

    public Child(Identity identity, DateTime birthDate, string? picturePath = null) : base(identity)
    {
        BirthDate = birthDate;
        PicturePath = picturePath;
    }

    public Child(Identity identity) : base(identity) { }


    public override string ToString()
    {
        string description = $"""
        {Identity.Id,-13} ; {Identity.Name,-25} ;   {Identity.Firstname,-15} ; {Identity.Nationality,10} ; {BirthDate:d} ; {AgeInMonth:D2} mois
        Groupe : {Group?.Name ?? "Pas de groupe"},
        Educateurs/Educatrices :
        
        """;
        if (Group != null)
        {
            foreach (Educator educator in Group.Educators)
            {
                description += "- " + educator.ToString() + '\n';
            }
        }

        description += "Personnes de confiance : \n";

        foreach (TrustedPerson trustedPerson in ContactList)
        {
            description += " - " + trustedPerson.ToString() + '\n';
        }

        return description;
    }

    /// <summary>
    /// Allows the addition of a Trusted Person (an adult who can pick up the child and whom we can contact) to the Contact List, with a limit of 5 Trusted People.
    /// </summary>
    /// <param name="trustedPerson">Adult who can pick up the child and whom we can contact</param>
    /// <exception cref="InvalidOperationException">If there are already 5 TrustedPeople</exception>
    public void AddATrustedPerson(TrustedPerson trustedPerson)
    {
        if (ContactList.Count < 5)
            ContactList.Add(trustedPerson);
        else
            throw new InvalidOperationException("Vous ne pouvez pas renseigner plus de 5 contacts d'urgence pour votre enfant");

    }



}




