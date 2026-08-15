using System.ComponentModel;
using System.Reflection;

namespace FinTrack.Application.Common.Helpers;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? value.ToString();
    }
}
