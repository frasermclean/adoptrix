using FluentResults;

namespace Adoptrix.Logic.Errors;

public class DuplicateBreedError(string breedName) : Error($"Breed with name {breedName} already exists.");
