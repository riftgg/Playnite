using BattleNetLibrary.Services;
using Newtonsoft.Json;
using Playnite;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetLibrary
{
    public class BattleNetLibrarySettings : ObservableObject, ISettings
    {
        private static ILogger logger = LogManager.GetLogger();
        private BattleNetLibrarySettings editingClone;
        private BattleNetLibrary library;
        private IPlayniteAPI api;

        #region Settings      

        public bool ImportInstalledGames { get; set; } = true;

        public bool ImportUninstalledGames { get; set; } = false;

        #endregion Settings

        [JsonIgnore]
        public bool IsUserLoggedIn
        {
            get
            {
                return false;
            }
        }

        public BattleNetLibrarySettings()
        {
        }

        public BattleNetLibrarySettings(BattleNetLibrary library, IPlayniteAPI api)
        {
            this.library = library;
            this.api = api;

            var settings = library.LoadPluginSettings<BattleNetLibrarySettings>();
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

        private void LoadValues(BattleNetLibrarySettings source)
        {
            source.CopyProperties(this, false, null, true);
        }

        private void Login()
        {

        }
    }
}
