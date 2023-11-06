using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Config
{
    [CreateAssetMenu(fileName = "ShopSheep", menuName = "Configs/ShopSheep", order = 0)]
    public class ShopSheepConfig : ScriptableObject
    {
        public List<ItemById> Items;
        
        [Serializable]
        public struct ItemById
        {
            public string Id;
            public int Cost;
        }
    }
}