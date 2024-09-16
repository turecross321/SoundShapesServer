﻿using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Common.Types.Responses.Api.DataTypes;

public record ApiEulaResponse : IApiResponse
{
    public required string CustomText { get; init; }
    public required string License { get; init; }
    public required string RepositoryUrl { get; init; }
}