using Blace.Editing;

namespace WebBuilder2.Client.Utils;

public static class EnumHelper
{
    public static List<T> All<T>()
    {
        Array enums = Enum.GetValues(typeof(T));
        IEnumerable<T> enumsAsSyntax = enums.Cast<T>();
        return enumsAsSyntax.ToList();
    }
}
