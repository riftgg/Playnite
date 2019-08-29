using BattleNetLibrary.Models;
using Playnite.Common;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetLibrary.Services
{
    public class BattleNetAccountClient
    {
        private const string apiStatusUrl = @"https://account.blizzard.com/api/";
        private const string gamesUrl = @"https://account.blizzard.com/api/games-and-subs";
        private const string classicGamesUrl = @"https://account.blizzard.com/api/classic-games";
        private static ILogger logger = LogManager.GetLogger();
        //private IWebView webView;

        //public BattleNetAccountClient(IWebView webView)
        //{
        //    this.webView = webView;
        //}

        public async Task<List<GamesAndSubs.GameAccount>> GetOwnedGames()
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{gamesUrl}");
            var response = await httpClient.SendAsync(request);
            var textGames = await response.Content.ReadAsStringAsync();

            var games = Serialization.FromJson<GamesAndSubs>(textGames);
            return games.gameAccounts;
        }

        public async Task<List<ClassicGames.ClassicGame>> GetOwnedClassicGames()
        {

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{classicGamesUrl}");
            var response = await httpClient.SendAsync(request);
            var textGames = await response.Content.ReadAsStringAsync();
            var games = Serialization.FromJson<ClassicGames>(textGames);
            return games.classicGames;
        }

        public void Login()
        {
            //var apiUrls = GetDefaultApiStatus();
            //webView.NavigationChanged += (s, e) =>
            //{
            //    var address = webView.GetCurrentAddress();
            //    logger.Debug($"Battlenet login navigation {address}");
            //    if (address.Equals(@"https://account.blizzard.com/overview", StringComparison.OrdinalIgnoreCase))
            //    {
            //        webView.Close();
            //    }
            //};

            //webView.Navigate(apiUrls.logoutUri);
            //webView.OpenDialog();
        }

        public bool GetIsUserLoggedIn()
        {
            return false;
        }

        public static BattleNetApiStatus GetDefaultApiStatus()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadData(apiStatusUrl);
                }
                catch (WebException exception) // Response is always 401
                {
                    string responseText = string.Empty;
                    var responseStream = exception.Response?.GetResponseStream();
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }

                    logger.Debug(responseText);
                    var deserialized = Serialization.FromJson<BattleNetApiStatus>(responseText);
                    return deserialized;
                }

                return null;
            }
        }

        public async Task<BattleNetApiStatus> GetApiStatus()
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiStatusUrl}");
            var response = await httpClient.SendAsync(request);
            var textStatus = await response.Content.ReadAsStringAsync();
            return Serialization.FromJson<BattleNetApiStatus>(textStatus);
        }
    }
}
