namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public interface IApiResponse<out TSelf> : IHasExample<TSelf> where TSelf : IHasExample<TSelf>
{
}
