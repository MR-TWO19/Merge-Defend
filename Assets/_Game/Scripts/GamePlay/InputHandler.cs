using DG.Tweening;
using UnityEngine;
using UnityEngine.Timeline;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem paticale;

    private ItemControl itemControlSelect;
    private Camera mainCam;

    private bool isLocked = false;
    public bool IsLocked => isLocked;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Reset()
    {
        gameObject.name = "InputHandler";
    }

    void Update()
    {
        if (isLocked)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            TrySelectObject(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            ReleaseObject();
    }

#if UNITY_ANDROID || UNITY_IOS
    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            TrySelectObject(touch.position);

        if (touch.phase == TouchPhase.Ended)
            ReleaseObject();
    }
#endif

    void TrySelectObject(Vector3 screenPos)
    {
        MoveParticleTo(screenPos);

        Ray ray = mainCam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("ItemControl")))
        {
            ItemControl itemControl = hit.collider.GetComponentInParent<ItemControl>();
            if (itemControl != null)
            {
                itemControlSelect = itemControl;
            }
        }
    }

    private void MoveParticleTo(Vector3 screenPos)
    {
        if (paticale == null) return;

        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 5f));

        paticale.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        paticale.transform.position = worldPos;
        paticale.Play();
    }

    void ReleaseObject()
    {
        if (itemControlSelect != null)
        {
            itemControlSelect.OnClick();
            itemControlSelect = null;
        }
    }

    public void LockInput()
    {
        isLocked = true;
    }

    public void UnlockInput()
    {
        isLocked = false;
    }
}
