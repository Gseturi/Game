using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Claims;

class lobbyHub :Hub
{

    private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

    private static readonly List<string[]> UsersFighting = new List<string[]>();




    public override Task OnConnectedAsync()
    {   

        /// this needs to change bad 
      
        string Name= "Anonymous";
        ////
        ///
        string userName;
        var headers = Context.GetHttpContext().Request.Headers;

        foreach (var header in headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
        var tempp = Context.UserIdentifier;

        if (_connectedUsers.Count==0)
        {
        userName = Context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
        }
        else
        {
         userName = Context.User.FindFirst(ClaimTypes.Name)?.Value ?? Name+ _connectedUsers.Count;
        }
       

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

    public async Task AcceptAMatch(string Name)
    {
        var opid=_connectedUsers.FirstOrDefault(sp=>sp.Value==Name).Key;

        string requestingUserName = _connectedUsers[Context.ConnectionId];

        UsersFighting.Add(new string[2]{ Name, requestingUserName });

        await Clients.Client(opid).SendAsync("RequestAccepted", requestingUserName);


    }


}