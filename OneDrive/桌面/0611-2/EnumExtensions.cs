using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute == null ? value.ToString() : displayAttribute.Name;
    }
}
