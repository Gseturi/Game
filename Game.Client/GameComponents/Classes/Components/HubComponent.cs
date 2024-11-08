
using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Game.Client.GameComponents.Classes.Components
{
    internal class HubComponent : BaseComponent, Ihubable
    {
        public HubComponent(GameObject owner) : base(owner)
        {
        }

        public GameObject Owner => throw new NotImplementedException();

   
        public ValueTask Update(GameContext game, HubConnection hubConnection)
        {
            hubConnection.On<int, int>("InfoRecived", async (x, y) =>
            {
                Console.WriteLine("enemyinfo:"+x+y);
                

            });
        }
    }
}