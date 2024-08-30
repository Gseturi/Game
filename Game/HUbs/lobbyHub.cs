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
        var headers = Context.GetHttpContext().Request.Headers;

        foreach (var header in headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
        var tempp = Context.UserIdentifier;
        string userName = Context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
        var temp=Context.User;
        _connectedUsers.TryAdd(Context.ConnectionId, userName);
        return Task.CompletedTask;
    }


    public Task<List<string>> GetAllPlayers()
    {
        var players = _connectedUsers.Values.ToList();
        return Task.FromResult(players);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedUsers.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task RequestAMatch(String Name)
    {
        var OponentID = _connectedUsers.FirstOrDefault(u => u.Value == Name).Key;

        if (!string.IsNullOrEmpty(OponentID))
        {
            string requestingUserName = _connectedUsers[Context.ConnectionId];
            await Clients.Client(OponentID).SendAsync("ReceiveMatchRequest", requestingUserName);
        }
        else
        {
            await Clients.Caller.SendAsync("OpponentNotFound", OponentID);
        }
    }


}