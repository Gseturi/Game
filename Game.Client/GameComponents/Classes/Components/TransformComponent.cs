using System.Threading.Tasks;

namespace Game.Client.GameComponents.Classes.Components
{
    public class TransformComponent : BaseComponent
    {
        private Transform _local = Transform.Identity();
        private readonly Transform _world = Transform.Identity();

        public TransformComponent(GameObject owner) : base(owner)
        {
        }

        public override async ValueTask Update(GameContext game)
        {
            _world.Clone(ref _local);

            if (null != Owner.Parent && Owner.Parent.Components.TryGet<TransformComponent>(out var parentTransform))
                _world.Position = _local.Position + parentTransform.World.Position;
        }

         Transform Local => _local;
         Transform World => _world;
    }

    
}