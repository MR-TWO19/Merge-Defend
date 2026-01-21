using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using LateExe;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TwoCore
{
    public class BootLoader : MonoBehaviour
    {
        public bool bootOnStart = false;
        public float bootDelay = 0.5f;
        public string bootStatusEvent = "bootStatus";
        public string progressEvent = "progressUpdate";
        public string bootCompletedEvent = "GoToMain";
        public bool logStep = false;

        [Space]
        public SceneLoader sceneLoader;
        public UnityEvent startBootEvent;
        public UnityEvent endBootEvent;

        private List<BootStep> bootSteps;
        private BootStep currentBootStep;
        private Executer executer;
        private bool stepCancel;
        private bool initialized = false;
        private bool firstBoot = true;
        private int maxStep;
        private int stepCount;

        public BootStep ForceNextStep { get; set; }

        public static BootLoader Ins { get; private set; }

        IEnumerator Start()
        {
            Ins = this;
            GlobalConfig.Ins.SelectedProfile = Profiles.Ins.profiles.Find(p => p.id == GlobalConfig.Ins.selectedProfileId);

            if (GlobalConfig.Ins.DebugLog)
            {
                var fpsObject = new GameObject("FPS Display", typeof(FPSDisplay));
                DontDestroyOnLoad(fpsObject);
            }

            SceneManager.sceneLoaded += OnSceneLoad;

#if !UNITY_WEBGL
            Application.targetFrameRate = 60;
#endif
            Screen.sleepTimeout = GlobalConfig.Ins.screenSleepMode;
            Debug.unityLogger.logEnabled = GlobalConfig.Ins.VerboseLog || GlobalConfig.Ins.DebugLog;

            initialized = true;
            executer = new Executer(this);
            bootSteps = GetComponentsInChildren<BootStep>().ToList();
            for (int i = 0; i < bootSteps.Count; i++)
                bootSteps[i].StepOrder = i + 1;

            if (bootOnStart) StartBoot();

            yield return new WaitForEndOfFrame();
        }

        void OnEnable()
        {
            InternetConnectionWatcher.OnNoConnection += OnNoConnection;
            InternetConnectionWatcher.OnHaveConnection += OnHaveConnection;

            if (bootOnStart && initialized) StartBoot();
        }

        void OnDisable()
        {
            InternetConnectionWatcher.OnNoConnection -= OnNoConnection;
            InternetConnectionWatcher.OnHaveConnection -= OnHaveConnection;
        }

        public void StartBoot()
        {
            maxStep = bootSteps.Count();
            stepCount = 0;
            startBootEvent.Invoke();
            executer.DelayExecute(bootDelay, () =>
            {
                UpdateBootStep();
                PerformBootStep();
            });
        }

        void UpdateBootStep()
        {
            stepCount++;
            if (currentBootStep == null)
            {
                currentBootStep = bootSteps.First();
            }
            else
            {
                if (ForceNextStep)
                {
                    var prevStep = currentBootStep;
                    currentBootStep = ForceNextStep;
                    ForceNextStep = null;
                    if (prevStep.CalledOnlyOnce) bootSteps.Remove(prevStep);
                }
                else
                {
                    var prevStep = currentBootStep;
                    currentBootStep = bootSteps.Next(prevStep);
                    if (prevStep.CalledOnlyOnce) bootSteps.Remove(prevStep);
                }
            }

            if (currentBootStep)
            {
                currentBootStep.onStatusUpdate = OnStatusUpdate;
                //currentBootStep.onProgressUpdate = OnProgressUpdate;
            }
        }

        //void OnProgressUpdate(float progress, string content)
        //{
        //    ProgressMessage.SendEvent(progressEvent, progress, content);
        //}

        void OnStatusUpdate(string status)
        {
            EventMessage.SendEvent(bootStatusEvent, status);
        }

        void PerformBootStep()
        {
            if (currentBootStep != null)
            {
                if (GlobalConfig.Ins.VerboseLog && logStep)
                {
                    Debug.Log($"Boot - Step [{currentBootStep}]");
                }

                var status = currentBootStep.Status;
                if (!string.IsNullOrEmpty(status))
                    EventMessage.SendEvent(bootStatusEvent, status);

                ProgressMessage.SendEvent(progressEvent, (float)stepCount / maxStep, "");

                currentBootStep.StepIn(onCompleted: () =>
                {
                    UpdateBootStep();
                    executer.DelayExecuteByFrame(1, PerformBootStep);
                },
                onCancel: () => stepCancel = true);
            }
            else if (sceneLoader)
            {
                if (GlobalConfig.Ins.VerboseLog && logStep)
                {
                    Debug.Log($"Boot - Completed");
                }

                if (firstBoot) sceneLoader.LoadSceneAsync();
                else BootCompleted();
            }
            else
            {
                BootCompleted();
                //Debug.LogError("Missing SceneLoader in BootLoader!!");
            }
        }

        void OnHaveConnection()
        {
            if (stepCancel)
            {
                executer.DelayExecuteByFrame(1, PerformBootStep);
                stepCancel = false;
            }
        }

        void OnNoConnection()
        {
            Debug.Log("BootLoader - OnNoConnection");
        }

        void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            BootCompleted();
        }

        void BootCompleted()
        {
            firstBoot = false;

            endBootEvent.Invoke();

            if (!string.IsNullOrEmpty(bootCompletedEvent))
                GameEventMessage.SendEvent(bootCompletedEvent, null);
        }
    }
}