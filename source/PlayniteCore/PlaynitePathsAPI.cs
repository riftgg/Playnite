using Playnite.SDK;

namespace PlayniteCore
{
    public class PlaynitePathsAPI : IPlaynitePathsAPI
    {
        public bool IsPortable => true;

        public string ApplicationPath => ".";

        public string ConfigurationPath => ".";

        public string ExtensionsDataPath => "Extensions";
    }
}
