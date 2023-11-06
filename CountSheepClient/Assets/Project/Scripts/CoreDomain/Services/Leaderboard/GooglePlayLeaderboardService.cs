using System;
using Project.CoreDomain.Services.GoogplayGames;
using UnityEngine;

namespace Project.CoreDomain.Services.Leaderboard
{
    public class GooglePlayLeaderboardService : ILeaderboardService
    {
        private readonly IGoogleplayGamesService _googleplayGamesService;
        private string _GPGSID = "***SECRET***";
        private int _maxScore;

        public GooglePlayLeaderboardService(IGoogleplayGamesService googleplayGamesService)
        {
            _googleplayGamesService = googleplayGamesService;
        }

        public void Report(int score)
        {
            if (score <= _maxScore)
            {
                return;
            }

            _maxScore = score;

            if (_googleplayGamesService.IsAuthenticated)
            {
                Social.ReportScore(_maxScore, _GPGSID, _=> {});
            }
        }

        public void Show()
        {
            try
            {
                if (_googleplayGamesService.IsAuthenticated)
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
                    _googleplayGamesService.SignIn(() =>
                    {
                        Social.ReportScore(_maxScore, _GPGSID, _ =>
                        {
                            Social.ShowLeaderboardUI();
                        });
                    });
                }
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }
        }
    }
}