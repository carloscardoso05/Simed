using System;

public static class Extensions
{

    public static T Next<T>(this T src) where T : Enum
    {
        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int index = Math.Min(Array.IndexOf(Arr, src) + 1, Arr.Length-1);
        return Arr[index];            
    }
}