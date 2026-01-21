using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine.Scripting;


[Preserve]
public class UserSaveData : BaseUserData
{
    public static UserSaveData Ins => LocalData.Get<UserSaveData>();

    public int Level;
    public bool OnSound;
    public bool OnMusic;
    public bool OnHaptic;

    public Tutorial Tutorial;

    public void SaveTutorial(Tutorial tutorial)
    {
        Tutorial = tutorial;
        Save();
    }

    protected internal override void OnInit()
    {
        base.OnInit();
        Level = 1;
        OnSound = true;
        OnMusic = true;
        OnHaptic = true;
    }

    public void NextLevel()
    {
        Level++;
        Save();
    }
    
    public void SetLevel(int level)
    {
        Level = level;
        Save();
    }

    public void SetOnSound(bool isOn)
    {
        OnSound = isOn;
        Save();
    }

    public void SetOnMusic(bool isOn)
    {
        OnMusic = isOn;
        Save();
    }

    public void SetOnHaptic(bool isOn)
    {
        OnHaptic = isOn;
        Save();
    }
}
