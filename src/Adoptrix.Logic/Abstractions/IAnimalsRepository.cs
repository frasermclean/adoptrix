﻿using Adoptrix.Core;

namespace Adoptrix.Logic.Abstractions;

public interface IAnimalsRepository
{
    Task<Animal?> GetAsync(Guid animalId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
