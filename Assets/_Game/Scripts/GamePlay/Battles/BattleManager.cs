using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class BattleManager : SingletonMono<BattleManager>
{
    public HomeCharater HomeEnemy;
    public HomeCharater HomeHero;

    public HeroManager HeroManager;
    public EnemyManager EnemyManager;


    public void StartBattle(List<Wave> waves)
    {
        HomeEnemy.SetData(10);
        HomeHero.SetData(10);

        EnemyManager.SetUp(waves);
    }

    public void RemoveCharacter(Character character)
    {
        if (character.CharacterType == CharacterType.Hero)
        {
            HeroManager.RemoveHero(character);
        }
        else
        {
            EnemyManager.RemoveHero(character);
        }

    }

    public void ATKHome(CharacterType characterType)
    {
        if (characterType == CharacterType.Hero)
        {
            HomeEnemy.TakeDamage(1);
        }
        else
        {
            HomeHero.TakeDamage(1);
        }
    }
}
