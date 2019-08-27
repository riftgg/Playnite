using Playnite.Controllers;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlayniteCore
{
    public class GameLibrary : IGameLibrary
    {
        public GameLibrary()
        {
            Controllers = new GameControllerFactory();
            Api = new PlayniteAPI(null, null, null, paths: new PlaynitePathsAPI(), null, notifications: new NotificationsAPI());
            LoadPlugins();
        }

        IPlayniteAPI Api { get; set; }
        GameControllerFactory Controllers { get; set; }

        List<LibraryPlugin> Plugins { get; set; }

        public void LoadPlugins()
        {
            Plugins = new List<LibraryPlugin>
            {
                new RiotLibrary.RiotLibrary(Api),
                new BattleNetLibrary.BattleNetLibrary(Api),
                new EpicLibrary.EpicLibrary(Api),
                new OriginLibrary.OriginLibrary(Api),
                new SteamLibrary.SteamLibrary(Api)
            };
        }

        public IEnumerable<GameInfo> GetGames()
        {
            List<GameInfo> games = new List<GameInfo>();
            foreach (var plugin in Plugins)
            {
                var pluginGames = plugin.GetGames();
                foreach (var game in pluginGames)
                {
                    game.SourcePlugin = plugin;
                    games.Add(game);
                }
            }
            return games;
        }

        public void PlayGame(GameInfo gameInfo)
        {
            var controller = Controllers.GetGameBasedController(GameInfoToGame(gameInfo), gameInfo.SourcePlugin);
            controller.Play();
        }

        static Game GameInfoToGame(GameInfo game)
        {
            return GameInfoToGame(game, Guid.Empty);
        }

        static Game GameInfoToGame(GameInfo game, Guid pluginId)
        {
            if (pluginId == Guid.Empty)
            {
                pluginId = game.SourcePlugin.Id;
            }

            var newGame = new Game()
            {
                PluginId = pluginId,
                Name = game.Name,
                GameId = game.GameId,
                Description = game.Description,
                InstallDirectory = game.InstallDirectory,
                GameImagePath = game.GameImagePath,
                SortingName = game.SortingName,
                OtherActions = game.OtherActions == null ? null : new ObservableCollection<GameAction>(game.OtherActions),
                PlayAction = game.PlayAction,
                ReleaseDate = game.ReleaseDate,
                Links = game.Links == null ? null : new ObservableCollection<Link>(game.Links),
                IsInstalled = game.IsInstalled,
                Playtime = game.Playtime,
                PlayCount = game.PlayCount,
                LastActivity = game.LastActivity,
                Version = game.Version,
                CompletionStatus = game.CompletionStatus,
                UserScore = game.UserScore,
                CriticScore = game.CriticScore,
                CommunityScore = game.CommunityScore
            };
            return newGame;
        }

    }
}
