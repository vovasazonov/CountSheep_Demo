using UnityEngine;
using UnityEngine.UI;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.View
{
    public class ScoreInBattleView : MonoBehaviour, IScoreInBattleView
    {
        [SerializeField] private Text _scoreText;

        public int Score
        {
            set => _scoreText.text = value.ToString();
        }

        public void Show()
        {
            _scoreText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _scoreText.gameObject.SetActive(false);
        }
    }
}