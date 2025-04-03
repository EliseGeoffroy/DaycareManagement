using System.ComponentModel.DataAnnotations;
using General;

namespace ValidationBase;


public static class ValidationRules
{

    public static ValidationResult? ValidateBirthDate(DateTime date)
    {
        if (date > DateTime.Today)
        {
            return new ValidationResult("Nous n'acceptons pas l'inscription des enfants avant leur naissance.");
        }
        else
        {
            if (Utilities.CalculateAgeInMonth(date) > 4 * 12)
            {
                return new ValidationResult("Votre enfant est trop âgé pour être gardé en crèche. Veuillez vous rapprocher du foyer de jour.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

}