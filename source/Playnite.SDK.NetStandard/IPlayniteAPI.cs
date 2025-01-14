﻿using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playnite.SDK
{
    /// <summary>
    /// Describes object providing Playnite API.
    /// </summary>
    public interface IPlayniteAPI
    {
        /// <summary>
        /// Gest main view API.
        /// </summary>
        IMainViewAPI MainView { get; }

        /// <summary>
        /// Gets database API.
        /// </summary>
        IGameDatabaseAPI Database { get; }

        /// <summary>
        /// Gets paths API.
        /// </summary>
        IPlaynitePathsAPI Paths { get; }

        /// <summary>
        /// Gets notification API.
        /// </summary>
        INotificationsAPI Notifications { get; }

        /// <summary>
        /// Gets application info API.
        /// </summary>
        IPlayniteInfoAPI ApplicationInfo { get; }

        /// <summary>
        /// Gets resources API.
        /// </summary>
        IResourceProvider Resources { get; }

        /// <summary>
        /// Expands dynamic game variables in specified string.
        /// </summary>
        /// <param name="game">Game to use dynamic variables from.</param>
        /// <param name="inputString">String containing dynamic variables.</param>
        /// <returns>String with replaces variables.</returns>
        string ExpandGameVariables(Game game, string inputString);

        /// <summary>
        /// Expands dynamic game variables in specified game action.
        /// </summary>
        /// <param name="game">Game to use dynamic variables from.</param>
        /// <param name="action">Game action to expand variables to.</param>
        /// <returns>Game action with expanded variables.</returns>
        GameAction ExpandGameVariables(Game game, GameAction action);
    }
}
