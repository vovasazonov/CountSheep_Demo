using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.U2D.Animation;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.View
{
    public interface IAnimalView
    {
        event Action<List<int>> Contacted;
        int Id { get; set; }
        bool IsMax { set; }
        void SetSpriteLibrary(SpriteLibraryAsset spriteLibraryAsset);
        void MergeEffect();
        UniTask PlayNewCollectionEffect();
    }
}