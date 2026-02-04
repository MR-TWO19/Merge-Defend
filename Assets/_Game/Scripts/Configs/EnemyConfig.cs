using TwoCore;
using UnityEngine;

[System.Serializable]
public class CharacterData : BaseData
{
    public EnemyType Type;
    public float Health = 0;
    public float Speed = 0;
    public int Damage = 0;
    public GameObject Prefab;
}

[System.Serializable]
public class CharacterInfo
{
    public int CharId;
    public int Level;
    public float Health = 0;
    public float Speed = 0;
    public int Damage = 0;
}