Console.Write("Choose your nickname: ");
string nickname = Console.ReadLine() ?? throw new Exception();

ChatClient client = new(nickname);

await client.ConnectAsync("address", 55555);
