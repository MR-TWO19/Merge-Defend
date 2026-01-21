using Sirenix.OdinInspector;
using UnityEngine;

public class TestSpotlight : MonoBehaviour
{
    public FTUESpotlightZoom[] spotlights;
    public Transform target;
    public Transform[] targets;
    public RectTransform target2;

    [Button("Test Focus")]
    void TestFocus()
    {
        spotlights[0].FocusOnTarget(target);
    }

    [Button("Test Focus2")]
    void TestFocus2()
    {
        spotlights[0].FocusOnTarget(target2);
    }

    [Button("Test Focus22")]
    void TestFocus22()
    {
        spotlights[0].FocusOnTarget(targets[0]);
        spotlights[1].FocusOnTarget(targets[1]);
    }
}
