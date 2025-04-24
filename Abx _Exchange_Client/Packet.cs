using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abx__Exchange_Client
{
    public class Packet
    {
        public string Symbol { get; set; }
        public string Side { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Sequence { get; set; }

        public static Packet Parse(byte[] data)
        {
            var symbol = Encoding.ASCII.GetString(data, 0, 4);
            var side = Encoding.ASCII.GetString(data, 4, 1);
            var quantity = Utils.ToInt32BigEndian(data, 5);
            var price = Utils.ToInt32BigEndian(data, 9);
            var sequence = Utils.ToInt32BigEndian(data, 13);

            return new Packet
            {
                Symbol = symbol,
                Side = side,
                Quantity = quantity,
                Price = price,
                Sequence = sequence
            };
        }
    }

}
