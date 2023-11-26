using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adoptrix.Infrastructure.Converters;

public class SpeciesCodeConverter : ValueConverter<Species, string>
{
    public const int CodeLength = 2;

    private const string DogCode = "DG";
    private const string CatCode = "CT";
    private const string HorseCode = "HS";

    public SpeciesCodeConverter()
        : base(species => GetCodeFromSpecies(species),
            code => GetSpeciesFromCode(code))
    {
    }

    private static string GetCodeFromSpecies(Species species) =>
        species switch
        {
            Species.Dog => DogCode,
            Species.Cat => CatCode,
            Species.Horse => HorseCode,
            _ => throw new ArgumentOutOfRangeException(nameof(species), species, null)
        };

    private static Species GetSpeciesFromCode(string code) =>
        code switch
        {
            DogCode => Species.Dog,
            CatCode => Species.Cat,
            HorseCode => Species.Horse,
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
}