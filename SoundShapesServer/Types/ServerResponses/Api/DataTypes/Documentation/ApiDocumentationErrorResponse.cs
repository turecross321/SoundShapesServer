﻿using AttribDoc;
using SoundShapesServer.Types.ServerResponses.Api.ApiTypes;

namespace SoundShapesServer.Types.ServerResponses.Api.DataTypes.Documentation;

public record ApiDocumentationErrorResponse : IApiResponse
{
    public required string Name { get; set; }
    public required string OccursWhen { get; set; }
    
    private static ApiDocumentationErrorResponse FromError(Error old)
    {
        return new ApiDocumentationErrorResponse
        {
            Name = old.Name,
            OccursWhen = old.OccursWhen,
        };
    }
    
    public static IEnumerable<ApiDocumentationErrorResponse> FromErrorList(IEnumerable<Error> oldList) => oldList.Select(FromError);
}