using System;

public static class Extensions
{
    public static T Next<T>(this T current) where T : Enum
    {
        T[] values = (T[])Enum.GetValues(current.GetType());
        int index = Math.Min(Array.IndexOf(values, current) + 1, values.Length - 1);
        return values[index];
    }
}