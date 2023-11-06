using System;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View
{
    public class CollectionImportantSignView : MonoBehaviour
    {
        private IFarmModel _farmModel;

        [Inject]
        private void Constructor(IFarmModel farmModel)
        {
            _farmModel = farmModel;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            _farmModel.CollectionUpdated += i => { gameObject.SetActive(true); };
        }
    }
}