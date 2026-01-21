using System.Collections;
using Hawky.Scene;
using KeatonCore.Tutorial;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : FTUEController
{
    [BoxGroup("FTUE Settings"), SerializeField] private FTUETextGroup _textGroup;
    [BoxGroup("FTUE Settings"), SerializeField] private FTUEIndicatorUI _arrowUI;
    [BoxGroup("FTUE Settings"), SerializeField] private FTUEDarkScreen _drarkScreen;
    [BoxGroup("FTUE Settings"), SerializeField] private FTUERenderTool _renderTool;
    [BoxGroup("FTUE Settings"), SerializeField] private FTUESpotlightZoom _spotlightZoom;
    [BoxGroup("FTUE Settings"), SerializeField] private Canvas canvas2;
    [SerializeField] private Button btnNextStep;

    private bool isPaused = false;
    private TutorialBase curTutorial;

    public FTUETextGroup TextGroup => _textGroup;
    public FTUEIndicatorUI ArrowUI => _arrowUI;
    public FTUESpotlightZoom SpotlightZoom => _spotlightZoom;
    public Canvas Canvas => canvas2;
    public Button BtnNextStep => btnNextStep;
    public FTUEDarkScreen DarkScreen => _drarkScreen;
    public FTUERenderTool RenderTool => _renderTool;
    public bool IsPaused() => isPaused;
    public void Resume() => isPaused = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        btnNextStep.onClick.AddListener(() =>
        {
            _textGroup.Hide();
            _arrowUI.Hide();
            _drarkScreen.Hide();
            _spotlightZoom.Hide();
            btnNextStep.gameObject.SetActive(false);
            NextStep();
        });

        TutorialManager.Instance.TutorialPopup = this;
    }

    public override string SceneName() => SceneId.TUTORIAL_POPUP;

    private void CreateTutorial()
    {
        switch (TutorialManager.Instance.CurrTutorial)
        {
            case Tutorial.GamePlayIntroduction:
                curTutorial = new GamePlayIntroductionTutorial(this);
                break;
            default:
                curTutorial = null;
                break;
        }
    }

    protected override void OnShown()
    {
        base.OnShown();
        StartFTUE();
    }

    protected override IEnumerator OnStep(int stepIndex)
    {
        if (curTutorial == null)
        {
            EndFTUE();
            yield break;
        }

        yield return curTutorial.OnStep(stepIndex);
    }

    protected override void OnEnd()
    {
        _textGroup.Hide();
        _arrowUI.Hide();
        _drarkScreen.Hide();
        _spotlightZoom.Hide();
        Hide();
        CompleteTutorial();
    }

    private void CompleteTutorial()
    {
        int idx = (int)UserSaveData.Ins.Tutorial;
        idx++;
        UserSaveData.Ins.Tutorial = (Tutorial)idx;
    }

    protected override void OnStart()
    {
        _textGroup.Hide();
        _arrowUI.Hide();
        _drarkScreen.Hide();
        _spotlightZoom.Hide();
        btnNextStep.gameObject.SetActive(false);
        CreateTutorial();
    }

    public Vector2 WorldToCanvasAnchor(Vector3 worldPos)
    {
        RectTransform canvasRT = canvas2.GetComponent<RectTransform>();

        // Convert world → screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Convert screen → local position trong canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT,
            screenPos,
            canvas2.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas2.worldCamera,
            out Vector2 anchoredPos
        );

        return anchoredPos;
    }
}
