using FluentResults;

namespace Adoptrix.Api.Extensions;

public static class ResultExtensions
{
    public static string GetFirstErrorMessage(this IResultBase result)=>
        result.Errors.First().Message;
}