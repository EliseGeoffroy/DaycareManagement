using System.ComponentModel.DataAnnotations;
using ChildrenManagement.staticClasses;

namespace ValidationBase;


public static class ValidationRules
{

    public static ValidationResult? ValidateChildBirthDate(DateTime date)
    {
        if (date > DateTime.Today)
        {
            return new ValidationResult("Nous n'acceptons pas l'inscription des enfants avant leur naissance.");
        }
        else
        {
            if (Utilities.CalculateAgeInMonth(date, DateTime.Today) > 4 * 12)
            {
                return new ValidationResult("Votre enfant est trop âgé pour être gardé en crèche. Veuillez vous rapprocher du foyer de jour.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public static ValidationResult? ValidateTrustedPersonBirthDate(DateTime date)
    {


        if (Utilities.CalculateAgeInMonth(date, DateTime.Today) < 18 * 12)
        {
            return new ValidationResult("Seul un adulte peut venir récupérer l'enfant.");
        }
        else
        {
            return ValidationResult.Success;
        }

    }

}