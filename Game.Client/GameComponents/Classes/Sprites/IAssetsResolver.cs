namespace Game.Client.GameComponents.Classes.Sprites
{
    internal interface IAssetsResolver
    {
        ValueTask<TA> Load<TA>(string path) where TA : IAsset;
        TA Get<TA>(string name) where TA : class, IAsset;
    }

    public interface IAsset
    {
        string Name { get; }
    }
}