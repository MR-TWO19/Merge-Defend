using DG.Tweening;
using UnityEngine;

namespace KeatonCore.Tutorial
{
    public class FTUEDarkScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup darkScreenCG;

        public void Show(float duration = 0.25f)
        {
            darkScreenCG.DOKill();
            darkScreenCG.blocksRaycasts = true;
            darkScreenCG.alpha = 0;
            darkScreenCG.DOFade(1f, duration);
        }

        public void Hide(float duration = 0.25f)
        {
            darkScreenCG.DOKill();
            darkScreenCG.DOFade(0f, duration)
                .OnComplete(() => darkScreenCG.blocksRaycasts = false);
        }

        public void FadeTo(float alpha, float duration)
        {
            darkScreenCG.DOKill();
            darkScreenCG.blocksRaycasts = alpha > 0;
            darkScreenCG.DOFade(alpha, duration);
        }

        public void SetAlphaImmediately(float value)
        {
            darkScreenCG.DOKill();
            darkScreenCG.alpha = value;
            darkScreenCG.blocksRaycasts = value > 0f;
        }

        public void SetRaycast(bool active)
        {
            darkScreenCG.blocksRaycasts = active;
        }
    }
}
