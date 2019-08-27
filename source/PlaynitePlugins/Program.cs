using BattleNetLibrary;
using EpicLibrary;
using Moq;
using OriginLibrary;
using Playnite.SDK;
using Playnite.SDK.Models;
using PlayniteCore;
using SteamLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PlaynitePlugins
{
    class Program
    {
        static void Main(string[] args)
        {
            GameLibrary gameLibrary = new GameLibrary();

            var games = gameLibrary.GetGames();
            Console.WriteLine("Battle.Net");
            foreach (var item in games.Where(g => g.Source == "Battle.net"))
            {
                Console.WriteLine("\t" + item.Name + " " + item.InstallDirectory);
            }
            Console.WriteLine("Epic");
            foreach (var item in games.Where(g => g.Source == "Epic"))
            {
                Console.WriteLine("\t" + item.Name + " " + item.InstallDirectory);
            }
            Console.WriteLine("Origin");
            foreach (var item in games.Where(g => g.Source == "Origin"))
            {
                Console.WriteLine("\t" + item.Name + " " + item.InstallDirectory);
            }
            Console.WriteLine("Steam");
            foreach (var item in games.Where(g => g.Source == "Steam"))
            {
                Console.WriteLine("\t" + item.Name + " " + item.InstallDirectory);
            }

            string selected = null;
            int index = -1;
            do
            {
                selected = Console.ReadLine();
            } while (!int.TryParse(selected, out index));

            if (index >= 0 && index < games.Count())
            {
                gameLibrary.PlayGame(games.ElementAt(index));
            }
        }
    }
}
