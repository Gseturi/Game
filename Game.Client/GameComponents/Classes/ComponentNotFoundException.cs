
namespace Game.Client.GameComponents.Classes
{
    [Serializable]
    internal class ComponentNotFoundException<T> : Exception where T : class, IComponent
    {
        public ComponentNotFoundException()
        {
        }

        public ComponentNotFoundException(string? message) : base(message)
        {
        }

        public ComponentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}