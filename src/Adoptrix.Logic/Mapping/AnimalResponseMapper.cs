using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Logic.Mapping;

[Mapper]
[UseStaticMapper(typeof(AnimalImageResponseMapper))]
public static partial class AnimalResponseMapper
{
    [MapProperty("Breed.Name", "BreedName")]
    [MapProperty("Breed.Species.Name", "SpeciesName")]
    [MapProperty("DateOfBirth", "Age", Use = nameof(MapAge))]
    public static partial AnimalResponse ToResponse(this Animal animal);

    private static DateTime ConvertToUtc(DateTime dateTime) => dateTime.ToUniversalTime();

    private static string MapAge(DateOnly dateOfBirth)
    {
        var now = DateTime.UtcNow;
        var months = now.Month - dateOfBirth.Month;
        var years = now.Year - dateOfBirth.Year;

        if (now.Day < dateOfBirth.Day)
        {
            months--;
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return $"{years} year{(years == 1 ? "" : "s")}, {months} month{(months == 1 ? "" : "s")}";
    }
}
