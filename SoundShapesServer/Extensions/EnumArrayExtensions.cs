namespace SoundShapesServer.Extensions;

public static class EnumArrayExtensions
{
    public static int[] ToIntArray<T>(this T[] array) where T : struct, Enum
    {
        List<int> intList = array.Select(enumValue => (int)Enum.ToObject(typeof(T), enumValue)).ToList();
        return intList.ToArray();
    }
}