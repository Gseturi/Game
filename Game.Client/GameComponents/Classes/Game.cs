using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes.Components;
using Game.Client.GameComponents.Classes.Sprites;
using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Numerics;

namespace Game.Client.GameComponents.Classes
{
    public class Gameinstance: GameContext
    {

        private readonly Canvas2DContext _context;
        private readonly SceneGraph _sceneGraph;
        private readonly IAssetsResolver _assetsResolver;
        private readonly HubConnection _hubConnection;


        private Gameinstance(Canvas2DContext context, IAssetsResolver assetsResolver,HubConnection hubConnection)
        {
            _context= context;
            _sceneGraph = new SceneGraph(_hubConnection);
             _assetsResolver= assetsResolver;
            _hubConnection= hubConnection;
                
        }

        public static async ValueTask<Gameinstance> Create(Canvas2DContext context, IAssetsResolver assetsResolver, HubConnection hubConnection)
        {
            var game = new Gameinstance(context, assetsResolver,hubConnection);
            GameObject Hubmanager = new GameObject();
            Hubmanager.Components.Add<HubComponent>();


            var playerone=new GameObject();
            var playeroneSprite=assetsResolver.Get<Sprite>("Sprites/MachaPlayer.png").Source;
            var playerOneTransform = playerone.Components.Add<TransformComponent>();

            return game;
        }



        protected override async ValueTask Render()
        {
            await _context.ClearRectAsync(0, 0,this.Display.Size.Width, this.Display.Size.Height);
                
            await  render(_sceneGraph.Root);
        }


        private async ValueTask render(GameObject root)
        {
            if (null == root)
            {
                return;
            }

            foreach(var component in root.Components)
            {
                if (component is Irenderable irenderable)
                {
                    await irenderable.Render(this, _context);
                }

               
            }

       
        }

        protected override async ValueTask Update()
        {
           await _sceneGraph.Update(this);
        }
    }
}
