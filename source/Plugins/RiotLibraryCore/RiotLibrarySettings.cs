using Newtonsoft.Json;
using Playnite;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotLibrary
{
    public class RiotLibrarySettings : ObservableObject, ISettings
    {
        private static ILogger logger = LogManager.GetLogger();
        private RiotLibrarySettings editingClone;
        private RiotLibrary library;
        private IPlayniteAPI api;

        #region Settings      

        public bool ImportInstalledGames { get; set; } = true;

        public bool ImportUninstalledGames { get; set; } = false;

        #endregion Settings

        public RiotLibrarySettings()
        {
        }

        public RiotLibrarySettings(RiotLibrary library, IPlayniteAPI api)
        {
            this.library = library;
            this.api = api;

            var settings = library.LoadPluginSettings<RiotLibrarySettings>();
            if (settings != null)
            {
                LoadValues(settings);
            }
        }

        public void BeginEdit()
        {
            editingClone = this.GetClone();
        }

        public void CancelEdit()
        {
            LoadValues(editingClone);
        }

        public void EndEdit()
        {
            library.SavePluginSettings(this);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = null;
            return true;
        }

        private void LoadValues(RiotLibrarySettings source)
        {
            source.CopyProperties(this, false, null, true);
        }
    }
}
