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
            bool isWin = HomeEnemy.TakeDamage(1);
            if (isWin)
                GameManager.Ins.WinGame();
        }
        else
        {
            bool isLose = HomeHero.TakeDamage(1);
            if (isLose)
                GameManager.Ins.LoseGame();
        }
    }

    public void ResetData()
    {
        HomeEnemy.SetData(10);
        HomeHero.SetData(10);
        HeroManager.ResetChar();
        EnemyManager.ResetChar();
    }
}
