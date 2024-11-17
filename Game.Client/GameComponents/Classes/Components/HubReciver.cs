using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Numerics;

namespace Game.Client.GameComponents.Classes.Components
{
    internal class HubReciver : BaseComponent, Ihubable
    {
        public TransformComponent _transform { get; set; }
        public HubConnection hubConnection { get; set; }
        public HubReciver(GameObject owner,HubConnection _hub) : base(owner)
        {
            _transform=owner.Components.Get<TransformComponent>();
            hubConnection = _hub;
        }

        public override  async ValueTask Update(GameContext game)
        {
            Console.WriteLine("brapchadaputaria");
            hubConnection.On<int, int>("InfoRecived", (x, y) =>
            {
                Console.WriteLine("brapchadaputaria");
                Console.WriteLine("enemyinfo:" + x + y);
                Console.WriteLine(x);
                Console.WriteLine(y);
                _transform.Local.Position.X = x;
                _transform.Local.Position.Y = y;
            });
           
     

            
        }
    }
}
