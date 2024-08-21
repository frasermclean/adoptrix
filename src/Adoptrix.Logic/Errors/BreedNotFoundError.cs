using FluentResults;

namespace Adoptrix.Logic.Errors;

public class BreedNotFoundError(int breedId) : Error($"Breed with ID {breedId} not found.");
