using RiotLibrary.Models;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotLibrary
{
    public class RiotGames
    {
        public static readonly List<RiotApp> Games = new List<RiotApp>()
        {
            new RiotApp()
            {
                ProductId = "LoL",
                Name = "League of Legends",
                ExePath = "LeagueClient.exe"
            }
        };

        public static RiotApp GetAppDefinition(string productId)
        {
            return Games.FirstOrDefault(a => a.ProductId == productId);
        }
    }
}
