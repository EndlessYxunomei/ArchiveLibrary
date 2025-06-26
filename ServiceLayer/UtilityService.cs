using ArchiveModels.Interfaces;
using System.Collections.ObjectModel;

namespace ServiceLayer;

public class UtilityService
{
    public static void UpdateList<T>(ObservableCollection<T> list, T updateItem) where T : IIdentityModel
    {
        T? exist = list.FirstOrDefault(x => x?.Id == updateItem.Id);
        if (exist != null)
        {
            list[list.IndexOf(exist)] = updateItem;
        }
        else
        {
            list.Add(updateItem);
        }
    }
}
