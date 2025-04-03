using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace General;

public static class Utilities
{
    public static int CalculateAgeInMonth(DateTime birthDate)
    {
        int age = 0;
        if (birthDate.Year == DateTime.Today.Year)
        {
            age = DateTime.Today.Month - birthDate.Month;

        }
        else
        {
            //Number of the months before next year
            age = 12 - birthDate.Month + 1;
            //Number of years between this next year and the beginning of the current one
            age += 12 * (DateTime.Today.Year - (birthDate.Year + 1));
            //Number of month since the beginning of this year;
            age += DateTime.Today.Month - 1;
        }
        if (birthDate.Day > DateTime.Today.Day)
        {
            age--;
        }
        return age;
    }

    public static void HandleValidationError(Action saisie)
    {
        bool erreur;
        do
        {
            erreur = false;
            try
            {
                saisie();
            }
            catch (TargetInvocationException tie)
            {
                System.Console.WriteLine(tie.InnerException?.Message);
                erreur = true;
            }
            catch (InvalidCastException)
            {
                System.Console.WriteLine("Format invalide.");
                erreur = true;
            }
            catch (FormatException)
            {
                System.Console.WriteLine("Format invalide.");
                erreur = true;
            }
        } while (erreur);
    }

    public static void InputData<T>(T obj) where T : class
    {
        PropertyInfo[] propertyList = obj.GetType().GetProperties();
        if (propertyList.Length != 0)
        {
            foreach (PropertyInfo property in propertyList)
            {

                if (property.GetCustomAttribute<DisplayAttribute>() != null)
                {
                    HandleValidationError(() =>
                        {
                            System.Console.WriteLine(property.GetCustomAttribute<DisplayAttribute>()?.Prompt);
                            Type type = property.PropertyType;
                            if (type.IsEnum)
                            {
                                type = typeof(int);
                            }
                            var newValue = Convert.ChangeType(Console.ReadLine(), type) ?? "";
                            property.SetValue(obj, newValue);
                        });
                }

            }


        }
    }

    public static void InputDataTest<T>(T obj, string?[] values) where T : class
    {
        List<PropertyInfo> propertyList = obj.GetType().GetProperties().ToList<PropertyInfo>();
        var propertiesRequiredList = propertyList.Where(prop => prop.GetCustomAttribute<DisplayAttribute>() != null);
        if (propertiesRequiredList.Any())
        {
            if (propertiesRequiredList.Count() != values.Length)
            {
                string message = "Attention : le jeu de test est erroné : ";
                int diff;
                message += ((diff = propertiesRequiredList.Count() - values.Length) > 0) ? $"il manque {diff} valeurs." : $"il y a {diff} valeurs en trop";
                throw new ArgumentException(message);
            }
            else
            {

                int indexValue = 0;
                foreach (PropertyInfo property in propertiesRequiredList)
                {

                    System.Console.WriteLine(property.GetCustomAttribute<DisplayAttribute>()?.Prompt);
                    Type type = property.PropertyType;
                    if (type.IsEnum)
                    {
                        type = typeof(int);
                    }

                    var newValue = Convert.ChangeType(values[indexValue], type) ?? "";
                    property.SetValue(obj, newValue);

                    indexValue++;
                }
            }
        }

    }


}