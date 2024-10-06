using FluentResults;

namespace Adoptrix.Api.Errors;

public class UserNotFoundError(Guid userId) : Error($"User with ID {userId} not found.");
