using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Ins { get; private set; }

    private void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }

        Ins = this;
    }

    public void PlayHaptic(HapticPatterns.PresetType type)
    {
        // No-op by default (can be wired to a vibration plugin later)
    }

    public void PlaySuccess()
    {
    }
}
