using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChildrenManagementClasses;

/// <summary>
/// Group the different children's groups and reference the number of available spots for each age.
/// Send an event JustOnePlaceAvailableEvent if there's no more left spots for an age-range.
/// 
/// Properties :
/// - List<Group> Groups
/// - int [] table referencing the number of available spots for each age (index 0 : Baby, index 1: Toddler, index 2: Kid)
/// 
/// Constructors : 
/// - Group 1
/// - Group 2
/// -...
/// 
/// Methods :
/// -FindAGroup(Child child) : find an available and age adapted group for the child
///
/// 
/// </summary>
public class GroupList
{
    public static event EventHandler<ChildTypes>? JustOnePlaceAvailableEvent;
    public List<Group> Groups { get; set; } = [];

    private List<int> _nbPlaces = [0, 0, 0];


    public GroupList(params Group[] groupList)
    {
        foreach (Group group in groupList)
        {
            Groups.Add(group);

            _nbPlaces[(int)group.ChildType] = group.AvailableCapacity;
        }

    }

    /// <summary>
    /// find an available and age adapted group for the child
    ///invoke an event if no more spot for this age-range after the inscription
    /// </summary>
    /// <param name="child">Child child to put in a group</param>
    /// <exception cref="InvalidOperationException">throws an exception if no possibilities</exception>
    /// 
    public void FindAGroup(Child child)
    {
        if (_nbPlaces[(int)child.ChildType] == 0)
        {
            throw new InvalidOperationException("Aucune place n'est disponible pour cet âge. L'inscription est annulée.");
        }
        else
        {
            Group? group = Groups.Where(g => g.ChildType == child.ChildType && g.CurrentCapacity < g.FullCapacity).FirstOrDefault();
            if (group != null)
            {
                child.Group = group;
                group.AddAChild(child);
                _nbPlaces[(int)child.ChildType]--;
            }
            if (_nbPlaces[(int)child.ChildType] == 0)
            {
                JustOnePlaceAvailableEvent?.Invoke(this, child.ChildType);
            }
        }

    }
}

/// <summary>
/// Children grouped by age, supervised by several Educators
/// 
/// Properties :
/// - string Name 
/// - List<Child> Children (ReadOnlyCollection)
/// - int FullCapacity : nb max of children in this group
/// - int CurrentCapacity : current number of children
/// - int AvailableCapacity=FullCapacity-CurrentCapacity
/// - List<Educator> Educators (ReadOnlyCollection)
/// - ChildTypes ChildType : Age-range of Children accepted in this group
/// 
/// Constructors :
/// - name, fullcapacity, childtype,Educator1, Educator2,...
/// 
/// Methods :
/// - AddAChild : add a Child in Children (throws an InvalidOperationException if the group is full)
/// - AddSeveralChildren : add all Children into a Table in Children ( if the group is full,send back the number of children without a group)
/// 
/// </summary>

public class Group
{
    public string Name { get; init; } = string.Empty;
    private List<Child> _children = [];

    public ReadOnlyCollection<Child> Children => _children.AsReadOnly();

    public int FullCapacity { get; init; }

    public int CurrentCapacity { get; set; }

    public int AvailableCapacity => FullCapacity - CurrentCapacity;

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

    /// <summary>
    /// add a Child into Children 
    /// 
    /// </summary>
    /// <param name="child"></param>
    /// <exception cref="InvalidOperationException">(throws an InvalidOperationException if the group is full (CurrentCapacity==FullCapacity))</exception>
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
    /// <summary>
    ///  add all Children in a Table into Children
    /// if the group is full,send back the number of children without a group
    /// </summary>
    /// <param name="children"></param>
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

/// <summary>
/// Educator
/// </summary>
/// <param name="identity"> Identituy (Id, Name, Firstname, Nationality)</param>
/// <param name="preferenceType">ChildType</param>
/// <param name="picturePath"> string url to educator picture, can be null</param>
public class Educator(Identity identity, ChildTypes preferenceType, string? picturePath = null) : Person(identity)
{
    public string? PicturePath { get; set; } = picturePath;

    public ChildTypes PreferenceType { get; set; } = preferenceType;

    public override string ToString()
    {
        return $"{Identity.Firstname} {Identity.Name}";
    }
}


public enum ChildTypes { Baby, Toddler, Kid }