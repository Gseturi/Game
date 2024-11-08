using Game.Client.GameComponents.Classes.Sprites;

internal interface IAssetLoaderFactory
{
    IAssetLoader<TA> Get<TA>() where TA : IAsset;
}