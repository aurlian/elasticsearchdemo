using ConsoleApplication3.ElasticSearch;
using System;
using System.Collections.Generic;
using System.IO;
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

            var task = TestES();
            task.Wait();
            bool result = task.Result;


        }

        public static async Task<bool> TestES()
        {

            ElasticSearchRepo client = new ElasticSearchRepo();

            //client.DeleteIndexAsync("mherindex");

            //client.CreateIndexAsync("mherindex", "mher").Wait();

            //client.RemoveAlias("mher").Wait();

            //client.AddItem("mherindex", new Symptom { Name = "Red Skin", Description = "Skin color is red" }).Wait();

            //client.AddSymptoms("mherindex").Wait();

            //var result = client.Query();

            //var update = client.UpdateAsync("AVT0lr2GXfxqVSCeV7UR", "Red Eye");

            var delete = await client.DeleteAsync("AVT0i_LdXfxqVSCeV7Aj");

            return true;
        }






    }
}
