using FluentResults;

namespace Adoptrix.Logic.Errors;

public class UserNotFoundError(Guid userId) : Error($"User with ID {userId} not found.");
