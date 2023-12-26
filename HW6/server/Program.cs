using server;

string host = "address";
int port = 55555;
ChatServer server = new(host, port);
await server.Start();
