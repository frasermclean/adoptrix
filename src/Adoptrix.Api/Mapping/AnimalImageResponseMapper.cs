﻿using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class AnimalImageResponseMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}

