using Microsoft.Extensions.Logging;
using Playnite;
using Playnite.SDK;
using Playnite.SDK.Models;

namespace PlayniteCore
{
    public class PlayniteAPI : IPlayniteAPI
    {
        public PlayniteAPI(
            IGameDatabaseAPI database,
            IMainViewAPI mainView,
            IPlayniteInfoAPI info,
            IPlaynitePathsAPI paths,
            IResourceProvider resources,
            INotificationsAPI notifications)
        {
            Paths = paths;
            ApplicationInfo = info;
            MainView = mainView;
            Database = database;
            Resources = resources;
            Notifications = notifications;
            Logger = new LoggerFactory().CreateLogger<PlayniteAPI>();
        }

        public IGameDatabaseAPI Database { get; }

        public IMainViewAPI MainView { get; set; }

        public IPlaynitePathsAPI Paths { get; }

        public IPlayniteInfoAPI ApplicationInfo { get; }

        public IResourceProvider Resources { get; }

        public INotificationsAPI Notifications { get; }

        public ILogger<PlayniteAPI> Logger { get; }

        public string ExpandGameVariables(Game game, string inputString)
        {
            return game?.ExpandVariables(inputString);
        }

        public GameAction ExpandGameVariables(Game game, GameAction action)
        {
            return action?.ExpandVariables(game);
        }
    }
}
