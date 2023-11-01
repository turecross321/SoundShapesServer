using System.Reflection;
using AttribDoc;
using AttribDoc.Attributes;
using SoundShapesServer.Attributes;

namespace SoundShapesServer.Documentation.Attributes;

public class DocUsesOrderAttribute<TEnum> : DocAttribute where TEnum : struct, Enum
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        List<OrderTypeAttribute> types = new ();
        Type enumType = typeof(TEnum);

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (TEnum value in Enum.GetValues(enumType))
        {
            FieldInfo? fieldInfo = enumType.GetField(value.ToString());
            if (fieldInfo == null)
                continue;
            
            // Check if the enum value has an OrderType attribute
            OrderTypeAttribute? orderTypeAttribute = (OrderTypeAttribute?)fieldInfo.GetCustomAttribute(typeof(OrderTypeAttribute), false);
            if (orderTypeAttribute != null)
            {
                types.Add(orderTypeAttribute);
            }
        }
        
        route.Parameters.Add(new Parameter("orderBy", ParameterType.Query, "Specifies how list should be ordered."));
        route.ExtraProperties.Add("orderTypes", types);
    }
}