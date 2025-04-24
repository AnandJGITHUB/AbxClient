// Program.cs
using Abx__Exchange_Client;
using System;

class Program
{
    private const string serverIp= "127.0.0.1";
    private const int serverPort = 3000;
    static void Main(string[] args)
    {
        var client = new AbxClient(serverIp, serverPort);
        client.Run();
    }
}
