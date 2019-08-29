using Playnite.SDK.Models;
using System.Collections.Generic;

namespace PlayniteCore
{
    public interface IGameLibrary
    {
        IEnumerable<GameInfo> GetGames();

        void PlayGame(GameInfo gameInfo);
    }
}
