using Blazor.Extensions.Canvas.Canvas2D;
using Game.Client.GameComponents.Classes.Sprites;
using Game.Client.GameComponents.Interfaces;

namespace Game.Client.GameComponents.Classes.Components
{
    internal class SpriteRenderComponent : BaseComponent, Irenderable
    {
        TransformComponent _transform;


        public SpriteRenderComponent(GameObject owner) : base(owner)
        {
            _transform = owner.Components.Get<TransformComponent>();
        }

        public async ValueTask Render(GameContext game, Canvas2DContext context)
        {

          //  Console.WriteLine("es aris rasa" + _transform.Local.Position.X);
            await context.SaveAsync();
   
            await context.TranslateAsync(_transform.World.Position.X, _transform.World.Position.Y);
            await context.RotateAsync(_transform.World.Rotation);

            await context.DrawImageAsync(Sprite.Source, -Sprite.Origin.X, -Sprite.Origin.Y,
                Sprite.Size.Width, Sprite.Size.Height);

            await context.RestoreAsync();
           
        }
        public Sprite Sprite { get; set; }
    }
}