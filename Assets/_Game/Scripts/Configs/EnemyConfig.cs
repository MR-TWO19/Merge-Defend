using TwoCore;
using UnityEngine;

[System.Serializable]
public class CharacterData : BaseData
{
    public EnemyType Type;
    public float Health = 10f;
    public float Speed = 2f;
    public int Damage = 1;
    public GameObject Prefab;
}

[System.Serializable]
public class CharacterInfo
{
    public int HeroId;
    public float Health = 10f;
    public float Speed = 2f;
    public int Damage = 1;
}