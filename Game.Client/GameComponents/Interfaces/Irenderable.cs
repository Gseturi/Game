using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes;

namespace Game.Client.GameComponents.Interfaces
{
    public interface Irenderable
    {


         ValueTask Render(GameContext game, Canvas2DContext context);
    }
}