namespace Adoptrix.Tests.Shared;

public static class SampleData
{
    public static readonly string[] Names = ["Buddy", "Max", "Bella", "Lucy", "Charlie", "Daisy", "Bailey", "Molly"];

    public static string RandomName => Names[Random.Shared.Next(Names.Length)];
}
