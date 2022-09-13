using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum e)
    {
        var fi = e.GetType().GetField(e.ToString());
        if (fi == null) return string.Empty;

        var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
        return (attrs != null && attrs.Length > 0)
            ? ((DescriptionAttribute)attrs[0]).Description
            : string.Empty;
    }
}