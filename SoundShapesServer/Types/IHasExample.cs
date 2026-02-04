namespace SoundShapesServer.Types;

public interface IHasExample<out TSelf> where TSelf : IHasExample<TSelf>
{
    static abstract TSelf Example();
}