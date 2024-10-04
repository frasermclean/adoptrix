using Adoptrix.Core;
using FluentResults;

namespace Adoptrix.Logic.Errors;

public class UserRoleAlreadyAssignedError(UserRole role) : Error($"User role {role} is already assigned to this user");
