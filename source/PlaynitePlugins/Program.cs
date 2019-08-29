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
            foreach (var item in games)
            {
                Console.WriteLine(item.Source + "\t" + item.Name + " " + item.InstallDirectory);
            }

            string selected;
            int index;
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
