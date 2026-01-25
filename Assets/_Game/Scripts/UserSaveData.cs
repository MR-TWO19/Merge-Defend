using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine.Scripting;


[Preserve]
public class UserSaveData : BaseUserData
{
    public static UserSaveData Ins => LocalData.Get<UserSaveData>();

    public int Level;
    public bool MusicOn;
    public bool SfxOn;

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
        SfxOn = true;
        MusicOn = true;
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
}
