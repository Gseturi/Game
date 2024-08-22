using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Claims;

class lobbyHub :Hub
{

    private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

   

   

    public override Task OnConnectedAsync()
    {
        string userName = Context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
        _connectedUsers.TryAdd(Context.ConnectionId, userName);
        return Task.CompletedTask;
    }


    public async Task GetAllPlayer()
    {




    }

    public async Task RequestAMatch()
    {



    }


}