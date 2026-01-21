using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace KeatonCore.Tutorial
{
    [System.Flags]
    public enum FTUETextGroupEffect
    {
        None = 0,
        Fade = 1 << 0,
        Move = 1 << 1,
        Scale = 1 << 2
    }

    public struct FTUETextHighlightOptions
    {
        public string word;
        public string color;
        public int size;
        public string font;
        public bool bold;
        public bool italic;

        public static FTUETextHighlightOptions None => new FTUETextHighlightOptions
        {
            word = null,
            color = null,
            size = 0,
            font = null,
            bold = false,
            italic = false
        };
    }

    public struct FTUETextShowOptions
    {
        public List<FTUETextHighlightOptions> highlights;

        public bool typewriter;
        public float typewriterSpeed;
        public FTUETextGroupEffect effects;
        public Vector2 moveOffset;
        public Vector3 scaleFrom;
        public float duration;
        public Ease ease;

        public static FTUETextShowOptions Default => new FTUETextShowOptions
        {
            highlights = new List<FTUETextHighlightOptions>(),
            typewriter = false,
            typewriterSpeed = 0.03f,
            effects = FTUETextGroupEffect.Fade,
            moveOffset = Vector2.zero,
            scaleFrom = Vector3.zero,
            duration = 0.5f,
            ease = Ease.OutCubic
        };
    }

    public class FTUETextGroup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private Vector3 originalPos;
        private Vector3 originalScale;

        private Tween fadeTween, moveTween, scaleTween;
        private Coroutine typewriterRoutine;

        private void Awake()
        {
            originalPos = transform.localPosition;
            originalScale = transform.localScale;
        }

        public string text
        {
            get => textMeshProUGUI.text;
            set => textMeshProUGUI.text = value;
        }

        public void Show(string content)
        {
            Show(content, FTUETextShowOptions.Default);
        }

        public void Show(string content, string highlightWord, string color = null, int size = 0)
        {
            var options = FTUETextShowOptions.Default;
            options.highlights.Add(new FTUETextHighlightOptions
            {
                word = highlightWord,
                color = color,
                size = size,
                font = null,
                bold = false,
                italic = false
            });
            Show(content, options);
        }

        public void Show(string content, bool typewriter)
        {
            var options = FTUETextShowOptions.Default;
            options.typewriter = typewriter;
            Show(content, options);
        }

        public void Show(string content, FTUETextShowOptions options)
        {
            gameObject.SetActive(true);

            if (options.highlights != null && options.highlights.Count > 0)
            {
                text = ApplyHighlights(content, options.highlights);
            }
            else
            {
                text = content;
            }

            KillTweens();
            if (typewriterRoutine != null) StopCoroutine(typewriterRoutine);

            if (options.effects.HasFlag(FTUETextGroupEffect.Fade))
            {
                canvasGroup.alpha = 0;
                fadeTween = canvasGroup.DOFade(1f, options.duration).SetEase(options.ease);
            }

            if (options.effects.HasFlag(FTUETextGroupEffect.Move))
            {
                transform.localPosition = originalPos + (Vector3)options.moveOffset;
                moveTween = transform.DOLocalMove(originalPos, options.duration).SetEase(options.ease);
            }

            if (options.effects.HasFlag(FTUETextGroupEffect.Scale))
            {
                transform.localScale = options.scaleFrom;
                scaleTween = transform.DOScale(originalScale, options.duration).SetEase(options.ease);
            }

            if (options.typewriter)
            {
                typewriterRoutine = StartCoroutine(TypewriterCoroutine(textMeshProUGUI.text, options.typewriterSpeed));
            }
        }

        public void Hide(float duration = 0.5f, Ease ease = Ease.OutCubic)
        {
            KillTweens();
            if (typewriterRoutine != null) StopCoroutine(typewriterRoutine);
            fadeTween = canvasGroup.DOFade(0f, duration).SetEase(ease)
                .OnComplete(() => gameObject.SetActive(false));
        }

        #region Position Utils
        public void SetRotation(float zRotation)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }

        public void SetAnchoredPosition(Vector2 anchoredPos)
        {
            ((RectTransform)transform).anchoredPosition = anchoredPos;
        }

        public void SetPositionFromWorld(Vector3 worldPos, Canvas canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform.parent,
                Camera.main.WorldToScreenPoint(worldPos),
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 anchoredPos
            );
            SetAnchoredPosition(anchoredPos);
        }
        #endregion

        #region Private Helpers

        private void KillTweens()
        {
            fadeTween?.Kill();
            moveTween?.Kill();
            scaleTween?.Kill();
        }

        private string ApplyHighlights(string baseText, List<FTUETextHighlightOptions> highlights)
        {
            foreach (var h in highlights)
            {
                if (string.IsNullOrEmpty(h.word) || !baseText.Contains(h.word))
                    continue;

                string format = "{0}";

                if (!string.IsNullOrEmpty(h.color))
                    format = $"<color={h.color}>{format}</color>";

                if (h.size > 0)
                    format = $"<size={h.size}>{format}</size>";

                if (!string.IsNullOrEmpty(h.font))
                    format = $"<font=\"{h.font}\">{format}</font>";

                if (h.bold)
                    format = $"<b>{format}</b>";

                if (h.italic)
                    format = $"<i>{format}</i>";

                baseText = baseText.Replace(h.word, string.Format(format, h.word));
            }

            return baseText;
        }

        private IEnumerator TypewriterCoroutine(string fullText, float speed)
        {
            textMeshProUGUI.text = "";
            int i = 0;
            while (i < fullText.Length)
            {
                if (fullText[i] == '<') // skip tag
                {
                    int closingIndex = fullText.IndexOf('>', i);
                    if (closingIndex != -1)
                    {
                        textMeshProUGUI.text += fullText.Substring(i, closingIndex - i + 1);
                        i = closingIndex + 1;
                    }
                    else
                    {
                        textMeshProUGUI.text += fullText[i];
                        i++;
                    }
                }
                else
                {
                    textMeshProUGUI.text += fullText[i];
                    i++;
                    yield return new WaitForSeconds(speed);
                }
            }
        }

        #endregion
    }
}
