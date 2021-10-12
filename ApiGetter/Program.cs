using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ApiGetter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            GetData();
            Console.ReadLine();
        }

        static async void GetData()
        {
            //Define your baseUrl
            //ring baseUrl = "http://ergast.com/api/f1/1950.json";
            //Have your using statements within a try/catch blokc that will catch any exceptions.
            try
            {
                var allSeasons = new List<RaceTable>();

                for (var year = 2020; year < 2021; year++)
                {
                    var season = new RaceTable();
                    var keepPolling = true;
                    var url = $"http://ergast.com/api/f1/{year}.json";

                    using var client = new HttpClient();
                    using var seasonResponse = await client.GetAsync(url);
                    using var seasonContent = seasonResponse.Content;

                    var seasonData = await seasonContent.ReadAsStringAsync();
                    //var data = Resource._1950Season;
                    var seasonDataModified = seasonData.Remove(0, 10);
                    var seasonDataModifiedComplete = seasonDataModified.Remove(seasonDataModified.Length - 1, 1);
                    var seasonResult = Jsonseason.DeserializeObject<MRData>(seasonDataModifiedComplete);

                    var roundsInSeason = seasonResult.Total;
                    season.Season = year;
                    Thread.Sleep(1000);
                    var round = 1;

                    do
                    {
                        var baseUrl = $"http://ergast.com/api/f1/{year}/{round}/fastest/1/results.json";

                        using var res = await client.GetAsync(baseUrl);
                        using var content = res.Content;

                        //Now assign your content to your data variable, by seasoning into a string using the await keyword.
                        var data = await content.ReadAsStringAsync();
                        //var data = Resource._1950Season;
                        var dataModified = data.Remove(0, 10);
                        var dataModifiedComplete = dataModified.Remove(dataModified.Length - 1, 1);
                        var raceResult = Jsonseason.DeserializeObject(dataModifiedComplete);
                        //var raceResult = Jsonseason.DeserializeObject<MRData>(dataModifiedComplete);

                        //season.Races.Add(raceResult.RaceTable.Races[0]);
                        Thread.Sleep(1000);
                        round++;

                        if (round > roundsInSeason)
                        {
                            keepPolling = false;
                        }

                    } while (keepPolling);
                    allSeasons.Add(season);
                    Console.WriteLine($"{year} completed");
                }

                var x = Jsonseason.SerializeObject(allSeasons);

            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }
        }
    }
}