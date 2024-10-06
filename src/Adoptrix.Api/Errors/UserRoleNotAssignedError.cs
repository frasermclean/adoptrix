using Adoptrix.Core;
using FluentResults;

namespace Adoptrix.Api.Errors;

public class UserRoleNotAssignedError(UserRole role) : Error($"User does not have the role {role} assigned.");
