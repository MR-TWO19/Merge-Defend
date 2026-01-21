using Hawky.Sound;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hawky.UI
{
    public class AutoToggle : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected GameObject _activeRoot;
        [SerializeField] protected GameObject _deactiveRoot;

        protected Func<bool> _delegate;
        public Action<bool> OnValueChange;

        protected virtual void Awake()
        {
            _button.onClick.AddListener(OnTapMain);
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void SetData(Func<bool> dele)
        {
            _delegate = dele;

            Refresh();
        }

        protected void OnTapMain()
        {
            SoundManager.Instance.PlaySound(SoundId.Click_Button);
            if (_delegate != null)
            {
                var currentValue = _delegate.Invoke();

                OnValueChange?.Invoke(!currentValue);

                Refresh();
            }
        }

        protected virtual void Refresh()
        {
            if (_delegate != null)
            {
                var result = _delegate.Invoke();

                _activeRoot.SetActive(result);
                _deactiveRoot.SetActive(!result);
            }
        }
    }
}
