using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggle : MonoBehaviour
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        [SerializeField] private Toggle toggle;

        private Action callBack;

        private void Reset()
        {
            toggle = GetComponent<Toggle>();
            targetImage = GetComponent<Image>();
        }

        private void OnValidate()
        {
            targetImage.sprite = toggle.isOn ? onSprite : offSprite;
        }

        private void Start()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
                OnToggleValueChanged(toggle.isOn);
            }
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (targetImage != null)
            {
                targetImage.sprite = isOn ? onSprite : offSprite;
                callBack?.Invoke();
            }
        }

        public void SetValue(bool isOn)
        {
            if (toggle != null)
            {
                toggle.isOn = isOn;
            }
        }

        public void AddListener(UnityAction<bool> action)
        {
            toggle.onValueChanged.AddListener(action);
        }

        public void RemoveListener(UnityAction<bool> action)
        {
            toggle.onValueChanged.RemoveListener(action);
        }

        public void On()
        {
            SetValue(true);
        }

        public void Off()
        {
            SetValue(false);
        }
    }
}

