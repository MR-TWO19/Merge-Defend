using Hawky;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : MonoSingleton<CheatManager>
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button btnGo;
    [SerializeField] private Button btnNextLevel;
    [SerializeField] private Button btnCheat;
    [SerializeField] private GameObject frameUI;

    private void Start()
    {
        if (GameConfig.Ins != null)
            gameObject.SetActive(GameConfig.Ins.IsCheatMode);

        if (btnNextLevel != null) btnNextLevel.onClick.AddListener(NextLevel);
        if (btnGo != null) btnGo.onClick.AddListener(GoLevel);
        if (btnCheat != null) btnCheat.onClick.AddListener(UpdateUICheat);
    }

    private void NextLevel()
    {
        if (GameManager.Ins != null)
            GameManager.Ins.NextLevel();
    }

    private void GoLevel()
    {
        if (GameManager.Ins == null) return;
        if (inputField == null) return;

        if (int.TryParse(inputField.text, out var level))
            GameManager.Ins.GoLevel(level);
    }

    private void UpdateUICheat()
    {
        if (frameUI == null) return;
        frameUI.SetActive(!frameUI.activeSelf);
    }
}
