using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes.Sprites;

namespace Game.Client.GameComponents.Classes
{
    public class Game: GameContext
    {

        private readonly Canvas2DContext _context;
        private readonly SceneGraph _sceneGraph;
        private readonly IAssetsResolver _assetsResolver;


        private Game(Canvas2DContext context, SceneGraph sceneGraph, IAssetsResolver assetsResolver)
        {
            _context= context;
            _sceneGraph = sceneGraph;   
             _assetsResolver= assetsResolver;
                
        }


        protected override async ValueTask Render()
        {
            throw new NotImplementedException();
        }


        protected override async ValueTask Update()
        {
            throw new NotImplementedException();
        }
    }
}
