using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Linq;

namespace Abx__Exchange_Client
{
   

    public class AbxClient
    {
        private readonly string hostIP;
        private readonly int port;
        private readonly Dictionary<int, Packet> packets = new();
        private const string fileName = "packets.json";
        public AbxClient(string host, int port)
        {
            hostIP = host;
            this.port = port;
        }

        public void Run()
        {
            if (ReceiveAllPackets())
            {
                var missing = GetMissingSequences();
                foreach (var seq in missing)
                {
                    var packet = RequestMissingPacket(seq);
                    if (packet != null) packets[packet.Sequence] = packet;
                }

                var ordered = packets.Values.OrderBy(p => p.Sequence).ToList();
                var json = JsonSerializer.Serialize(ordered, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fileName, json);

                Console.WriteLine("Done! JSON saved as packets.json");

            }

            
            Console.ReadLine();
        }

        private bool ReceiveAllPackets()
        {
            try
            {

                using var client = new TcpClient(hostIP, port);
                using var stream = client.GetStream();

                stream.Write(new byte[] { 0x01, 0x00 }); // Stream all packets

                byte[] buffer = new byte[17];
                while (stream.Read(buffer, 0, 17) == 17)
                {
                    var packet = Packet.Parse(buffer);
                    packets[packet.Sequence] = packet;
               
                }
                client.Close();
                return true;
            }catch(SocketException ex)
            {
                Console.WriteLine($"Please check IP port and Make sure TCP Server is running {ex.Message}");
                //throw ex;
                return false;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                  return false;
            }
            
        }

        private List<int> GetMissingSequences()
        {
            var sequences = packets.Keys.OrderBy(k => k).ToList();//order by sequence id
            var missing = new List<int>();
            for (int i = sequences[0]; i < sequences[sequences.Count-1]; i++)
            {
                if (!packets.ContainsKey(i)) missing.Add(i);
            }
            return missing;
        }

        private Packet? RequestMissingPacket(int sequence)
        {
            try
            {
                using var client = new TcpClient(hostIP, port);
                using var stream = client.GetStream();

                stream.Write(new byte[] { 0x02, (byte)sequence });
                byte[] buffer = new byte[17];
                if (stream.Read(buffer, 0, 17) == 17)
                {
                    return Packet.Parse(buffer);
                }
                client.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Please check IP port and Make sure TCP Server is running {ex.Message}");
                //throw ex;
                //return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                //return false;
            }
            return null;
        }
    }
}
