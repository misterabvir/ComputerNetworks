using System.Net.Sockets;

internal class ChatClient
{
    private readonly string _nickname;
    private readonly TcpClient _client;

    public ChatClient(string nickname)
    {
        _nickname = nickname;
        _client = new TcpClient();
    }

    internal async Task ConnectAsync(string ipAddress, int port)
    {
        try
        {
            await _client.ConnectAsync(ipAddress, port);

            _ = Task.Run(Receive);
            await Write();
        }
        catch (Exception ex)
        {
            this.PrintError($"Connect.Error: {ex.Message}");
        }
        finally
        {
            _client.Close();
        }
    }

    private async Task Receive()
    {
        try
        {
            StreamReader reader = new(_client.GetStream());

            while (true)
            {
                string message = (await reader.ReadLineAsync())!;
                if (message == "NICK")
                {
                    await SendMessage(_nickname);
                }
                else
                {
                   this.PrintConversationMessage(message);
                }
            }
        }
        catch (Exception ex)
        {
            this.PrintError($"ReceiveError: {ex.Message}");    
        }
    }

    private async Task Write()
    {
        try
        {
            StreamWriter writer = new(_client.GetStream()) { AutoFlush = true };

            while (true)
            {
                string message = $"{_nickname}: {Console.ReadLine()}";
                await SendMessage(message);
            }
        }
        catch (Exception ex)
        {
            this.PrintError($"An error occurred: {ex.Message}");
        }
    }

    private async Task SendMessage(string message)
    {
        StreamWriter writer = new(_client.GetStream()) { AutoFlush = true };
        await writer.WriteLineAsync(message);
    }
}

internal static class ChatClientExtensions{
    public static void PrintError(this ChatClient client, string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;    
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    public static void PrintInfo(this ChatClient client, string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;    
        Console.WriteLine($"Info: {message}");
        Console.ResetColor();
    }

    public static void PrintConversationMessage(this ChatClient client, string message)
    {  
        var strings = message.Split(':');
        if (strings.Length == 2)
        {
            var nickname = strings[0];
            var text = strings[1];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{nickname}: ");
            Console.ResetColor();
            Console.WriteLine(text);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }
    }
}
