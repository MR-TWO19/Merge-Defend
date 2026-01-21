using BigNumbers;
using QuickEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Enum = System.Enum;
using Object = UnityEngine.Object;
using Unity_Color = UnityEngine.Color;

namespace TwoCore
{
    public class Draw
    {
        static Draw()
        {
            ToolbarButtonStyle.fontSize = 11;
        }

        public static void Empty(float maxWidth) => Label("", maxWidth);

        public static int IndentLevel
        {
            get => EditorGUI.indentLevel;
            set => EditorGUI.indentLevel = value;
        }

        public static void ResetInputFocus() => GUIUtility.keyboardControl = 0; // Reset Input Focus

        #region primitive
        public static void SeparateHLine(float width = -1)
        {
            Rect rect = width == -1
                ? EditorGUILayout.GetControlRect(false, 1)
                : EditorGUILayout.GetControlRect(GUILayout.Width(width), GUILayout.Height(1));

            EditorGUI.DrawRect(rect, Unity_Color.white);
        }

        public static void SeparateVLine(float maxHeight = -1)
        {
            if (maxHeight == -1) EditorGUILayout.TextField("", GUILayout.MaxWidth(1));
            else EditorGUILayout.TextField("", GUILayout.MaxWidth(1), GUILayout.MaxHeight(maxHeight));
        }

        public static void Separator() => EditorGUILayout.Separator();

        public static string Text(string text, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.TextField(text) : EditorGUILayout.TextField(text, GUILayout.MaxWidth(maxWidth));

        public static string TextField(string name, string text, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            string val = Text(text, maxWidth);
            EndHorizontal();
            return val;
        }

        public static string TextAreaField(string name, string text, float height = 20, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            string val = TextArea(text, height, maxWidth);
            EndHorizontal();
            return val;
        }

        public static string TextArea(string text, float height = 20, float maxWidth = -1)
            => maxWidth == -1
            ? EditorGUILayout.TextArea(text, GUILayout.MaxHeight(height))
            : EditorGUILayout.TextArea(text, GUILayout.MaxHeight(height), GUILayout.MaxWidth(maxWidth));

        public static void Label(string label, float maxWidth = -1)
        {
            if (maxWidth == -1) EditorGUILayout.LabelField(label);
            else EditorGUILayout.LabelField(label, GUILayout.MaxWidth(maxWidth));
        }

        public static void Label(string label, GUIStyle style, float maxWidth = -1)
        {
            if (maxWidth == -1) EditorGUILayout.LabelField(label, style);
            else EditorGUILayout.LabelField(label, style, GUILayout.MaxWidth(maxWidth));
        }

        public static void SetDirty(Object target) => EditorUtility.SetDirty(target);

        public static void FitLabel(string text, float maxHeight = -1)
        {
            label.Text = text;
            float maxWidth = label.x + 12;
            if (maxHeight == -1) EditorGUILayout.LabelField(text, GUILayout.MaxWidth(maxWidth));
            else EditorGUILayout.LabelField(text, GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight));
        }

        public static void LabelBold(string text, float maxWidth = -1)
        {
            if (maxWidth == -1) EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            else EditorGUILayout.LabelField(text, EditorStyles.boldLabel, GUILayout.MaxWidth(maxWidth));
        }

        public static void LabelBoldBox(string text, float maxWidth = -1)
        {
            if (maxWidth == -1) EditorGUILayout.LabelField(text, LabelBoldBoxStyle);
            else EditorGUILayout.LabelField(text, LabelBoldBoxStyle, GUILayout.MaxWidth(maxWidth));
        }

        public static void LabelBoldBox(string text, Unity_Color bgColor, float maxWidth = -1)
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            LabelBoldBox(text, maxWidth);
            GUI.backgroundColor = color;
        }

        public static string OpenFilePanel(string title, string directory, string extension)
            => EditorUtility.OpenFilePanel(title, directory, extension);

        public static string OpenFilePanelWithFilters(string title, string directory, string[] filters)
            => EditorUtility.OpenFilePanelWithFilters(title, directory, filters);

        public static void SpaceAndLabelBoldBox(string text, float maxWidth = -1)
            => SpaceAndLabelBoldBox(text, 12, maxWidth);

        public static void SpaceAndLabelBoldBox(string text, int space, float maxWidth = -1)
        {
            GUILayout.Space(space);
            LabelBoldBox(text, maxWidth);
        }

        public static void SpaceAndLabelBoldBox(string text, Unity_Color bgColor, float maxWidth = -1)
            => SpaceAndLabelBoldBox(text, 12, bgColor, maxWidth);

        public static void SpaceAndLabelBoldBox(string text, int space, Unity_Color bgColor, float maxWidth = -1)
        {
            GUILayout.Space(space);
            LabelBoldBox(text, bgColor, maxWidth);
        }

        public static void FitLabelBold(string text, float width = -1, float maxHeight = -1)
        {
            label.Text = text;
            float _width = label.x + 12;
            _width = width == -1 ? _width : width;
            if (maxHeight == -1) EditorGUILayout.LabelField(text, EditorStyles.boldLabel, GUILayout.Width(_width));
            else EditorGUILayout.LabelField(text, EditorStyles.boldLabel, GUILayout.Width(_width), GUILayout.MaxHeight(maxHeight));
        }

        public static bool Toggle(bool value, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.Toggle(value) : EditorGUILayout.Toggle(value, GUILayout.MaxWidth(maxWidth));

        public static bool ToggleField(string name, bool value, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            bool val = Toggle(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static bool ToggleField(bool value, string name, float maxWidth = 20)
        {
            BeginHorizontal();
            bool val = Toggle(value, maxWidth);
            Label(name, kLabelWidth);
            EndHorizontal();
            return val;
        }

        public static Vector2 Vector2Field(string name, Vector2 value, float maxWidth = 100)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = Vector2(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static Vector2 Vector2(Vector2 value, float maxWidth = 100)
        {
            float width = (Mathf.Max(100, maxWidth) - 40) / 2;
            BeginHorizontal();
            Label("X", 25);
            value.x = Float(value.x, width);
            Label("Y", 25);
            value.y = Float(value.y, width);
            EndHorizontal();
            return value;
        }

        public static Vector2Int Vector2IntField(string name, Vector2Int value, float maxWidth = 100)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = Vector2Int(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static Vector2Int Vector2Int(Vector2Int value, float maxWidth = 100)
        {
            float width = (Mathf.Max(100, maxWidth) - 40) / 2;
            BeginHorizontal();
            Label("X", 25);
            value.x = Int(value.x, width);
            Label("Y", 25);
            value.y = Int(value.y, width);
            EndHorizontal();
            return value;
        }

        public static int IntSlider(int value, int minValue, int maxValue, int maxWidth = -1)
        {
            return maxWidth == -1
                ? EditorGUILayout.IntSlider(value, minValue, maxValue)
                : EditorGUILayout.IntSlider(value, minValue, maxValue, GUILayout.MaxWidth(maxWidth));
        }

        public static int IntSliderField(string name, int value, int minValue, int maxValue, int maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            int val = IntSlider(value, minValue, maxValue, maxWidth);
            EndHorizontal();
            return val;
        }

        public static int Int(int value, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.IntField(value) : EditorGUILayout.IntField(value, GUILayout.MaxWidth(maxWidth));

        public static int IntField(string name, int value, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            int val = Int(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static long Long(long value, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.LongField(value) : EditorGUILayout.LongField(value, GUILayout.MaxWidth(maxWidth));

        public static long LongField(string name, long value, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            long val = Long(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static bool BeginFoldoutGroup(bool fold, string content) => BeginFoldoutGroup(fold, content, Unity_Color.green);
        public static bool BeginFoldoutGroup(bool fold, string content, Unity_Color bgColor)
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            var ret = EditorGUILayout.BeginFoldoutHeaderGroup(fold, content, FoldoutStyle);
            GUI.backgroundColor = color;
            return ret;
        }

        public static void EndFoldoutGroup() => EditorGUILayout.EndFoldoutHeaderGroup();

        public static float Float(float value, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.FloatField(value) : EditorGUILayout.FloatField(value, GUILayout.MaxWidth(maxWidth));

        public static float FloatField(string name, float value, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            float val = Float(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static float FloatSlider(float value, float minValue, float maxValue, int maxWidth = -1)
        {
            return maxWidth == -1
                ? EditorGUILayout.Slider(value, minValue, maxValue)
                : EditorGUILayout.Slider(value, minValue, maxValue, GUILayout.MaxWidth(maxWidth));
        }

        public static float FloatSliderField(string name, float value, float minValue, float maxValue, int maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            float val = FloatSlider(value, minValue, maxValue, maxWidth);
            EndHorizontal();
            return val;
        }

        public static double Double(double value, float maxWidth = -1)
            => maxWidth == -1 ? EditorGUILayout.DoubleField(value) : EditorGUILayout.DoubleField(value, GUILayout.MaxWidth(maxWidth));

        public static double DoubleField(string name, double value, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            double val = Double(value, maxWidth);
            EndHorizontal();
            return val;
        }

        public static BigNumber BigNumber(BigNumber bigNumber, float width = -1)
        {
            EditorGUI.BeginChangeCheck();
            string newValueStr = Text(bigNumber.ToShortString(4, 0), width);
            BigNumber newValue = EditorGUI.EndChangeCheck() ? BigNumbers.BigNumber.FromShortString(newValueStr) : bigNumber;
            return newValue;
        }

        public static BigNumber BigNumberField(string name, BigNumber bigNumber, float width = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            BigNumber newValue = BigNumber(bigNumber, width);
            EndHorizontal();
            return newValue;
        }

        public static void EndVertical() => EditorGUILayout.EndVertical();

        public static void BeginVertical(float width = -1)
        {
            if (width == -1) EditorGUILayout.BeginVertical();
            else EditorGUILayout.BeginVertical(GUILayout.Width(width));
        }

        public static void BeginVertical(params GUILayoutOption[] options) => EditorGUILayout.BeginVertical(options);
        public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
        {
            if (style == null) EditorGUILayout.BeginVertical(options);
            else EditorGUILayout.BeginVertical(style, options);
        }

        public static void EndHorizontal() => EditorGUILayout.EndHorizontal();
        public static void BeginHorizontal(float height = -1)
        {
            if (height == -1) EditorGUILayout.BeginHorizontal();
            else EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        }
        public static void BeginHorizontal(GUIStyle style)
        {
            if (style == null) EditorGUILayout.BeginHorizontal();
            else EditorGUILayout.BeginHorizontal(style);
        }

        public static void BeginHorizontal(params GUILayoutOption[] options) => EditorGUILayout.BeginHorizontal(options);

        public static void BeginDisabledGroup(bool value = true) => EditorGUI.BeginDisabledGroup(value);
        public static void EndDisabledGroup() => EditorGUI.EndDisabledGroup();

        public static void BeginChangeCheck() => EditorGUI.BeginChangeCheck();
        public static bool EndChangeCheck(Object target = null)
        {
            if (EditorGUI.EndChangeCheck()) { if (target != null) SetDirty(target); return true; } else return false;
        }

        public static Vector2 BeginScrollView(Vector2 scrollPos) => EditorGUILayout.BeginScrollView(scrollPos);
        public static void EndScrollView() => EditorGUILayout.EndScrollView();

        public static T Object<T>(T @object, float maxWidth = -1) where T : Object
            => Object(@object, false, maxWidth);

        public static T Object<T>(T @object, bool allowSceneObjects = false, float maxWidth = -1) where T : Object
            => maxWidth == -1
            ? (T)EditorGUILayout.ObjectField(@object, typeof(T), allowSceneObjects)
            : (T)EditorGUILayout.ObjectField(@object, typeof(T), allowSceneObjects, GUILayout.MaxWidth(maxWidth));

        public static Color Color(Color color, float maxWidth = -1)
            => maxWidth == -1
            ? EditorGUILayout.ColorField(color)
            : EditorGUILayout.ColorField(color, GUILayout.Width(maxWidth));

        public static Color ColorField(string name, Color color, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            Color val = Color(color, maxWidth);
            EndHorizontal();
            return val;
        }

        public static T ObjectField<T>(string name, T @object, float maxWidth = -1) where T : Object
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            T val = Object(@object, maxWidth);
            EndHorizontal();
            return val;
        }

        public static Range RangeField(string name, Range range, float width = -1)
        {
            range.min = FloatField(name + " Min", range.min, width);
            range.max = FloatField(name + " Max", range.max, width);
            range.Validate();
            return range;
        }

        public static Range RangeShortField(string name, Range range, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            range.min = Float(range.min, maxWidth == -1 ? -1 : maxWidth / 2);
            range.max = Float(range.max, maxWidth == -1 ? -1 : maxWidth / 2);
            EndHorizontal();
            return range;
        }

        public static T Enum<T>(T selected, float maxWidth = -1) where T : Enum
            => (T)(maxWidth == -1
            ? EditorGUILayout.EnumPopup(selected)
            : EditorGUILayout.EnumPopup(selected, GUILayout.Width(maxWidth)));

        public static T EnumField<T>(string name, T selected, float maxWidth = -1) where T : Enum
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            T val = Enum(selected, maxWidth);
            EndHorizontal();
            return val;
        }

        public static bool ToggleGroup(bool show, string text, float width = 16)
            => ToggleGroup(show, text, QColors.Color.Green, width);

        public static bool FitToggleGroup(bool show, string text, QColors.Color color = QColors.Color.Green)
        {
            label.Text = text;
            float maxWidth = label.x + 36;
            return ToggleGroup(show, text, color, maxWidth);
        }

        public static bool ToggleGroup(bool show, string text, QColors.Color color = QColors.Color.Green, float width = 16)
        {
            if (QUI.GhostBar(text, color, new AnimBool(show), width, 20))
                return !show;
            return show;
        }

        public static T StringPopupField<T>(string name, T value, List<T> list, string valueField, float maxWidth = -1)
            => StringPopupField(name, "-", value, list, valueField, maxWidth);

        public static T StringPopupField<T>(string name, string hint, T value, List<T> list, string valueField, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = StringPopup(hint, value, list, valueField, maxWidth);
            EndHorizontal();
            return val;
        }

        public static T StringPopup<T>(T value, List<T> list, string valueField, float maxWidth = -1)
            => StringPopup("-", value, list, valueField, maxWidth);

        public static T StringPopup<T>(string hint, T value, List<T> list, string field, float maxWidth = -1)
        {
            string strValue = value != null ? (string)value.GetType().GetField(field).GetValue(value) : "-";


            List<string> values;
            Type type = typeof(T);

            if (field.Length > 0)
            {
                values = list.Map(l => (string)type.GetField(field).GetValue(l));
            }
            else
            {
                values = list.Map(l => l.ToString());
            }

            string newStrValue = StringPopup(hint, strValue, values, maxWidth);
            int newIdx = values.IndexOf(newStrValue);
            if (newIdx > -1)
                return list[newIdx];
            else
                return default;
        }

        public static string StringPopupField<T>(string name, string value, List<T> list, string valueField, float maxWidth = -1)
            => StringPopupField(name, "-", value, list, valueField, maxWidth);

        public static string StringPopupField<T>(string name, string hint, string value, List<T> list, string valueField, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = StringPopup(hint, value, list, valueField, maxWidth);
            EndHorizontal();
            return val;
        }

        public static string StringPopup<T>(string value, List<T> list, string field, float maxWidth = -1)
            => StringPopup("-", value, list, field, maxWidth);

        public static string StringPopup<T>(string hint, string value, List<T> list, string field, float maxWidth = -1)
        {
            List<string> values;
            Type type = typeof(T);
            if (field.Length > 0) values = list.Map(l => (string)type.GetField(field).GetValue(l));
            else values = list.Map(l => l.ToString());

            return StringPopup(hint, value, values, maxWidth);
        }

        public static string StringPopupField(string name, string value, List<string> values, float maxWidth = -1)
            => StringPopupField(name, "-", value, values, maxWidth);

        public static string StringPopupField(string name, string hint, string value, List<string> values, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = StringPopup(hint, value, values, maxWidth);
            EndHorizontal();
            return val;
        }

        public static string StringPopup(string value, List<string> values, float maxWidth = -1)
            => StringPopup("-", value, values, maxWidth);

        public static string StringPopup(string hint, string value, List<string> values, float maxWidth = -1)
            => _StringPopup(value, values.ToList().Unshift(hint), maxWidth);

        private static string _StringPopup(string value, List<string> values, float maxWidth = -1)
        {
            if (values.Count == 0) return value;

            int selectedIndex = values.IndexOf(value);
            selectedIndex = Math.Max(0, selectedIndex);

            int newIndex = maxWidth == -1
                ? EditorGUILayout.Popup(selectedIndex, values.ToArray())
                : EditorGUILayout.Popup(selectedIndex, values.ToArray(), GUILayout.Width(maxWidth));

            return values[newIndex];
        }

        public static string StringPopup<T>(Rect rect, string value, List<T> list, string valueField)
        {
            List<string> values;

            Type type = typeof(T);
            if (valueField.Length > 0)
            {
                values = list.Map(l => (string)type.GetField(valueField).GetValue(l));
            }
            else
            {
                values = list.Map(l => l.ToString());
            }

            if (values.Count > 0)
            {
                int selectedIndex = values.IndexOf(value);
                selectedIndex = Math.Max(0, selectedIndex);
                int newIndex = EditorGUI.Popup(rect, selectedIndex, values.ToArray());
                return values[newIndex];
            }
            else
            {
                EditorGUI.Popup(rect, 0, values.ToArray());
                return "";
            }
        }

        public static string StringPopup(Rect rect, string value, List<string> list)
        {
            if (list.Count == 0) return value;

            int selectedIndex = list.IndexOf(value);
            selectedIndex = Math.Max(0, selectedIndex);
            int newIndex = EditorGUI.Popup(rect, selectedIndex, list.ToArray());
            return list[newIndex];
        }

        public static int IntPopup<T>(int selected, List<T> list, string labelField, string valueField, float maxWidth = -1)
            => IntPopup("-", selected, list, labelField, valueField, maxWidth);

        public static int IntPopup<T>(int selected, List<T> list, float maxWidth = -1)
            => IntPopup(selected, list, "name", "id", maxWidth);

        public static int IntPopupField<T>(string name, int selected, List<T> list, string labelField = "name", string valueField = "id", float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            int val = IntPopup("-", selected, list, labelField, valueField, maxWidth);
            EndHorizontal();
            return val;
        }

        public static int IntPopup<T>(string hint, int value, List<T> list, string labelField, string valueField, float maxWidth = -1)
        {
            List<string> labels = new List<string>() { hint };
            List<int> values = new List<int>() { -1 };

            Type type = typeof(T);

            if (labelField.Length > 0)
            {
                var shortifyLabel = list.Count > 20;
                foreach (T obj in list)
                {
                    string label = (string)type.GetField(labelField).GetValue(obj);
                    label = shortifyLabel ? label.First() + "/" + label : label;
                    labels.Add(label);
                    values.Add((int)type.GetField(valueField).GetValue(obj));
                }
            }
            else
            {
                foreach (T obj in list)
                {
                    int val = (int)type.GetField(valueField).GetValue(obj);
                    labels.Add(val.ToString());
                    values.Add(val);
                }
            }

            return IntPopup(value, labels, values, maxWidth);
        }

        public static int IntPopupField(string name, int selected, List<int> list, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            int val = IntPopup(selected, list, maxWidth);
            EndHorizontal();
            return val;
        }

        public static int IntPopup(int value, List<int> list, float width = -1)
            => IntPopup(value, list.Select(_ => _.ToString()).ToList(), list, width);

        private static int IntPopup(int value, List<string> contents, List<int> values, float width = -1) => width == -1
            ? EditorGUILayout.IntPopup(value, contents.ToArray(), values.ToArray())
            : EditorGUILayout.IntPopup(value, contents.ToArray(), values.ToArray(), GUILayout.Width(width));

        public static int IntPopup<T>(Rect rect, int value, List<T> list, string labelField, string valueField)
        {
            return IntPopup(rect, "-", value, list, labelField, valueField);
        }

        public static int IntPopup<T>(Rect rect, string hint, int value, List<T> list, string labelField, string valueField)
        {
            List<string> contents = new List<string>() { hint };
            List<int> values = new List<int>() { -1 };

            foreach (T obj in list)
            {
                Type type = obj.GetType();
                if (labelField.Length > 0)
                {
                    contents.Add((string)type.GetField(labelField).GetValue(obj));
                    values.Add((int)type.GetField(valueField).GetValue(obj));
                }
                else
                {
                    int val = (int)type.GetField(valueField).GetValue(obj);
                    contents.Add(val.ToString());
                    values.Add(val);
                }
            }

            return EditorGUI.IntPopup(rect, value, contents.ToArray(), values.ToArray());
        }

        public static bool Button(string text, float maxWidth = -1, float maxHeight = -1)
                    => maxHeight == -1
                    ? maxWidth == -1
                    ? GUILayout.Button(text)
                    : GUILayout.Button(text, GUILayout.MaxWidth(maxWidth))
                    : GUILayout.Button(text, GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight));

        public static bool Button(string text, Color bgColor, float maxWidth = -1, float maxHeight = -1)
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            bool ret = Button(text, maxWidth, maxHeight);
            GUI.backgroundColor = color;
            return ret;
        }

        public static bool Button(string text, Color bgColor, Color textColor, float maxWidth = -1)
        {
            Color _textColor = GUI.skin.button.normal.textColor;
            Color _bgColor = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            GUI.skin.button.normal.textColor = textColor;
            bool value = Button(text, maxWidth);
            GUI.backgroundColor = _bgColor;
            GUI.skin.button.normal.textColor = _textColor;
            return value;
        }

        public static bool FitToolbarButton(string text, float maxHeight = -1, float padding = 12)
        {
            label.Text = text;
            float maxWidth = label.x + padding;
            return ToolbarButton(text, maxWidth, maxHeight);
        }

        public static bool FitToolbarButton(string text, Color backgroundColor, Color textColor, float maxHeight = -1)
        {
            label.Text = text;
            float maxWidth = label.x + 12;
            return ToolbarButton(text, backgroundColor, textColor, maxWidth, maxHeight);
        }

        public static bool ToolbarButton(Rect position, string text, Color backgroundColor, Color textColor)
        {
            float oldH = ToolbarButtonStyle.fixedHeight;
            Color oldBgColor = GUI.backgroundColor;
            Color oldColor = GUI.skin.button.normal.textColor;

            ToolbarButtonStyle.fixedHeight = position.height;
            GUI.backgroundColor = backgroundColor;
            ToolbarButtonStyle.normal.textColor = textColor;
            bool ret = GUI.Button(position, text, ToolbarButtonStyle);

            GUI.backgroundColor = oldBgColor;
            ToolbarButtonStyle.normal.textColor = oldColor;
            ToolbarButtonStyle.fixedHeight = oldH;
            return ret;
        }

        public static bool ToolbarButton(string text, float maxWidth = -1, float maxHeight = -1)
        {
            float oldH = ToolbarButtonStyle.fixedHeight;
            if (maxHeight != -1)
                ToolbarButtonStyle.fixedHeight = maxHeight;

            bool ret = maxWidth == -1
                ? GUILayout.Button(text, ToolbarButtonStyle)
                : GUILayout.Button(text, ToolbarButtonStyle, GUILayout.MaxWidth(maxWidth));

            ToolbarButtonStyle.fixedHeight = oldH;

            return ret;
        }

        public static bool ToolbarButton(string text, Color backgroundColor, Color textColor, float maxWidth = -1, float maxHeight = -1)
        {
            float oldH = ToolbarButtonStyle.fixedHeight;
            if (maxHeight != -1)
                ToolbarButtonStyle.fixedHeight = maxHeight;

            Color color = GUI.skin.button.normal.textColor;
            Color bgColor = GUI.backgroundColor;
            GUI.backgroundColor = backgroundColor;
            ToolbarButtonStyle.normal.textColor = textColor;

            bool value = maxWidth == -1
                ? GUILayout.Button(text, ToolbarButtonStyle)
                : GUILayout.Button(text, ToolbarButtonStyle, GUILayout.MaxWidth(maxWidth));

            GUI.backgroundColor = bgColor;
            ToolbarButtonStyle.normal.textColor = color;
            ToolbarButtonStyle.fixedHeight = oldH;
            return value;
        }

        public static bool FitToggleButton(bool selected, string text, float height = -1, int fontSize = 12)
        {
            label.Text = text;
            float maxWidth = label.x + 12;
            return ToggleButton(selected, text, maxWidth, height, fontSize);
        }

        public static bool ToggleButton(bool selected, string text, float maxWidth = -1, float height = -1, int fontSize = 12)
        {
            float oldH = ToolbarButtonStyle.fixedHeight;
            if (height != -1) ToolbarButtonStyle.fixedHeight = height;
            int oldFontSize = ToolbarButtonStyle.fontSize;
            ToolbarButtonStyle.fontSize = fontSize;
            ToolbarButtonStyle.alignment = TextAnchor.MiddleCenter;
            bool ret = height == -1
            ? GUILayout.Toggle(selected, text, ToolbarButtonStyle, GUILayout.MaxWidth(maxWidth))
            : GUILayout.Toggle(selected, text, ToolbarButtonStyle, GUILayout.MaxWidth(maxWidth), GUILayout.Height(height));
            ToolbarButtonStyle.fixedHeight = oldH;
            ToolbarButtonStyle.fontSize = oldFontSize;
            return ret;
        }

        public static bool FitButton(string text, Color bgColor, float height = -1)
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            bool ret = FitButton(text, height);
            GUI.backgroundColor = color;
            return ret;
        }

        public static bool FitButton(string text, float height = -1)
        {
            label.Text = text;
            float maxWidth = label.x + 12;
            return height == -1
            ? GUILayout.Button(text, GUILayout.MaxWidth(maxWidth))
            : GUILayout.Button(text, GUILayout.MaxWidth(maxWidth), GUILayout.MinHeight(height));
        }

        public static bool FitButton(string text, Color bgColor, GUIStyle style, float height = -1)
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            bool ret = FitButton(text, style, height);
            GUI.backgroundColor = color;
            return ret;
        }

        public static bool FitButton(string text, GUIStyle style, float height = -1)
        {
            label.Text = text;
            float maxWidth = label.x + 12;
            return height == -1
            ? GUILayout.Button(text, style, GUILayout.MaxWidth(maxWidth))
            : GUILayout.Button(text, style, GUILayout.MaxWidth(maxWidth), GUILayout.MinHeight(height));
        }

        public static float GetWidth(string text)
        {
            label.Text = text;
            return label.x + 12;
        }

        public static void Space(float pixels = 12) => GUILayout.Space(pixels);

        public static void SpaceAndLabelBold(string label, float pixels = 12)
        {
            GUILayout.Space(pixels);
            FitLabelBold(label);
        }

        public static void FlexibleSpace() => GUILayout.FlexibleSpace();

        public static void SpriteThumb(Sprite sprite, bool autoSize = true, float maxWidth = 100, float maxHeight = 100)
        {
            if (autoSize)
            {
                maxWidth = Mathf.Min(sprite ? sprite.rect.width : 80, maxWidth);
                maxHeight = Mathf.Min(sprite ? sprite.rect.height : 80, maxHeight);
            }
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(maxWidth), GUILayout.Height(maxHeight));
            var coord = new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
            GUI.DrawTextureWithTexCoords(rect, sprite.texture, coord);
        }

        public static Sprite SpriteField(string name, Sprite icon, float maxWidth = -1)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var ret = Sprite(icon, maxWidth);
            EndHorizontal();
            return ret;
        }

        public static Sprite SpriteField(string name, Sprite image, bool autoSize, float maxWidth = 100, float maxHeight = 100)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = Sprite(image, autoSize, maxWidth, maxHeight);
            EndHorizontal();
            return val;
        }

        public static Sprite Sprite(Sprite icon, float maxWidth = -1)
        {
            if (maxWidth == -1) return (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false);
            else
            {
                BeginHorizontal();
                var val = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false, GUILayout.MaxWidth(maxWidth - 4));
                Space(4);
                EndHorizontal();
                return val;
            }
        }

        public static Sprite Sprite(Sprite sprite, bool autoSize = true, float maxWidth = 100, float maxHeight = 100)
        {
            if (autoSize)
            {
                maxWidth = Mathf.Min(sprite ? sprite.rect.width : 80, maxWidth);
                maxHeight = Mathf.Min(sprite ? sprite.rect.height : 80, maxHeight);
            }

            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(maxWidth), GUILayout.Height(maxHeight));
            return EditorGUI.ObjectField(rect, sprite, typeof(Sprite), false) as Sprite;
        }

        public static Texture TextureField(string name, Texture texture, bool autoSize = true, float width = 100, float height = 100)
        {
            BeginHorizontal();
            Label(name, kLabelWidth);
            var val = Texture(texture, autoSize, width, height);
            EndHorizontal();
            return val;
        }

        public static Texture Texture(Texture texture, bool autoSize = true, float maxWidth = 100, float maxHeight = 100)
        {
            if (texture && autoSize)
            {
                maxWidth = Mathf.Min(texture ? texture.width : 80, maxWidth);
                maxHeight = Mathf.Min(texture ? texture.height : 80, maxHeight);
            }

            var rect = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight));
            return EditorGUI.ObjectField(rect, texture, typeof(Texture), false) as Texture;
        }

        public static Material MaterialField(string name, Material mat, bool allowScene = true, float maxWidth = 100)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(kLabelWidth));

            mat = Material(mat, allowScene, maxWidth);

            EditorGUILayout.EndHorizontal();
            return mat;
        }

        public static Material Material(Material mat, bool allowScene, float width)
        {
            return (Material)EditorGUILayout.ObjectField(mat, typeof(Material), allowScene, GUILayout.Width(width));
        }
        #endregion

        #region styles
        public static float kLabelWidth = 130;
        public static string noValueLabelColor = "#808080";
        public static Label label = new Label();

        public static GUIStyle DefaultStyle { get; } = new GUIStyle() { padding = new RectOffset(10, 10, 10, 10) };
        public static GUIStyle SmallText { get; } = new GUIStyle() { fontSize = 8 };
        public static GUIStyle ToolbarButtonStyle { get; } = new GUIStyle(EditorStyles.toolbarButton) { fontStyle = FontStyle.Bold };
        public static GUIStyle TopToolbarStyle { get; } = new GUIStyle(EditorStyles.toolbarButton) { fixedHeight = 40 };
        public static GUIStyle SubContentStyle { get; } = new GUIStyle() { margin = new RectOffset(10, 10, 10, 10) };
        public static GUIStyle BoxStyle { get; } = new GUIStyle("HelpBox") { margin = new RectOffset(), padding = new RectOffset(10, 10, 10, 10) };
        public static GUIStyle DeleteButtonStyle { get; } = new GUIStyle("OL Minus");
        public static GUIStyle TableViewToolbarTextStyle { get; } = new GUIStyle(EditorStyles.miniLabel);
        public static GUIStyle TableViewButtonStyle { get; } = new GUIStyle("toolbarbutton");
        public static GUIStyle BoldTextStyle { get; } = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
        public static GUIStyle RichTextLabelStyle { get; } = new GUIStyle(EditorStyles.label) { richText = true };
        public static GUIStyle SideBarStyle { get; } = new GUIStyle("HelpBox")
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(10, 5, 5, 5)
        };

        public static GUIStyle TitleStyle { get; } = new GUIStyle("IN TitleText")
        {
            alignment = TextAnchor.MiddleLeft,
            padding = { left = 5 },
            margin = { top = 10 }
        };

        public static GUIStyle CenteredGrayLabel { get; } = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Unity_Color.gray }
        };

        public static GUIStyle ButtonStyle { get; } = new GUIStyle(EditorStyles.miniButton)
        {
            fixedHeight = 20f,
            fontStyle = FontStyle.Bold,
            fontSize = 12
        };

        public static GUIStyle LabelBoldBoxStyle { get; } = new GUIStyle(EditorStyles.helpBox)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset(),
            padding = new RectOffset(10, 4, 4, 4)
        };

        public static GUIStyle FoldoutStyle { get; } = new GUIStyle(EditorStyles.foldoutHeader)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            padding = new RectOffset(20, 4, 4, 4)
        };

        public static GUIStyle CategoryListItemStyle { get; } =
            new GUIStyle(EditorStyles.helpBox)
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(8, 7, 6, 6),
                wordWrap = false,
                clipping = TextClipping.Overflow,
                alignment = TextAnchor.MiddleLeft
            };
        #endregion
    }
}