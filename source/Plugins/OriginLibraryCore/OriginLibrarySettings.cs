﻿using Newtonsoft.Json;
using OriginLibrary.Services;
using Playnite;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginLibrary
{
    public class OriginLibrarySettings : ObservableObject, ISettings
    {
        private static ILogger logger = LogManager.GetLogger();
        private OriginLibrarySettings editingClone;
        private OriginLibrary library;
        private IPlayniteAPI api;

        #region Settings      

        public bool ImportInstalledGames { get; set; } = true;

        public bool ImportUninstalledGames { get; set; } = false;

        #endregion Settings

        [JsonIgnore]
        public bool IsUserLoggedIn
        {
            get => false;
            //{
            //    using (var view = api.WebViews.CreateOffscreenView())
            //    {
            //        var api = new OriginAccountClient(view);
            //        return api.GetIsUserLoggedIn();
            //    }
            //}
        }

        [JsonIgnore]
        public RelayCommand<object> LoginCommand
        {
            get => new RelayCommand<object>((a) =>
            {
                Login();
            });
        }

        public OriginLibrarySettings()
        {
        }

        public OriginLibrarySettings(OriginLibrary library, IPlayniteAPI api)
        {
            this.library = library;
            this.api = api;

            var settings = library.LoadPluginSettings<OriginLibrarySettings>();
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

        private void LoadValues(OriginLibrarySettings source)
        {
            source.CopyProperties(this, false, null, true);
        }

        private void Login()
        {
            //try
            //{
            //    using (var view = api.WebViews.CreateView(490, 670))
            //    {
            //        var api = new OriginAccountClient(view);
            //        api.Login();
            //    }

            //    OnPropertyChanged(nameof(IsUserLoggedIn));
            //}
            //catch (Exception e) when (!Environment.IsDebugBuild)
            //{
            //    logger.Error(e, "Failed to authenticate user.");
            //}
            throw new NotImplementedException("We are not using Login for the moment, and WebViews are not available without WPF");

        }
    }
}
