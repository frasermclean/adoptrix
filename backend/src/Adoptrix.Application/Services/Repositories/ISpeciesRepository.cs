﻿using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface ISpeciesRepository
{
    Task<Result<Species>> GetSpeciesByIdAsync(int speciesId, CancellationToken cancellationToken = default);
    Task<Result<Species>> GetSpeciesByNameAsync(string name, CancellationToken cancellationToken = default);
}