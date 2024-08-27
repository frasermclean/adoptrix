using FluentResults;

namespace Adoptrix.Logic.Errors;

public class SpeciesNotFoundError(string speciesName) : Error($"Species with name {speciesName} not found.");
