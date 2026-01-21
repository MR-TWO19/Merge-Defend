//using Lofelt.NiceVibrations;
//using UnityEngine;

//public class HapticManager : MonoBehaviour
//{
//    public void PlayHaptic(HapticPatterns.PresetType type)
//    {
//        if (!UserSaveData.Ins.OnHaptic)
//            return;

//        HapticPatterns.PlayPreset(type); 
//    }

//    public void PlaySuccess()
//    {
//        // Nếu project có implementation rung khác thì dùng ở đây.
//        // Mặc định gọi hàm hiện có (nếu có) hoặc no-op.
//        try
//        {
//            PlayHaptic(HapticPatterns.PresetType.Success);
//        }
//        catch
//        {
//            // ignore
//        }
//    }
//}