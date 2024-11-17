using Game.Client.GameComponents.Classes.Components;
using System.Numerics;

namespace Game.Client.GameComponents.Classes.PlayerComponent
{
    public class InputComponent : BaseComponent
    {
        InputSystem _InputSystem { get; set; }

        TransformComponent _TransformComponent;
        Transform _Transform { get; set; }
        
        Vector2 _Vector { get; set; }

      
        public InputComponent(GameObject owner) : base(owner)
        {
            Console.WriteLine("times");
            _InputSystem=InputSystem.Instance;
            _TransformComponent = Owner.Components.Get<TransformComponent>();
            _Transform = _TransformComponent.Local;

        }

        public override ValueTask Update(GameContext game)
        {

            if (_InputSystem.GetKeyState((Keys)Keys.Right).State == (ButtonState.States)1)
            {
                _Transform.Position.X++;
            }
            if (_InputSystem.GetKeyState((Keys)Keys.Left).State == (ButtonState.States)1)
            {
                Console.WriteLine("leftKeywasPressed");

                _Transform.Position.X--;
            }
            if (_InputSystem.GetKeyState((Keys)Keys.Up).State == (ButtonState.States)1)
            {
                Console.WriteLine("upKeywasPressed");
                _Transform.Position.Y--;

            }
            if (_InputSystem.GetKeyState((Keys)Keys.Down).State == (ButtonState.States)1)
            {
                Console.WriteLine("downKeywasPressed");
                _Transform.Position.Y++;
            }

            return ValueTask.CompletedTask;
        }
    }
   }
