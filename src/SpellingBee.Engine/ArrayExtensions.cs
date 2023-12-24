namespace SpellingBee.Engine;

public static class ArrayExtensions
{
    /// <summary>
    /// Appends a new item to the end of a new array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T[] AppendToNewArray<T>(this T[] source, T value)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var newArray = new T[source.Length + 1];

        newArray[source.Length] = value;

        for (var index = 0; index < source.Length; index++)
        {
            newArray[index] = source[index];
        }

        return newArray;
    }
}
