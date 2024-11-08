using Game.Client.GameComponents.Classes.Sprites;
using System.Collections.Concurrent;
using System.IO;

 class AssetsResolver: IAssetsResolver
{
    private readonly ConcurrentDictionary<string, IAsset> _assets;
    private readonly IAssetLoaderFactory _assetLoaderFactory;
    private readonly ILogger<AssetsResolver> _logger;

    public AssetsResolver(IAssetLoaderFactory assetLoaderFactory, ILogger<AssetsResolver> logger)
    {
        _assetLoaderFactory = assetLoaderFactory;
        _logger = logger;
        _assets = new ConcurrentDictionary<string, IAsset>();
    }

    public async ValueTask<Ta> Load<Ta>(string assetpath) where Ta : IAsset
    {
          _logger.LogInformation("GetAsset: {AssetName}", assetpath);

        var Loader = _assetLoaderFactory.Get<Ta>();
        var asset=await Loader.Load(assetpath);


        if (asset is null) throw new TypeLoadException($"unable to load asset type '{typeof(Ta)}' from path '{assetpath}'");

        _assets.AddOrUpdate(assetpath, k => asset, (k, v) => asset);
        return asset;
    }

    public TA Get<TA>(string name) where TA : class, IAsset => _assets[name] as TA;

}