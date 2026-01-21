using System;
using TwoCore;
using UnityEngine;


namespace ColorFight
{
    [Serializable]
    public class ItemData : BaseData
    {
        public GameObject Prefab;
        public Material MaterialItem;
        public Material MaterialTarget;
        public Color ColorBalloon;
    }

    [Serializable]
    public class LockData : BaseData
    {
        public Material materialLock;
        public Material materialKey;
    }

}
