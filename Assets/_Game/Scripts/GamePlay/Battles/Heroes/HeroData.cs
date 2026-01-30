using UnityEngine;

[System.Serializable]
public class HeroData
{
    public HeroType Type;
    public float HP;
    public float Damage;
    public float AttackSpeed; // hits per second
    public float Range; // attack range
    public GameObject Prefab;
}
