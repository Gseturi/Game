using Game.Client.GameComponents.Classes.Sprites;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class SpriteAssetLoader : IAssetLoader<Sprite>
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SpriteAssetLoader> _logger;


    public SpriteAssetLoader(HttpClient httpClient, ILogger<SpriteAssetLoader> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async ValueTask<Sprite> Load(string path)
    {
        _logger.LogInformation($"loading sprite from path: {path}");

        var bytes = await _httpClient.GetByteArrayAsync(path);
        await using var stream = new MemoryStream(bytes);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(stream);
        var size = new Size(image.Width, image.Height);

        var elementRef = new ElementReference(Guid.NewGuid().ToString());
        return new Sprite(path, elementRef, size, bytes, ImageFormatUtils.FromPath(path));
    }
}