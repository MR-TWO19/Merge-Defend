using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimCharacter : MonoBehaviour
{
    [SerializeField] Character character;

    private void Reset()
    {
        character = GetComponentInParent<Character>();
    }

    public void ApplyATKDamage()
    {
        character.ApplyATKDamage();
    }
}
