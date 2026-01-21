using Doozy.Engine.UI;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TwoCore.editor
{
    public class UICreator
    {
        public const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        public const string kWhiteSprite = "Assets/Plugins/Extensions/Doozy/Examples/Textures/Shapes/SquareFull@32px.png";
        public const string kLockSprite = "Assets/Plugins/Extensions/Doozy/Examples/Icons/Awesome/lock-alt.png";

        public static Sprite WhiteSprite => AssetDatabase.LoadAssetAtPath<Sprite>(kWhiteSprite);
        public static Sprite LockSprite => AssetDatabase.LoadAssetAtPath<Sprite>(kLockSprite);

        private static void SetActiveAndUndo(GameObject go)
        {
            Selection.activeObject = go;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }

        public static UIButton CreateUIButton(GameObject parent, string name, string text = "Button")
        {
            UIButton uiButton = UIButton.CreateUIButton(parent);
            GameObject buttonGo = uiButton.gameObject;
            buttonGo.name = name;
#if dUI_TextMeshPro
            uiButton.TextMeshProLabel.text = text;
#endif
            return uiButton;
        }

        public static Image CreateImage(GameObject parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            GameObjectUtility.SetParentAndAlign(go, parent);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40f);
            go.GetComponent<Image>().sprite = WhiteSprite;
            go.GetComponent<Image>().raycastTarget = false;

            return go.GetComponent<Image>();
        }

        private static TextMeshProUGUI CreateTextMeshProUGUI(GameObject parent, string name = "Text (TMP)", string text = "Text")
        {
            GameObject textGo = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(textGo, parent);
            textGo.GetComponent<RectTransform>().FitParent();
            textGo.GetComponent<TextMeshProUGUI>().text = text;
            textGo.GetComponent<TextMeshProUGUI>().fontSize = 24;
            textGo.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            textGo.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;

            return textGo.GetComponent<TextMeshProUGUI>();
        }

        private static UIProgressBar CreateUIProgressBar(GameObject parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(UIProgressBar));
            GameObjectUtility.SetParentAndAlign(go, parent);
            go.GetComponent<Image>().raycastTarget = false;
            go.GetComponent<Image>().sprite = WhiteSprite;
            go.GetComponent<Image>().color = ColorExt.Parse("#545454");

            Image imageGo = CreateImage(go, "Image");
            imageGo.GetComponent<RectTransform>().FitParent();
            imageGo.GetComponent<Image>().raycastTarget = false;
            imageGo.GetComponent<Image>().sprite = WhiteSprite;
            imageGo.GetComponent<Image>().type = Image.Type.Filled;
            imageGo.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
            imageGo.GetComponent<Image>().fillAmount = 0.8f;
            imageGo.GetComponent<Image>().color = Color.yellow;

            var progressBar = go.GetComponent<UIProgressBar>();
            progressBar.image = imageGo.GetComponent<Image>();

            return progressBar;
        }

        private static UIProgress CreateUIProgress(GameObject parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(UIProgress));
            GameObjectUtility.SetParentAndAlign(go, parent);

            var progressBar = CreateUIProgressBar(go, "Progress Bar");
            progressBar.GetComponent<RectTransform>().FitParent();

            var progressText = CreateTextMeshProUGUI(go, "Progress Text", "00/99");
            progressText.color = ColorExt.Parse("#737373");

            var questProgress = go.GetComponent<UIProgress>();
            questProgress.progressText = progressText;
            questProgress.progressBar = progressBar;

            return go.GetComponent<UIProgress>();
        }

        [MenuItem("GameObject/TwoCore/Core/UIPages", false, 0)]
        private static void CreateUIPages(MenuCommand menuCommand)
        {
            GameObject targetParent = null;
            GameObject selectedGO = menuCommand.context as GameObject;
            if (selectedGO != null) targetParent = selectedGO;

            GameObject go = new GameObject("UIPages", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(UIPages));
            GameObjectUtility.SetParentAndAlign(go, targetParent);
            go.GetComponent<RectTransform>().FitParent();
            go.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -250f);
            go.GetComponent<RectTransform>().offsetMin = Vector2.zero;

            UIPages pages = go.GetComponent<UIPages>();
            GameObject labelContaingerGo = new GameObject("Labels", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(labelContaingerGo, go);
            labelContaingerGo.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 80f);
            labelContaingerGo.AddComponent<HorizontalLayoutGroup>();
            labelContaingerGo.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
            labelContaingerGo.GetComponent<HorizontalLayoutGroup>().childControlHeight = true;
            labelContaingerGo.AddComponent<LayoutElement>();
            labelContaingerGo.GetComponent<LayoutElement>().preferredHeight = 80f;
            labelContaingerGo.GetComponent<LayoutElement>().flexibleHeight = 0f;

            GameObject contentContaingerGo = new GameObject("Contents", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(contentContaingerGo, go);
            contentContaingerGo.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 540f);
            contentContaingerGo.AddComponent<HorizontalLayoutGroup>();
            contentContaingerGo.AddComponent<LayoutElement>();
            contentContaingerGo.GetComponent<LayoutElement>().preferredHeight = 540f;
            contentContaingerGo.GetComponent<LayoutElement>().flexibleHeight = 1f;

            //pages.pageSwiper = contentContaingerGo.GetComponent<UIPageSwiper>();
            pages.labelContainer = labelContaingerGo.GetComponent<RectTransform>();
            pages.contentContainer = contentContaingerGo.GetComponent<RectTransform>();

            SetActiveAndUndo(go);
        }

        [MenuItem("GameObject/TwoCore/Core/UITab", false, 0)]
        private static void CreateTab(MenuCommand menuCommand)
        {
            GameObject targetParent = null;
            GameObject selectedGO = menuCommand.context as GameObject;
            if (selectedGO != null) targetParent = selectedGO;

            GameObject go = new GameObject("UITab", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(UITab));
            GameObjectUtility.SetParentAndAlign(go, targetParent);
            go.GetComponent<RectTransform>().FitParent();
            go.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -250f);
            go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            go.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
            go.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            go.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;

            UITab tab = go.GetComponent<UITab>();
            GameObject labelContaingerGo = new GameObject("Labels", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(labelContaingerGo, go);
            labelContaingerGo.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 80f);
            labelContaingerGo.AddComponent<HorizontalLayoutGroup>();
            labelContaingerGo.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
            labelContaingerGo.GetComponent<HorizontalLayoutGroup>().childControlHeight = true;
            labelContaingerGo.AddComponent<LayoutElement>();
            labelContaingerGo.GetComponent<LayoutElement>().preferredHeight = 80f;
            labelContaingerGo.GetComponent<LayoutElement>().flexibleHeight = 0f;

            GameObject contentContaingerGo = new GameObject("Contents", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(contentContaingerGo, go);
            contentContaingerGo.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 540f);
            contentContaingerGo.AddComponent<LayoutElement>();
            contentContaingerGo.GetComponent<LayoutElement>().preferredHeight = 540f;
            contentContaingerGo.GetComponent<LayoutElement>().flexibleHeight = 1f;

            tab.labelContainer = labelContaingerGo.GetComponent<RectTransform>();
            tab.contentContainer = contentContaingerGo.GetComponent<RectTransform>();

            SetActiveAndUndo(go);
        }

        //        #region UI Sfx/Music
        //        [MenuItem("GameObject/VGames/Core/UIMusicToggle", false, 1)]
        //        private static void CreateMusicToggle(MenuCommand menuCommand)
        //        {
        //            GameObject targetParent = null;
        //            GameObject selectedGO = menuCommand.context as GameObject;
        //            if(selectedGO != null) targetParent = selectedGO;

        //            GameObject go = new GameObject("UIMusicToggle", typeof(RectTransform), typeof(Image), typeof(UIMusicToggle));
        //            GameObjectUtility.SetParentAndAlign(go, targetParent);
        //            go.GetComponent<RectTransform>().MidCenter(new Vector2(50f, 50f));
        //            go.GetComponent<Image>().SetSpriteDefault();
        //            go.GetComponent<Toggle>().targetGraphic = go.GetComponent<Image>();

        //            SetActiveAndUndo(go);
        //        }

        //        [MenuItem("GameObject/VGames/Core/UISoundToggle", false, 1)]
        //        private static void CreateSoundToggle(MenuCommand menuCommand)
        //        {
        //            GameObject targetParent = null;
        //            GameObject selectedGO = menuCommand.context as GameObject;
        //            if(selectedGO != null) targetParent = selectedGO;

        //            GameObject go = new GameObject("UISoundToggle", typeof(RectTransform), typeof(Image), typeof(UISoundToggle));
        //            GameObjectUtility.SetParentAndAlign(go, targetParent);
        //            go.GetComponent<RectTransform>().MidCenter(new Vector2(50f, 50f));
        //            go.GetComponent<Image>().SetSpriteDefault();
        //            go.GetComponent<Toggle>().targetGraphic = go.GetComponent<Image>();

        //            SetActiveAndUndo(go);
        //        }
        //        #endregion
    }
}