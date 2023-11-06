using System;
using System.Collections.Generic;
using System.Linq;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GameDomain.ScreensDomain.MainDomain.Areas.Debug
{
    public class DebugView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        private IFarmModel _farmModel;
        private IDataStorageService _dataStorageService;
        private ICurrencyModel _currencyModel;
        private IFarmConfigKeeper _farmConfigKeeper;

        [Inject]
        private void Constructor(
            IFarmModel farmModel,
            IDataStorageService dataStorageService,
            ICurrencyModel currencyModel,
            IFarmConfigKeeper farmConfigKeeper
        )
        {
            _farmModel = farmModel;
            _dataStorageService = dataStorageService;
            _currencyModel = currencyModel;
            _farmConfigKeeper = farmConfigKeeper;
        }

        private void Awake()
        {
#if !UNITY_EDITOR
            Destroy(gameObject);
#endif
        }

        private void Start()
        {
            InstantiateButton(ResetFarm, "reset farm");
            InstantiateButton(Insert10SheepToCollection, "add 10 sheep");
            InstantiateButton(Insert10PinkSheepToCollection, "add 10 pink sheep");
            InstantiateButton(Insert10BrownSheepToCollection, "add 10 brown sheep");
            InstantiateButton(MergeAll, "merge all");
            InstantiateButton(ResetCoins, "reset coins");
            InstantiateButton(Add1MillionCoins, "million coins");
            InstantiateButton(OpenAllSheep, "Open all sheep");
            InstantiateButton(ResetData, "RESET DATA");
        }

        private void InstantiateButton(Action action, string buttonName)
        {
            var button = new GameObject(buttonName+"Button");
            var text = new GameObject(buttonName + "Text");
            text.transform.SetParent(button.transform);
            button.transform.SetParent(_content);

            button.AddComponent<CanvasRenderer>();
            var image = button.AddComponent<Image>();
            image.rectTransform.sizeDelta = new Vector2(500, 100);
            var buttonComponent = button.AddComponent<Button>();
            buttonComponent.onClick.AddListener(action.Invoke);
            buttonComponent.image = image;

            text.AddComponent<CanvasRenderer>();
            var textComponent = text.AddComponent<TextMeshProUGUI>();
            textComponent.text = buttonName;
            textComponent.rectTransform.sizeDelta = new Vector2(500, 100);
            textComponent.color = Color.black;
            textComponent.alignment = TextAlignmentOptions.Center;
        }

        private void ResetFarm()
        {
            var animals = _farmModel.Animals.Keys.ToList();

            foreach (var animal in animals)
            {
                _farmModel.Remove(animal);
            }

            _farmModel.ResetCollection();

            _dataStorageService.Save();
        }

        private void Insert10SheepToCollection()
        {
            for (int i = 0; i < 10; i++)
            {
                _farmModel.Add("Sheep");
            }
        }

        private void Insert10PinkSheepToCollection()
        {
            for (int i = 0; i < 10; i++)
            {
                _farmModel.Add("PinkSheep");
            }
        }

        private void Insert10BrownSheepToCollection()
        {
            for (int i = 0; i < 10; i++)
            {
                _farmModel.Add("BrownSheep");
            }
        }

        private void MergeAll()
        {
            List<int> toMerge1 = new();
            List<int> toMerge2 = new();
            bool isMerged = true;

            while (isMerged)
            {
                if (isMerged)
                {
                    toMerge1.Clear();
                    toMerge2.Clear();
                    toMerge1.AddRange(_farmModel.Animals.Keys);
                    toMerge2.AddRange(_farmModel.Animals.Keys);
                }

                isMerged = false;

                foreach (var merge1 in toMerge1)
                {
                    isMerged = _farmModel.TryMerge(merge1, toMerge2);

                    if (isMerged)
                    {
                        break;
                    }
                }
            }
        }

        private void ResetCoins()
        {
            _currencyModel.Amount = 0;
        }

        private void Add1MillionCoins()
        {
            _currencyModel.Amount += 100000000;
        }

        private void OpenAllSheep()
        {
            foreach (var animalConfig in _farmConfigKeeper.FarmConfig.Animals)
            {
                _farmModel.Add(animalConfig.Id);
            }
        }
        
        private void ResetData()
        {
            _dataStorageService.Reset();
        }
    }
}