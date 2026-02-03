using System;
using TwoCore;
using UnityEngine;


namespace ColorFight
{
    [Serializable]
    public class ItemData : BaseData
    {
        public GameObject Prefab;
        public Material material;
        public int HeroID;

    }

    [Serializable]
    public class LockData : BaseData
    {
        public Material materialLock;
        public Material materialKey;
    }

}
