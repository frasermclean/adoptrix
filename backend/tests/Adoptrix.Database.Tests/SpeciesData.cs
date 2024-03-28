namespace Adoptrix.Database.Tests;

public class SpeciesData : TheoryData<Guid, string>
{
    public SpeciesData()
    {
        Add(SpeciesIds.Dog, "Dog");
        Add(SpeciesIds.Cat, "Cat");
        Add(SpeciesIds.Horse, "Horse");
    }
}
