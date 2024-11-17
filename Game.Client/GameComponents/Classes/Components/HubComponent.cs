
using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Game.Client.GameComponents.Classes.Components
{
    internal class HubComponent : BaseComponent, Ihubable
    {

        public HubConnection hubConnection { get;  set; }
        public TransformComponent transformComponent { get; set; }
        public HubComponent(GameObject owner) : base(owner)
        {
        }

        public GameObject Owner => throw new NotImplementedException();

   
        public override async ValueTask Update(GameContext game)
        {

            Console.WriteLine("seturi");
           // Console.WriteLine(transformComponent.Local.Position.X+"      "+transformComponent.Local.Position.Y);
           await hubConnection.InvokeAsync("Send", transformComponent.Local.Position.X, transformComponent.Local.Position.Y);
         
        }
    }
}