using FluentResults;

namespace Adoptrix.Core.Errors;

public class SpeciesNotFoundError(Guid speciesId)
    : Error($"Species with ID: {speciesId} was not found"), INotFoundError;
