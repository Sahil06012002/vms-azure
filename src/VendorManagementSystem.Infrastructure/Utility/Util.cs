using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Infrastructure.Utility
{
    public static class Util
    {
        public static string GenerateIdentifier(string prefix, int id,int length)
        {
            string numericPart = id.ToString().PadLeft(length,'0'); //convert id to 9 digit
            return prefix + numericPart;
        }
    }
}
