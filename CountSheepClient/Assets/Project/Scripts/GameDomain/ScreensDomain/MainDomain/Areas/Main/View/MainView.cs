using Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.View;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.View
{
    public class MainView : MonoBehaviour, IMainView
    {
        [SerializeField] private GameOverView _gameOver;
        [SerializeField] private GameObject _tutorialHand;
        [SerializeField] private ScoreInBattleView _scoreInBattleView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private GameObject _mergeUi;
        [SerializeField] private GameObject _battleUi;
        [SerializeField] private GameObject _mergeButton;
        [SerializeField] private CollectionView _collectionView;
        [SerializeField] private MarketView _marketView;

        public IGameOverView GameOver => _gameOver;
        public IScoreInBattleView ScoreInBattle => _scoreInBattleView;
        public IPauseView Pause => _pauseView;
        public ICollectionView Collection => _collectionView;
        public IMarketView Market => _marketView;

        public void SetActiveTutorialHand(bool isActive)
        {
            _tutorialHand.SetActive(isActive);
        }

        public void ShowBattle()
        {
            _battleUi.gameObject.SetActive(true);
            _mergeUi.gameObject.SetActive(false);
        }

        public void ShowMerge()
        {
            _mergeUi.gameObject.SetActive(true);
            _battleUi.gameObject.SetActive(false);
        }

        public void SetVisibleToMergeButton(bool isVisible)
        {
            _mergeButton.SetActive(isVisible);
        }
    }
}