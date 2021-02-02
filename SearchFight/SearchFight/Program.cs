using BE;
using ClientCall;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchFight
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("No ha digitado ninguna palabra de busqueda");
                Console.ReadLine();
                return;
            }

               
            string[] engines = ConfigurationManager.AppSettings["engines"].Split(',');

            long[,] matrix = new long[args.Length,engines.Length];
            int x =0;
            int y = 0;
            foreach (var word in args)
            {

                Console.Write("\n"+word + ": ");
                y = 0;
                foreach (var eng in engines)
                {
                    Console.Write(eng + ": ");
                    string[] parameters = ConfigurationManager.AppSettings[eng].Split(',');

                    string url = ConfigurationManager.AppSettings[eng + "_" + parameters.Where(x => x == "URL").FirstOrDefault()];
                    string method = ConfigurationManager.AppSettings[eng + "_" + parameters.Where(x => x == "METHOD").FirstOrDefault()];
                    string API_KEY = ConfigurationManager.AppSettings[eng + "_" + parameters.Where(x => x == "API_KEY").FirstOrDefault()];
                    string HEADERAUTH = ConfigurationManager.AppSettings[eng + "_" + parameters.Where(x => x == "HEADERAUTH").FirstOrDefault()];
                    string HEADERVALUE = ConfigurationManager.AppSettings[eng + "_" + parameters.Where(x => x == "HEADERAUTH_VALUE").FirstOrDefault()];

                    if (API_KEY != null)
                        url = url.Replace("{API_KEY}", API_KEY);
                    if (word != null)
                        url = url.Replace("{search_value}", Uri.EscapeDataString(word));


                    var busqueda = HttpClient.GET(url, HEADERAUTH, HEADERVALUE);

                    int result = 0;
                    switch (eng)
                    {
                        case "google":
                            var obj = JsonConvert.DeserializeObject<GoogleResponse>(busqueda);
                            matrix[x, y] = obj.queries.request[0].totalResults;
                            break;
                        case "bing":
                            var obj2 = JsonConvert.DeserializeObject<BingResponse>(busqueda);
                            matrix[x, y] = obj2.webPages.totalEstimatedMatches;
                            break;

                    }
                    Console.Write(matrix [x,y] + " ");
                    y++;

                }
                x++;
            }

            long totalNum=0; string totalName = "";
            for (int i = 0; i < engines.Length; i++)
            {
                Console.Write("\n" + engines[i] + " winner: ");
                long localNum = 0; string localName = "";
                for (int j = 0; j < args.Length; j++)
                {
                    if (matrix[j, i] > localNum)
                    {
                        localNum = matrix[j, i];

                        localName = args[j];
                    }
                    if (matrix[j, i] > totalNum)
                    {
                        totalNum = matrix[j, i];

                        totalName = args[j];
                    }
                    Console.Write( "");
                }
                Console.Write(localName);

            }
            Console.WriteLine("\nTotal Winner: " + totalName);

        }
    }
}
