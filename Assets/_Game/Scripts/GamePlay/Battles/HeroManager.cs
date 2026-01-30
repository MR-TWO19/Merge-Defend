using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<Character> activeHeroes = new List<Character>();

    [Header("Spawn Settings")]
    public float spawnOffset = 2f;
    public float spawnForwardRandom = 0.5f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CreateHeroById(0);
        }    
    }

    // Spawn a hero using CharacterData instance
    public Character CreateHero(CharacterInfo data)
    {
        Transform home = BattleManager.Ins.HomeHero.transform;
        Vector3 spawnPos = home.position;

        float lateral = Random.Range(-spawnOffset, spawnOffset);
        spawnPos += home.right * lateral;

        float forwardJitter = Random.Range(-spawnForwardRandom, spawnForwardRandom);
        spawnPos += home.forward * forwardJitter;

        GameObject prefab = GameConfig.Ins.GetHeroData(data.HeroId).Prefab;

        var go = GameObject.Instantiate(prefab, spawnPos, home.rotation);
        var ch = go.GetComponent<Character>();
        if (ch != null)
        {
            ch.SetUp(data);
            activeHeroes.Add(ch);
        }
        return ch;
    }

    // Spawn by hero id (looks up in GameConfig.HeroDatas)
    public Character CreateHeroById(int id)
    {
        var data = UserSaveData.Ins.GetCharacter(id);
        return CreateHero(data);
    }

    public void RemoveHero(Character hero)
    {
        if (hero == null) return;
        if (activeHeroes.Contains(hero)) activeHeroes.Remove(hero);
    }
}
