using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class BattleManager : SingletonMono<BattleManager>
{
    public GameObject HomeEnemy;
    public GameObject HomeHero;

    public HeroManager HeroManager;
    public EnemyManager EnemyManager;


    public void StartBattle(List<Wave> waves)
    {
        EnemyManager.SetUp(waves);
    }
}
