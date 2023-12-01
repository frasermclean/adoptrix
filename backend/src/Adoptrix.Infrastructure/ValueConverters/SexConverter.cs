using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adoptrix.Infrastructure.ValueConverters;

public class SexConverter()
    : ValueConverter<Sex?, char?>(sex => SexToChar(sex), c => CharToSex(c))
{
    private const char MaleChar = 'M';
    private const char FemaleChar = 'F';

    private static char? SexToChar(Sex? sex) => sex switch
    {
        Sex.Male => MaleChar,
        Sex.Female => FemaleChar,
        _ => null
    };

    private static Sex? CharToSex(char? c) => c switch
    {
        MaleChar => Sex.Male,
        FemaleChar => Sex.Female,
        _ => null
    };
}