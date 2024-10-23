using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

class lobbyHub :Hub
{

    private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

    private static readonly ConcurrentDictionary<string, string> UsersFighting = new ConcurrentDictionary<string, string>();

    


    public override Task OnConnectedAsync()
    {   
        
        string userName;
        var headers = Context.GetHttpContext().Request.Headers;
        if (headers.Count==9) {
            var name = ParseClaimsFromJwt(Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            Claim brapch;
            List<Claim> claims = name.ToList<Claim>();
            userName = claims[1].ToString();
            userName = userName.Substring(userName.IndexOf(":") + 1).Trim();
        }
  
        userName="anonymous"+_connectedUsers.Count;
        /*
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
        */


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

        UsersFighting.TryAdd(Name, requestingUserName);

        await Clients.Client(opid).SendAsync("RequestAccepted", requestingUserName);


    }

    public async Task Send(int x,int y)
    {
        var sender = _connectedUsers[Context.ConnectionId];

        // Find the receiver from UsersFighting dictionary
        var reciverone = UsersFighting.FirstOrDefault(pair => pair.Key.Equals(sender)).Value;
        var reciver = UsersFighting.FirstOrDefault(pair => pair.Value.Equals(sender)).Key;

        // Check if a receiver was found
        if (reciverone == null && reciver == null)
        {
            
            Console.WriteLine("No opponent found for the sender.");
            return;
        }

        // Send the coordinates (x, y) to the receiver's client
        if (reciver != null)
        {
            var OponentId = _connectedUsers.FirstOrDefault(x=>x.Value.Equals(reciver)).Key;
            await Clients.Client(OponentId).SendAsync("InfoRecived", x, y);
        }
        else if (reciverone != null)
        {
            var OponentId = _connectedUsers.FirstOrDefault(x => x.Value.Equals(reciverone)).Key;
            await Clients.Client(OponentId).SendAsync("InfoRecived", x, y);
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}