using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using F1Points.Models;
using Newtonsoft.Json;

namespace F1Points
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = Resource._1950;

            var season = JsonConvert.DeserializeObject<SeasonModel>(json);

            //Get2014Points(season);
            Get1950To1960Points(season);
            GetSeasonStandings(season);
            //DisplayRaceResults(season);
            DisplayModifiedFinalDriverStandings(season);
            if (season.Season > 1957)
            {
                DisplayModifiedFinalConstructorStandings(season);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("No constructors championships before 1958");
            }

            Console.ReadLine();
        }

        private static void GetSeasonStandings(SeasonModel season)
        {
            GetDriverStandings(season);
            if (season.Season > 1957)
            {
                GetConstructorStandings(season);
            }
        }

        private static void GetDriverStandings(SeasonModel season, int? numberOfRacesToCount = null)
        {
            if (numberOfRacesToCount == null)
            {
                numberOfRacesToCount = season.Races.Count();
            }
            //Take into account best results for results prior to 1991
            var seasonDrivers = new List<DriverPointsModel>();
            GetDriversForSeason(season, seasonDrivers);
            GetDriverPoints(season, numberOfRacesToCount, seasonDrivers);
            var orderedDriverStandings = season.DriverStandings.OrderByDescending(x => x.Points);
            AddPosition(orderedDriverStandings);
            season.DriverStandings = orderedDriverStandings.ToList();
        }

        private static void GetDriverPoints(SeasonModel season, int? numberOfRacesToCount, List<DriverPointsModel> seasonDrivers)
        {
            foreach (var driverPointsModel in seasonDrivers)
            {
                var topResults = GetTopResults(numberOfRacesToCount.Value, driverPointsModel.Driver.Name, season.Races);
                driverPointsModel.TopResults = topResults;
                var driver = driverPointsModel.Driver;

                foreach (var result in driverPointsModel.TopResults)
                {
                    var currentStandingExists = season.DriverStandings.Any(c => c.Driver == result.Driver.Name);
                    if (currentStandingExists)
                    {
                        var standing = season.DriverStandings.First(c => c.Driver == driver.Name);
                        season.DriverStandings.Remove(standing);
                        standing.Points += result.Points;
                        season.DriverStandings.Add(standing);
                    }
                    else
                    {
                        var standing = new DriverSeasonStandingsModel {Driver = driver.Name, Points = result.Points};
                        season.DriverStandings.Add(standing);
                    }
                }
            }
        }

        private static void GetDriversForSeason(SeasonModel season, List<DriverPointsModel> seasonDrivers)
        {
            foreach (var race in season.Races)
            {
                var addDriver = true;
                foreach (var result in race.Results)
                {
                    var driverPointsModel = new DriverPointsModel {Driver = result.Driver};
                    foreach (var seasonDriver in seasonDrivers.Where(seasonDriver =>
                        seasonDriver.Driver.Name == result.Driver.Name))
                    {
                        addDriver = false;
                    }

                    if (addDriver)
                    {
                        seasonDrivers.Add(driverPointsModel);
                    }

                    addDriver = true;
                }
            }
        }

        private static void AddPosition(IEnumerable<DriverSeasonStandingsModel> orderedDriverStandings)
        {
            var position = 1;
            foreach (var standing in orderedDriverStandings)
            {
                standing.Position = position;
                position++;
            }
        }

        private static void GetConstructorStandings(SeasonModel season)
        {
            //Take into account best results for results prior to 1979 and all other implications :
            //https://en.wikipedia.org/wiki/List_of_Formula_One_World_Championship_points_scoring_systems

            switch (season.Season)
            {
                //case 1958:
                //    //https://en.wikipedia.org/wiki/1958_Formula_One_season#1958_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    break;

                //case 1959:
                //case 1961:
                //case 1962:
                //case 1966:
                ////https://en.wikipedia.org/wiki/1959_Formula_One_season#1959_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                ////https://en.wikipedia.org/wiki/1961_Formula_One_season#1961_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                ////https://en.wikipedia.org/wiki/1962_Formula_One_season#1962_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //https://en.wikipedia.org/wiki/1966_Formula_One_season#1966_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    break;

                //case 1960:
                //case 1963:
                //case 1964:
                //case 1965:
                //    //https://en.wikipedia.org/wiki/1960_Formula_One_season#1960_Constructors_Championship_%E2%80%93_final_standings
                //    //https://en.wikipedia.org/wiki/1963_Formula_One_season#1963_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    //https://en.wikipedia.org/wiki/1964_Formula_One_season#1964_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    //https://en.wikipedia.org/wiki/1965_Formula_One_season#1965_Constructors_Championship_final_standings
                //    break;

                //case 1967:
                //case 1969:
                //case 1971:
                //    //https://en.wikipedia.org/wiki/1967_Formula_One_season#1967_International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    //https://en.wikipedia.org/wiki/1969_Formula_One_season#1969_International_Cup_for_Formula_One_Manufacturers_-_final_standings
                //    //https://en.wikipedia.org/wiki/1971_Formula_One_season#International_Cup_for_F1_Manufacturers_-_final_standings
                //    break;

                //case 1968:
                //case 1972:
                //    //https://en.wikipedia.org/wiki/1968_Formula_One_season#1968_Constructors_Championship_final_standings
                //    //https://en.wikipedia.org/wiki/1972_Formula_One_season#International_Cup_for_F1_Manufacturers_-_final_standings
                //    break;

                //case 1970:
                //    //https://en.wikipedia.org/wiki/1970_Formula_One_season#International_Cup_for_Formula_1_Manufacturers_-_final_standings
                //    break;

                //case 1973:
                //case 1974:
                //    //https://en.wikipedia.org/wiki/1973_Formula_One_season#International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    //https://en.wikipedia.org/wiki/1974_Formula_One_season#International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    break;

                //case 1975:
                //    //https://en.wikipedia.org/wiki/1975_Formula_One_season#International_Cup_for_F1_Manufacturers_%E2%80%93_final_standings
                //    break;

                //case 1976:
                //    //https://en.wikipedia.org/wiki/1976_Formula_One_season#International_Cup_for_Formula_1_Manufacturers_%E2%80%93_final_standings
                //    break;

                //case 1977:
                //    //https://en.wikipedia.org/wiki/1977_Formula_One_season#International_Cup_for_Formula_1_Constructors_%E2%80%93_final_standings
                //    break;
                //case 1978:
                //    //https://en.wikipedia.org/wiki/1978_Formula_One_season#International_Cup_for_F1_Constructors_%E2%80%93_final_standings
                //    break;

                default:
                    GetConstructorPoints1979ToPresent(season);
                    break;



            }
            var orderedConstructorStandings = season.ConstructorStandings.OrderByDescending(x => x.Points);
            var position = 1;
            foreach (var standing in orderedConstructorStandings)
            {
                standing.Position = position;
                position++;
            }

            season.ConstructorStandings = orderedConstructorStandings.ToList();
        }

        private static void GetConstructorPoints1979ToPresent(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    var currentStandingExists =
                        season.ConstructorStandings.Any(c => c.Constructor == result.Constructor.Name);
                    if (currentStandingExists)
                    {
                        var standing =
                            season.ConstructorStandings.First(c => c.Constructor == result.Constructor.Name);
                        season.ConstructorStandings.Remove(standing);
                        if (season.Season == 1961)
                        {
                            standing.Points += (result.Points - 1);
                        }
                        else
                        {
                            standing.Points += result.Points;
                        }

                        season.ConstructorStandings.Add(standing);
                    }
                    else
                    {
                        var points = season.Season == 1961 ? result.Points - 1 : result.Points;

                        var standing = new ConstructorSeasonStandingsModel
                            {Constructor = result.Constructor.Name, Points = points};
                        season.ConstructorStandings.Add(standing);
                    }
                }
            }
        }

        private static IEnumerable<ResultModel> GetTopResults(int numberOfResults, string driverName, IEnumerable<RaceModel>seasonRaces)
        {
            var allResults = new List<ResultModel>();

            foreach (var race in seasonRaces)
            {
                allResults.AddRange(race.Results.Where(result => result.Driver.Name == driverName));
            }

            var filteredResults = allResults.Where(result => result.Finished || result.FastestLap);

            var orderedResults = filteredResults.OrderBy(x => x.Position);
            var topResults = orderedResults.Take(numberOfResults); 
            return topResults;
        }

        #region PointsSchemes
        private static void Get1950To1960Points(SeasonModel season)

        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (result.FastestLap)
                    {
                        result.Points = 1;
                    }

                    if (!result.Finished || result.Position > 5)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points += 8;
                            break;

                        case 2:
                            result.Points += 6;
                            break;
                        case 3:
                            result.Points += 4;
                            break;
                        case 4:
                            result.Points += 3;
                            break;
                        case 5:
                            result.Points += 2;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void Get1961To1990Points(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 6)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 9;
                            break;

                        case 2:
                            result.Points = 6;
                            break;
                        case 3:
                            result.Points = 4;
                            break;
                        case 4:
                            result.Points = 3;
                            break;
                        case 5:
                            result.Points = 2;
                            break;
                        case 6:
                            result.Points = 1;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void Get1991To2002Points(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 6)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 10;
                            break;

                        case 2:
                            result.Points = 6;
                            break;
                        case 3:
                            result.Points = 4;
                            break;
                        case 4:
                            result.Points = 3;
                            break;
                        case 5:
                            result.Points = 2;
                            break;
                        case 6:
                            result.Points = 1;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void Get2003To2009Points(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 8)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 10;
                            break;

                        case 2:
                            result.Points = 8;
                            break;
                        case 3:
                            result.Points = 6;
                            break;
                        case 4:
                            result.Points = 5;
                            break;
                        case 5:
                            result.Points = 4;
                            break;
                        case 6:
                            result.Points = 3;
                            break;
                        case 7:
                            result.Points = 2;
                            break;
                        case 8:
                            result.Points = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void Get2010To2018Exc2014Points(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 10)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 25;
                            break;

                        case 2:
                            result.Points = 18;
                            break;
                        case 3:
                            result.Points = 15;
                            break;
                        case 4:
                            result.Points = 12;
                            break;
                        case 5:
                            result.Points = 10;
                            break;
                        case 6:
                            result.Points = 8;
                            break;
                        case 7:
                            result.Points = 6;
                            break;
                        case 8:
                            result.Points = 4;
                            break;
                        case 9:
                            result.Points = 2;
                            break;
                        case 10:
                            result.Points = 1;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void Get2014Points(SeasonModel season)
        {
            var lastRace = season.Races.Last().RaceName;
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 10)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 25;
                            break;

                        case 2:
                            result.Points = 18;
                            break;
                        case 3:
                            result.Points = 15;
                            break;
                        case 4:
                            result.Points = 12;
                            break;
                        case 5:
                            result.Points = 10;
                            break;
                        case 6:
                            result.Points = 8;
                            break;
                        case 7:
                            result.Points = 6;
                            break;
                        case 8:
                            result.Points = 4;
                            break;
                        case 9:
                            result.Points = 2;
                            break;
                        case 10:
                            result.Points = 1;
                            break;

                        default:
                            break;
                    }

                    if (race.RaceName == lastRace)
                    {
                        result.Points *= 2;
                    }
                }
            }
        }

        private static void Get2018ToPresentPoints(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                foreach (var result in race.Results)
                {
                    if (!result.Finished || result.Position > 10)
                    {
                        continue;
                    }
                    switch (result.Position)
                    {
                        case 1:
                            result.Points = 25;
                            break;

                        case 2:
                            result.Points = 18;
                            break;
                        case 3:
                            result.Points = 15;
                            break;
                        case 4:
                            result.Points = 12;
                            break;
                        case 5:
                            result.Points = 10;
                            break;
                        case 6:
                            result.Points = 8;
                            break;
                        case 7:
                            result.Points = 6;
                            break;
                        case 8:
                            result.Points = 4;
                            break;
                        case 9:
                            result.Points = 2;
                            break;
                        case 10:
                            result.Points = 1;
                            break;

                        default:
                            break;
                    }

                    if (result.FastestLap && result.Position <= 10)
                    {
                        result.Points += 1;
                    }
                }
            }
        }

        #endregion PointsSchemes


        #region DisplayMethods
        private static void DisplayRaceResults(SeasonModel season)
        {
            foreach (var race in season.Races)
            {
                Console.WriteLine($"Modified results for {race.RaceName}");
                foreach (var result in race.Results)
                {
                    var message = $"{result.Position} | {result.Driver.Name} | {result.Constructor.Name} | {result.Points}";
                    Console.WriteLine(message);
                }
            }
            Console.Write("------------------------");
        }

        private static void DisplayModifiedFinalDriverStandings(SeasonModel season)
        {
            Console.WriteLine($"Modified driver standings for {season.Season}");
            foreach (var message in season.DriverStandings.Select(standing => $"{standing.Position} | {standing.Driver} | {standing.Points}"))
            {
                Console.WriteLine(message);
            }

            Console.Write("------------------------");
        }

        private static void DisplayModifiedFinalConstructorStandings(SeasonModel season)
        {
            Console.WriteLine($"Modified constructor standings for {season.Season}");
            foreach (var message in season.ConstructorStandings.Select(standing => $"{standing.Position} | {standing.Constructor} | {standing.Points}"))
            {
                Console.WriteLine(message);
            }
        }
        #endregion DisplayMethods
    }
}
