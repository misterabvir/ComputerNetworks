using System.Net;
using System.Net.Sockets;

namespace server;

public class ChatServer(string host, int port)
{
    private readonly TcpListener _server = new(IPAddress.Parse(host), port);
    private readonly HashSet<User> _users = [];

    internal async Task Start()
    {
        _server.Start();
        this.PrintInfo("Server is listening...");

        while (true)
        {
            TcpClient client = await _server.AcceptTcpClientAsync();
            this.PrintInfo($"Connected with {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");
            _ = Task.Run(() => HandleClient(client));
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        StreamReader reader = new(client.GetStream());
        StreamWriter writer = new(client.GetStream()) { AutoFlush = true };

        await writer.WriteLineAsync("NICK");
        string nickname = (await reader.ReadLineAsync())!;
        _users.Add(new(client, nickname));
        this.PrintInfo($"Nickname is {nickname}");
        await BroadcastAsync($"{nickname} joined!");

        while (true)
        {
            try
            {
                string message = (await reader.ReadLineAsync())!;
                await BroadcastAsync(message);
            }
            catch
            {
                var user = _users.First(u => u.Client == client);                
                _users.Remove(user);
                string message = $"{nickname} left!";  
                this.PrintError(message);
                await BroadcastAsync($"{nickname} left!");
                reader.Close();
                writer.Close();
                client.Close();
                this.PrintInfo($"Disconnected with {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");
                break;
            }
        }
    }

    private async Task BroadcastAsync(string message)
    {
        foreach (User user in _users)
        {
            StreamWriter writer = new(user.Client.GetStream()) { AutoFlush = true };
            await writer.WriteLineAsync(message);
        }
    }
}


public static class ChatServiceExtensions
{
    public static void PrintError(this ChatServer server, string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;    
        Console.WriteLine($"Error: {DateTime.UtcNow}");
        Console.ResetColor();
        Console.WriteLine(message);
    }

    public static void PrintInfo(this ChatServer server, string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;    
        Console.WriteLine($"Info: {DateTime.UtcNow}");
        Console.ResetColor();
        Console.WriteLine(message);
    }
}