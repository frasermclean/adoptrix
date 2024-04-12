﻿using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Features.Species.Queries;

public record GetAllSpeciesQuery : IRequest<IEnumerable<SpeciesModel>>;
