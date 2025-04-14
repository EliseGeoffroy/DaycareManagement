using System.Collections.ObjectModel;
using ChildrenManagement.staticClasses;

namespace ChildrenManagement.Classes;

/// <summary>
/// Children grouped by age, supervised by several Educators
/// 
/// Send an event JustOnePlaceAvailableEvent if there's no more left spots for an age-range.
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
/// - AddAnEducator 
/// - (static) FindAGroup(Child child) : find an available and age adapted group for the child
/// 
/// </summary>

public class Group
{

    public static event EventHandler<ChildTypes>? JustOnePlaceAvailableEvent;
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


    public void AddAnEducator(Educator educator)
    {
        _educators.Add(educator);
    }

    /// <summary>
    /// find an available and age adapted group for the child
    ///invoke an event if no more spot for this age-range after the inscription
    /// </summary>
    /// <param name="child">Child child to put in a group</param>
    /// <exception cref="InvalidOperationException">throws an exception if no possibilities</exception>
    /// 
    public static void FindAGroup(Child child)
    {
        if (Datas.GroupDictionary.Values.Where(g => g.ChildType == child.ChildType).Sum(g => g.AvailableCapacity) == 0)
        {

            Datas.ChildrenDictionary.Remove(child.Identity.Id);
            if (child.ContactList.Any())
            {
                foreach (TrustedPerson trustedPerson in child.ContactList)
                {
                    Datas.TrustedPeopleDictionary.Remove(trustedPerson.Identity.Id);
                }
            }

            throw new InvalidOperationException("Aucune place n'est disponible pour cet âge. L'inscription est annulée.");
        }
        else
        {
            Group? group = Datas.GroupDictionary.Values.Where(g => g.ChildType == child.ChildType && g.CurrentCapacity < g.FullCapacity).FirstOrDefault();
            if (group != null)
            {
                child.Group = group;
                group.AddAChild(child);
            }
            if (Datas.GroupDictionary.Values.Where(g => g.ChildType == child.ChildType).Sum(g => g.AvailableCapacity) == 0)
            {
                JustOnePlaceAvailableEvent?.Invoke(child, child.ChildType);
            }
        }

    }
}