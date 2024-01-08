using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Questao2;
using System.Net.Http.Json;
using System.Collections;

public class Program
{

    public static void Main()
    {

        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int gols = 0;
        string Headerapi = "https://jsonmock.hackerrank.com/api/";
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Headerapi);
            //HTTP GET
            var responseTask = client.GetAsync("football_matches?year=" + year + "&team1=" + team);
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {

               var readTask = result.Content.ReadAsStringAsync();
               readTask.Wait(); 

               var json = result.Content.ReadAsStringAsync().Result;

               var jsonDeserialized = JsonConvert.DeserializeObject<Times>(json);
               var listTimes  = jsonDeserialized.data;
                foreach(var time in listTimes)
                {
                    gols = gols + Convert.ToInt32(time.team1goals);  
                }
                
                
            }
        }
        return gols;
    }

}