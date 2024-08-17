namespace ExpenseTracker.API.Requests.Common;

//https://stackoverflow.com/questions/29482/how-do-i-cast-int-to-enum-in-c
//https://www.tutorialsteacher.com/articles/convert-int-to-enum-in-csharp

public static class IntToEnum
{
    public static T? Handle<T>(int value) where T : struct, Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
        {
            return null;
        }
        return (T)Enum.ToObject(typeof(T), value);
    }
}
