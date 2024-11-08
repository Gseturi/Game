using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Game.Client.GameComponents.Classes
{
    internal class SceneGraph
    {
        HubConnection hubConnection;
        public SceneGraph(HubConnection hubConnection)
        {
            Root = new GameObject();
            this.hubConnection = hubConnection;
        }

        public async ValueTask Update(GameContext game)
        {
            if (null == Root)
                return;
            if(Root is Ihubable hubable)
            { 
                await hubable.Update(game, hubConnection);

            }

            await Root.Update(game);
        }

        public GameObject Root { get; }
    }
}