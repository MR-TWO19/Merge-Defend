using System.Collections;
using Hawky.Scene;


namespace KeatonCore.Tutorial
{
    public enum TutorialState
    {
        Idle,
        Running,    
        Completed
    }

    public abstract class FTUEController : PopupController
    {
        protected int currentStep = 0;
        protected TutorialState currentState = TutorialState.Idle;

        public virtual void StartFTUE()
        {
            currentState = TutorialState.Running;
            currentStep = 1;
            OnStart();
            StartCoroutine(RunStep(currentStep));
        }

        public virtual void EndFTUE()
        {
            currentState = TutorialState.Completed;
            TutorialManager.Instance.CurrTutorial = global::Tutorial.None;
            OnEnd();
        }

        public virtual void NextStep()
        {
            currentStep++;
            StartCoroutine(RunStep(currentStep));
        }

        public virtual void GotoStep(int stepIndex)
        {
            currentStep = stepIndex;
            StartCoroutine(RunStep(currentStep));
        }

        protected virtual IEnumerator RunStep(int stepIndex)
        {
            yield return OnStep(stepIndex);
        }

        protected abstract void OnStart();
        protected abstract void OnEnd();
        protected abstract IEnumerator OnStep(int stepIndex);
    }
}

