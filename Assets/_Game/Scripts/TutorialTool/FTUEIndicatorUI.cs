using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace KeatonCore.Tutorial
{
    public class FTUEIndicatorUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private Vector3 orgScale;
        private Vector2 orgAnchoredPos;
        private RectTransform rectTransform;
        private Tween currentTween;
        private Coroutine moveRoutine;

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            orgScale = transform.localScale;
            orgAnchoredPos = rectTransform.anchoredPosition;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            StopAllEffects();
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            transform.localScale = orgScale;
            rectTransform.anchoredPosition = orgAnchoredPos;
        }

        #region Effects
        public void PlayPulse(float scaleMultiplier = 0.8f, float speed = 1f)
        {
            StopAllEffects();
            Show();
            currentTween = transform
                .DOScale(orgScale * scaleMultiplier, speed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void PlayBlink(float alpha = 0.3f, float speed = 1f)
        {
            StopAllEffects();
            Show();
            canvasGroup.alpha = 1;
            currentTween = canvasGroup
                .DOFade(alpha, speed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void PlayMoveBackAndForth(float distance = 30f, float speed = 1f)
        {
            StopAllEffects();
            Show();
            Vector2 dir = rectTransform.up.normalized;
            currentTween = rectTransform
                .DOAnchorPos(orgAnchoredPos + dir * distance, speed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void PlayDrag(Vector2 fromAnchoredPos, Vector2 toAnchoredPos, float duration = 1.5f, bool loopPingPong = false)
        {
            StopAllEffects();
            Show();
            moveRoutine = StartCoroutine(DragRoutine(fromAnchoredPos, toAnchoredPos, duration, loopPingPong));
        }

        public void PlayDrag(Vector2[] pathPoints, float durationPerSegment = 1.5f, bool loopPingPong = false)
        {
            if (pathPoints == null || pathPoints.Length < 2)
            {
                Debug.LogWarning("Path must contain at least 2 points.");
                return;
            }

            StopAllEffects();
            Show();
            moveRoutine = StartCoroutine(DragMultiPointRoutine(pathPoints, durationPerSegment, loopPingPong));
        }

        private void StopAllEffects()
        {
            if (currentTween != null && currentTween.IsActive()) currentTween.Kill();
            currentTween = null;

            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }

            transform.localScale = orgScale;
            rectTransform.anchoredPosition = orgAnchoredPos;
        }

        public void PointAtWorldObject(Transform target, Canvas canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform.parent,
                Camera.main.WorldToScreenPoint(target.position),
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 targetAnchoredPos
            );

            Vector2 dir = targetAnchoredPos - ((RectTransform)transform).anchoredPosition;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

            SetRotation(angle);
        }

        #endregion

        #region Position Utils
        public void SetRotation(float zRotation)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }

        public void SetAnchoredPosition(Vector2 anchoredPos)
        {
            rectTransform.anchoredPosition = anchoredPos;
            orgAnchoredPos = anchoredPos;
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

        #region Coroutines
        private IEnumerator DragRoutine(Vector2 from, Vector2 to, float duration, bool loopPingPong)
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            RectTransform rt = rectTransform;

            while (true)
            {
                rt.anchoredPosition = from;
                yield return rt.DOAnchorPos(to, duration).SetEase(Ease.InOutQuad).WaitForCompletion();
                yield return wait;

                if (loopPingPong)
                {
                    yield return rt.DOAnchorPos(from, duration).SetEase(Ease.InOutQuad).WaitForCompletion();
                    yield return wait;
                }
            }
        }

        private IEnumerator DragMultiPointRoutine(Vector2[] points, float durationPerSegment, bool loopPingPong)
        {
            RectTransform rt = rectTransform;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            int len = points.Length;

            while (true)
            {
                for (int i = 0; i < len - 1; i++)
                {
                    rt.anchoredPosition = points[i];
                    yield return rt.DOAnchorPos(points[i + 1], durationPerSegment).SetEase(Ease.InOutQuad).WaitForCompletion();
                    yield return wait;
                }

                if (!loopPingPong)
                    continue;

                for (int i = len - 1; i > 0; i--)
                {
                    rt.anchoredPosition = points[i];
                    yield return rt.DOAnchorPos(points[i - 1], durationPerSegment).SetEase(Ease.InOutQuad).WaitForCompletion();
                    yield return wait;
                }
            }
        }
        #endregion
    }
}
