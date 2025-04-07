using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChildrenManagementClasses;

public class Educator(Identity identity, ChildTypes preferenceType, string? picturePath = null) : Person(identity)
{
    public string? PicturePath { get; set; } = picturePath;

    public ChildTypes PreferenceType { get; set; } = preferenceType;

    public override string ToString()
    {
        return $"{Identity.Firstname} {Identity.Name}";
    }
}

public class Group
{
    public string Name { get; init; } = string.Empty;
    private List<Child> _children = [];

    public ReadOnlyCollection<Child> Children => _children.AsReadOnly();

    public int FullCapacity { get; init; }

    public int CurrentCapacity { get; set; }

    private List<Educator> _educators = [];

    public ReadOnlyCollection<Educator> Educators => _educators.AsReadOnly();

    public ChildTypes ChildType { get; init; }

    public Group(string name, int capacity, ChildTypes childType, params Educator[] educatorsTable)
    {
        Name = name;
        ChildType = childType;
        FullCapacity = capacity;
        foreach (Educator educator in educatorsTable)
        {
            _educators.Add(educator);
        }
    }

    public void AddAChild(Child child)
    {
        if (CurrentCapacity < FullCapacity)
        {
            _children.Add(child);
            CurrentCapacity++;
        }
        else
        {
            throw new InvalidOperationException("On ne peut plus ajouter d'enfants dans ce groupe.");
        }
    }

    public void AddSeveralChildren(List<Child> children)
    {
        int nbChildren = children.Count;
        try
        {
            foreach (Child child in children)
            {

                AddAChild(child);
                nbChildren--;

            }
        }
        catch (InvalidOperationException ioe)
        {
            System.Console.WriteLine(ioe.Message + '\n' + $"{nbChildren} n'ont pas été enregistrés dans ce groupe.");
        }
    }



}

public enum ChildTypes { Baby = 12, Toddler = 24, Kid = 48 }