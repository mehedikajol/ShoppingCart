using System.ComponentModel;

namespace ShoppingCart.SharedKerel.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var description = value.ToString();
        var fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo is not null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs is not null && attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return description;
    }

    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
            typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null)!;
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null)!;
            }
        }

        throw new ArgumentException("Provided value not found.");
        // Or return default(T);
    }
}
