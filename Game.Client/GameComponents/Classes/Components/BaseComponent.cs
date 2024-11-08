namespace Game.Client.GameComponents.Classes.Components
{
    public abstract class BaseComponent : IComponent
    {
        protected BaseComponent(GameObject owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public virtual async ValueTask Update(GameContext game)
        {
        }

        public GameObject Owner { get; }
    }
}