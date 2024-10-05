using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace PinewoodTechnologies.Code
{
    public static class Extensions
    {
        public static List<T> ToPagedList<T>(this List<T> query, int page, int size, string sort)
        {
            return query.OrderBy(item => GetPropertyValue(item, sort)) // Ensure proper ordering
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToList();
        }
        public static object GetPropertyValue(object obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return propertyInfo?.GetValue(obj, null);
        }
    }
}
