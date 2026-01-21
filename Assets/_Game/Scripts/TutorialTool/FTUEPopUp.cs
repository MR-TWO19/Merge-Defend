using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace KeatonCore.Tutorial
{
    public class FTUEPopUp : MonoBehaviour
    {
        public event Action OnGotIt = delegate { };

        [Header("References")]
        [SerializeField] private Button gotItBtn;
        [SerializeField] private Transform panel;
        [SerializeField] private TextMeshProUGUI titleTxt, descriptionTxt;
        [SerializeField] private Image icon, bg;

        public Image Icon => icon;

        private Sequence jiggleSequence;

        private void OnEnable()
        {
            gotItBtn.onClick.AddListener(OnClickedButtonGotIt);
        }

        private void OnDisable()
        {
            gotItBtn.onClick.RemoveListener(OnClickedButtonGotIt);
            KillTween();
        }

        public void SetText(string text)
        {
            descriptionTxt.text = text;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            gotItBtn.interactable = true;
            panel.localScale = Vector3.zero;
            panel.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

            // Jiggle button
            jiggleSequence = DOTween.Sequence();
            jiggleSequence.Append(gotItBtn.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f, 3, 1));
            jiggleSequence.AppendInterval(1.5f);
            jiggleSequence.SetLoops(-1);
        }

        public void Hide()
        {
            gotItBtn.interactable = false;
            panel.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                KillTween();
                OnGotIt = delegate { }; // Reset the event to prevent multiple invocations
                gameObject.SetActive(false);
            });
        }

        public void OnClickedButtonGotIt()
        {
            OnGotIt?.Invoke();
            gotItBtn.interactable = false;
        }

        public void KillTween()
        {
            if (jiggleSequence != null && jiggleSequence.IsActive())
            {
                jiggleSequence.Kill();
                jiggleSequence = null;
            }

            panel.DOKill();
        }
    }
}