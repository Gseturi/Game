using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Game.Client.GameComponents.Interfaces
{
 
     interface Ihubable
    {

      ValueTask Update(GameContext game);

    }
}