// Program.cs
using Abx__Exchange_Client;
using System;

class Program
{
    static void Main(string[] args)
    {
        var client = new AbxClient("127.0.0.1", 3000);
        client.Run();
    }
}
