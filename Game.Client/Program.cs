using Blazored.LocalStorage;
using Game.Client.GameComponents.Classes.Sprites;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthenticationStateProviderr>();
try
{
    builder.Services.AddSingleton<IAssetLoader<Sprite>, SpriteAssetLoader>();

    builder.Services.AddSingleton<IAssetLoaderFactory>(ctx =>
    {
        var factory = new AssetLoaderFactory();

    
        var spriteLoader = ctx.GetService<IAssetLoader<Sprite>>();
        if (spriteLoader == null)
        {
            throw new InvalidOperationException("Failed to retrieve SpriteAssetLoader.");
        }

        factory.Register(spriteLoader);

       

        return factory;
    });

    builder.Services.AddSingleton<IAssetsResolver, AssetsResolver>();

   
   


    

}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
await builder.Build().RunAsync();
