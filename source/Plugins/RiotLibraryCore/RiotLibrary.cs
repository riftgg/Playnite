using Playnite.Common;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using RiotLibraryCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static RiotLibrary.RiotConstants;

namespace RiotLibrary
{
    public class RiotLibrary : LibraryPlugin
    {
        private ILogger logger = LogManager.GetLogger();
        private const string dbImportMessageId = "riotlibImportError";

        internal RiotLibrarySettings LibrarySettings { get; private set; }

        public RiotLibrary(IPlayniteAPI api) : base(api)
        {
            LibrarySettings = new RiotLibrarySettings(this, PlayniteApi);
        }

        public Dictionary<string, GameInfo> GetInstalledGames()
        {
            var games = new Dictionary<string, GameInfo>();
            foreach (var prog in Programs.GetUnistallProgramsList())
            {
                if (string.IsNullOrEmpty(prog.UninstallString))
                {
                    continue;
                }

                if (prog.Publisher == PUBLISHER_NAME && RiotGames.Games.Any(a => prog.DisplayName == a.Name))
                {
                    var products = RiotGames.Games.Where(a => prog.DisplayName == a.Name);
                    foreach (var product in products)
                    {
                        if (!Directory.Exists(prog.InstallLocation))
                        {
                            continue;
                        }

                        var game = new GameInfo()
                        {
                            GameId = product.ProductId,
                            Source = SOURCE_NAME,
                            Name = product.Name,
                            PlayAction = new GameAction()
                            {
                                Type = GameActionType.CMD,
                                WorkingDir = ExpandableVariables.InstallationDirectory,
                                Path = Path.Combine(prog.InstallLocation, product.ExePath),
                                IsHandledByPlugin = true
                            },
                            InstallDirectory = prog.InstallLocation,
                            IsInstalled = true
                        };

                        // Check in case there are more versions of single games installed.
                        if (!games.TryGetValue(game.GameId, out var _))
                        {
                            games.Add(game.GameId, game);
                        }
                    }
                }
            }

            return games;
        }

        #region ILibraryPlugin

        public override string LibraryIcon => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\rioticon.png");

        public override string Name => SOURCE_NAME;

        public override Guid Id => Guid.Parse("611619F1-A801-4A7D-900D-054959CB317B");

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return LibrarySettings;
        }

        public override IEnumerable<GameInfo> GetGames()
        {
            var allGames = new List<GameInfo>();
            var installedGames = new Dictionary<string, GameInfo>();
            Exception importError = null;

            if (LibrarySettings.ImportInstalledGames)
            {
                try
                {
                    installedGames = GetInstalledGames();
                    logger.Debug($"Found {installedGames.Count} installed Riot games.");
                    allGames.AddRange(installedGames.Values.ToList());
                }
                catch (Exception e)
                {
                    logger.Error(e, "Failed to import installed Riot games.");
                    importError = e;
                }
            }

            return allGames;
        }

        public override IGameController GetGameController(Game game)
        {
            return new RiotGameController(game);
        }

        #endregion ILibraryPlugin
    }
}
