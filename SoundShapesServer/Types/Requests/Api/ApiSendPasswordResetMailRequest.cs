﻿namespace SoundShapesServer.Types.Requests.Api;

public record ApiSendPasswordResetMailRequest : IApiRequest
{
    public string Email { get; init; }
}