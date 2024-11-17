using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes.Components;
using Game.Client.GameComponents.Classes.PlayerComponent;
using Game.Client.GameComponents.Classes.Sprites;
using Game.Client.GameComponents.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Drawing;
using System.Numerics;
using System.Xml.Linq;

namespace Game.Client.GameComponents.Classes
{
    internal class Gameinstance: GameContext
    {

        private readonly Canvas2DContext _context;
        private readonly SceneGraph _sceneGraph;
        private readonly IAssetsResolver _assetsResolver;
        private readonly HubConnection _hubConnection;


        internal Gameinstance(Canvas2DContext context, IAssetsResolver assetsResolver,HubConnection hubConnection)
        {
            _context= context;
            _sceneGraph = new SceneGraph(_hubConnection);
             _assetsResolver= assetsResolver;
            _hubConnection= hubConnection;
                
        }

       
        internal static async ValueTask<Gameinstance> Create(Canvas2DContext context, IAssetsResolver assetsResolver, HubConnection hubConnection,String CurrentPlayer)
        {
            var game = new Gameinstance(context, assetsResolver,hubConnection);
            

            


            if (CurrentPlayer=="mach")
            {
                var playerone = new GameObject();
                var playerOneTransform = playerone.Components.Add<TransformComponent>();
                var PlayeroneInputStstem = playerone.Components.Add<InputComponent>();

                playerone.Components.AddReciverHUb(hubConnection);
                var playeroneSprite = assetsResolver.Get<Sprite>("Sprites/MachaPlayer.png");


                playerOneTransform.Local.Position.X = 100;
                playerOneTransform.Local.Position.Y = 100;

                var playeroneRender = playerone.Components.Add<SpriteRenderComponent>();
                var playeroneSender = playerone.Components.Add<HubComponent>();
                playeroneSender.hubConnection = hubConnection;
                playeroneSender.transformComponent = playerOneTransform;
                playeroneRender.Sprite = playeroneSprite;
                game._sceneGraph.Root.AddChild(playerone);

                var playertwo = new GameObject();
                var playertwoTransform = playertwo.Components.Add<TransformComponent>();
                playertwo.Components.AddReciverHUb(hubConnection);
                var playertwoSprite = assetsResolver.Get<Sprite>("Sprites/PlayerGuram.png");
                playertwoSprite.Origin = new Point(100, 100);
                playertwoSprite.Size = new Size(100, 100);
                var playerTwoReciver = playertwo.Components.Add<HubReciver>();
                playerTwoReciver.hubConnection = hubConnection;
                playerTwoReciver._transform = playertwoTransform;
                playertwoTransform.Local.Position.X = 200;
                playertwoTransform.Local.Position.Y = 100;
                playertwoTransform.Local.Scale = new Vector2(100, 100);

                var playertwoRender = playertwo.Components.Add<SpriteRenderComponent>();
                playertwoRender.Sprite = playertwoSprite;
                game._sceneGraph.Root.AddChild(playertwo);
            }
            else if (CurrentPlayer=="guram")
            {
                var playerone = new GameObject();
                var playerOneTransform = playerone.Components.Add<TransformComponent>();
                var PlayeroneInputStstem = playerone.Components.Add<InputComponent>();
                playerOneTransform.Local.Scale= new Vector2(100, 100);
                playerone.Components.AddReciverHUb(hubConnection);
                var playeroneSprite = assetsResolver.Get<Sprite>("Sprites/PlayerGuram.png");
                playeroneSprite.Origin = new Point(100, 100);
                playeroneSprite.Size = new Size(100, 100);

                playerOneTransform.Local.Position.X = 200;
                playerOneTransform.Local.Position.Y = 100;

                var playeroneRender = playerone.Components.Add<SpriteRenderComponent>();
                var playeroneSender = playerone.Components.Add<HubComponent>();
                playeroneSender.hubConnection = hubConnection;
                playeroneSender.transformComponent = playerOneTransform;
                playeroneRender.Sprite = playeroneSprite;
                game._sceneGraph.Root.AddChild(playerone);

                var playertwo = new GameObject();
                var playertwoTransform = playertwo.Components.Add<TransformComponent>();
                playertwo.Components.AddReciverHUb(hubConnection);
                var playertwoSprite = assetsResolver.Get<Sprite>("Sprites/MachaPlayer.png");
                playertwoSprite.Origin = new Point(100, 100);
                playertwoSprite.Size = new Size(100, 100);
                var playerTwoReciver = playertwo.Components.Add<HubReciver>();
                playerTwoReciver.hubConnection = hubConnection;
                playerTwoReciver._transform = playertwoTransform;
                playertwoTransform.Local.Position.X = 100;
                playertwoTransform.Local.Position.Y = 100;
                playertwoTransform.Local.Scale = new Vector2(100, 100);

                var playertwoRender = playertwo.Components.Add<SpriteRenderComponent>();
                playertwoRender.Sprite = playertwoSprite;
                game._sceneGraph.Root.AddChild(playertwo);


            }

           

            

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
     
            foreach (var component in root.Components)
            {
               
                if (component is Irenderable irenderable)
                {
                   
                    await irenderable.Render(this, _context);
                }

               
            }
            foreach (var child in root.Children)
                await render(child);



        }

        protected override async ValueTask Update()
        {
           
           await _sceneGraph.Update(this);
        }
    }
}
