﻿using System.Net;
using System.Reflection;
using Bunkum.Core;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions;

public static class RequestContextExtensions
{
    public static (int, int, bool) GetPageData(this RequestContext context, bool descendingIfNull = true)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = context.QueryString["descending"].ToBool() ?? descendingIfNull;

        return (from, count, descending);
    }

    public static T GetFilters<T>(this RequestContext context, GameDatabaseContext database) where T : IFilters, new()
    {
        T instance = new();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            FilterPropertyAttribute? attribute = property.GetCustomAttribute<FilterPropertyAttribute>();
            if (attribute == null) 
                continue;
            
            string? strValue = context.QueryString[attribute.ParameterName];
            if (strValue == null)
                continue;

            object? value = null;

            // typeof can't be used in switch statements :/
            if (property.PropertyType == typeof(GameUser))
            {
                value = database.GetUserWithId(strValue);
            }
            else if (property.PropertyType == typeof(GameLevel))
            {
                value = database.GetLevelWithId(strValue);
            }
            else if (property.PropertyType == typeof(GameAlbum))
            {
                value = database.GetAlbumWithId(strValue);
            }
            else if (property.PropertyType == typeof(bool?))
            {
                value = strValue.ToBool();
            }
            else if (property.PropertyType == typeof(string))
            {
                value = strValue;
            }
            else if (property.PropertyType == typeof(DateTimeOffset?))
            {
                if (DateTimeOffset.TryParse(strValue, out DateTimeOffset result)) value = result;
            }
            else if (property.PropertyType == typeof(int?))
            {
                value = strValue.ToInt();
            }
            else if (property.PropertyType == typeof(int[]))
            {
                List<int> list = [];
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (string numberStr in strValue.Split(","))
                {
                    int? number = numberStr.ToInt();
                    if (number != null) list.Add((int)number);
                }

                value = Convert.ChangeType(list.ToArray(), property.PropertyType);
            }
            else if (property.PropertyType == typeof(GameUser[]))
            {
                value = strValue.Split(",").Select(id => database.GetUserWithId(id)).OfType<GameUser>().ToArray();
            }
            else if (property.PropertyType == typeof(GameLevel[]))
            {
                string[] ids = strValue.Split(",");
                value = ids.Select(database.GetLevelWithId).OfType<GameLevel>().ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException(property.PropertyType.Name + " filter is not supported");
            }

            property.SetValue(instance, value);
        }

        return instance;
    }

    public static TEnum? GetOrderType<TEnum>(this RequestContext context) where TEnum : struct, Enum
    {
        Type enumType = typeof(TEnum);

        foreach (TEnum value in Enum.GetValues(enumType))
        {
            FieldInfo? fieldInfo = enumType.GetField(value.ToString());
            if (fieldInfo == null)
                continue;

            // Check if the enum value has an OrderType attribute
            OrderTypeAttribute? orderTypeAttribute =
                (OrderTypeAttribute?)fieldInfo.GetCustomAttribute(typeof(OrderTypeAttribute), false);

            if (orderTypeAttribute != null && orderTypeAttribute.Value == context.QueryString["orderBy"]) return value;
        }

        return null;
    }

    public static string GetIpAddress(this RequestContext context)
    {
        return ((IPEndPoint)context.RemoteEndpoint).Address.ToString();
        ;
    }

    public static GameIp? GetGameIp(this RequestContext context, GameDatabaseContext database, GameUser user)
    {
        string ipAddress = GetIpAddress(context);
        GameIp? gameIp = database.GetIpWithAddress(user, ipAddress);

        return gameIp;
    }
}