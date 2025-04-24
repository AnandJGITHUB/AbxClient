using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abx__Exchange_Client
{
    public static class Utils
    {
        public static int ToInt32BigEndian(byte[] data, int offset)
        {
            return (data[offset] << 24) | (data[offset + 1] << 16) |
                   (data[offset + 2] << 8) | data[offset + 3];
        }
    }
}
