using System.Net.Sockets;

namespace server;

internal record User(TcpClient Client, string NickName);