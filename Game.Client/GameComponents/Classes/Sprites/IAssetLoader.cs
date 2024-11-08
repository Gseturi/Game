using Game.Client.GameComponents.Classes.Sprites;

public interface IAssetLoader<TA> where TA : IAsset
{
    ValueTask<TA> Load(string path);
}