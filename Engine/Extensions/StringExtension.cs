using System;
using System.Security.Cryptography;
using System.Text;

namespace AdAstra.Engine.Extensions
{
    internal static class StringExtension
    {
        public static int ToSeed(this string str) => BitConverter.ToInt32(SHA256.HashData(Encoding.UTF8.GetBytes(str)), 0);
    }
}
