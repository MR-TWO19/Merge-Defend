using Doozy.Engine.UI;
using UnityEngine;

namespace TwoCore
{
    [RequireComponent(typeof(UIView))]
    public abstract class BaseView : MonoBehaviour
    {
        #region components
        private UIView _view;
        public UIView View { get { if (!_view) _view = GetComponent<UIView>(); return _view; } }
        #endregion

        #region methods
        protected virtual void Awake()
        {
            View.ShowBehavior.OnStart.Event.AddListener(OnShow);
            View.HideBehavior.OnFinished.Event.AddListener(OnHide);
        }

        protected virtual void OnShow()
        {
            //tracking.TrackingManager.OnEnterView(View.ViewName);
        }

        protected virtual void OnHide() { }
        #endregion
    }
}