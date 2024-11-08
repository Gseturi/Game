using Game.Client.GameComponents.Classes.Components;

namespace Game.Client.GameComponents.Classes.PlayerComponent
{
    public class ConnectionComp : BaseComponent
    {
        public ConnectionComp(GameObject owner) : base(owner)
        {


        }
        public override ValueTask Update(GameContext game)
        {

            return base.Update(game);
        }

    }
}
