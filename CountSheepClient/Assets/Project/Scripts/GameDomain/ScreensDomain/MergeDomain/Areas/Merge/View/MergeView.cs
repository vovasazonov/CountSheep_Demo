using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Merge.View
{
    public class MergeView : MonoBehaviour, IMergeView
    {
        [SerializeField] private Animator _mergeAnimator;
        [SerializeField] private Text _sheepNameText;
        [SerializeField] private Text _amountCollectedCoinsText;
        private IFarmModel _farmModel;

        [Inject]
        private void Constructor(IFarmModel farmModel)
        {
            _farmModel = farmModel;
        }

        private void OnEnable()
        {
            _farmModel.CollectionUpdated += OnCollectionUpdated;
        }

        private void OnDisable()
        {
            _farmModel.CollectionUpdated -= OnCollectionUpdated;
        }

        private void OnCollectionUpdated(string id)
        {
            _sheepNameText.text = _farmModel.GetName(id).ToUpper();
            _mergeAnimator.Rebind();
            _mergeAnimator.Update(0f);
            _mergeAnimator.Play("CollectionNew");
        }

        public void ShowAmountCollectedCoins(int amount)
        {
            if (amount == 0)
            {
                return;
            }
            _amountCollectedCoinsText.text = $"+{amount:n0}";
            _mergeAnimator.Rebind();
            _mergeAnimator.Update(0f);
            _mergeAnimator.Play("CollectedCoins");
        }
    }
}