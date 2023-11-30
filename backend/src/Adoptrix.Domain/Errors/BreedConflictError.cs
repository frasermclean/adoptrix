using FluentResults;

namespace Adoptrix.Domain.Errors;

public class BreedConflictError(string name) : Error($"Breed with name: '{name}' already exists.");