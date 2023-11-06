using System.Collections.Generic;
using Project.GameDomain.ScreensDomain.BattleDomain;
using Project.GameDomain.ScreensDomain.LoadingDomain;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.SplashDomain;

namespace Project.GameDomain.ScreensDomain
{
    public static class ScreensIds
    {
        public static List<string> Ids => new()
        {
            SplashScreen.Id,
            LoadingScreen.Id,
            MainScreen.Id,
            BattleScreen.Id
        };
    }}