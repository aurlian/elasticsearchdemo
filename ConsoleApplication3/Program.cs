using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test md hashing");
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = new byte[10] { 123, 2, 100, 65, 252, 3, 4, 55, 67, 98 };

                Console.WriteLine("original bytes");
                StringBuilder builder = new StringBuilder();
                for(int j = 0; j < bytes.Length; j++)
                {
                    Console.WriteLine(bytes[j]);
                    builder.Append(bytes[j].ToString("x2"));
                }

                Console.WriteLine("string representation");
                var converted = Encoding.ASCII.GetString(bytes);
                Console.WriteLine(converted);
                Console.WriteLine(builder.ToString());
                Console.WriteLine("back to byte");

                var reverted = Encoding.ASCII.GetBytes(converted);
                for (int k = 0; k < reverted.Length; k++)
                {
                    Console.WriteLine(bytes[k]);
                }

                var result = md5Hash.ComputeHash(bytes);

                Console.WriteLine("resulting bytes");

                for(int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine(result[i]);
                }
                Console.WriteLine("string representation");
                Console.WriteLine(Encoding.ASCII.GetString(result));



            }


        }
    }
}
