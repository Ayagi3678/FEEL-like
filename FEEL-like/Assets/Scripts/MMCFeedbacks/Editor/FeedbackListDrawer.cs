using System;
using AnnulusGames.LucidTools.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace MMCFeedbacks
{
    [CustomPropertyDrawer(typeof(FeedbackList))]
    public class FeedbackListDrawer : PropertyDrawer
    {
        private FeedbackList _feedbackList;
        private bool _isProSkin;
        private GUIStyle _labelStyle;
        private ReorderableList _reorderableList;
        private SerializedProperty _saveProperty;

        private void Initialize(SerializedProperty property)
        {
            _saveProperty = property;
            _feedbackList = property.GetValue<FeedbackList>();
            var listProperty = property.FindPropertyRelative("List");

            _reorderableList = new ReorderableList(property.serializedObject, listProperty, true, true,
                false, false)
            {
                drawElementCallback = DrawElement,
                drawElementBackgroundCallback = DrawElementBackground,
                elementHeightCallback = GetElementHeight,
                headerHeight = 25,
                showDefaultBackground = false,
                onChangedCallback = list => { property.serializedObject.ApplyModifiedProperties(); },
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y + 5, rect.width, rect.height), "Feedbacks :",
                        _labelStyle);
                    EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height, rect.width, 1), Color.gray);
                },
                drawNoneElementCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, rect.height),
                        "Not a single Feedback...");
                }
            };

            _isProSkin = EditorGUIUtility.isProSkin;

            _labelStyle ??= new GUIStyle();
            _labelStyle.fontStyle = FontStyle.Bold;
            _labelStyle.normal.textColor = _isProSkin ? Color.white : Color.black;
        }

        private void DuplicateElement(int index)
        {
            var copyItem = Activator.CreateInstance(_feedbackList.List[index].GetType()) as IFeedback;
            _feedbackList.List.Add(copyItem);
        }

        private void DeleteElement(int index)
        {
            _feedbackList.List.RemoveAt(index);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var defaultColor = !_isProSkin ? new Color(.7f, .7f, .7f) : new Color(.25f, .25f, .25f);
            var barColor = _feedbackList.List[index].BarColor;
            var label = _feedbackList.List[index].Label;

            rect.y += 2;

            if (GUI.Button(
                    new Rect(rect.x + EditorGUIUtility.currentViewWidth - 70, rect.y + 1, 20,
                        EditorGUIUtility.singleLineHeight),
                    "[-]", _labelStyle))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Duplicate"), false, () => DuplicateElement(index));
                menu.AddItem(new GUIContent("Delete"), false, () => DeleteElement(index));
                menu.ShowAsContext();
            }

            EditorGUI.Foldout(
                new Rect(rect.x + 10, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight),
                element.isExpanded, "");
            _feedbackList.List[index].IsActive =
                EditorGUI.Toggle(new Rect(rect.x + 12, rect.y - 1, 20, 20), _feedbackList.List[index].IsActive);
            EditorGUI.LabelField(new Rect(rect.x + 12, rect.y + 2, rect.width - 12, rect.height), "　　 " + label,
                _labelStyle);

            EditorGUI.PropertyField(
                !element.isExpanded
                    ? new Rect(rect.x + 28, rect.y + 3, rect.width + 13, rect.height + 4)
                    : new Rect(rect.x - 14, rect.y + 3, rect.width + 13, rect.height + 40),
                element,
                new GUIContent(""),
                true
            );


            if (element.isExpanded)
            {
                EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
                if (GUI.Button(
                        new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(element) + 12, rect.width * .5f,
                            EditorGUIUtility.singleLineHeight), "Play")) _feedbackList.List[index].Play();
                if (GUI.Button(
                        new Rect(rect.x + rect.width * .5f, rect.y + EditorGUI.GetPropertyHeight(element) + 12,
                            rect.width * .5f, EditorGUIUtility.singleLineHeight), "Stop"))
                    _feedbackList.List[index].Stop();
                EditorGUI.EndDisabledGroup();
            }

            _feedbackList.List[index].IsActive =
                EditorGUI.Toggle(new Rect(rect.x + 12, rect.y - 1, 20, 20), _feedbackList.List[index].IsActive);

            rect.x -= 20;
            const int barWidth = 5;
            var box = new Rect(rect.x - 3, rect.y - 2, barWidth, EditorGUIUtility.singleLineHeight + 4);
            var bar = new Rect(rect.x - 3, rect.y - 2, barWidth, rect.height - 1);
            var point = new Rect(rect.x + 5, rect.y + 5, 10, 1);
            var point2 = new Rect(rect.x + 5, rect.y + 9, 10, 1);
            var point3 = new Rect(rect.x + 5, rect.y + 13, 10, 1);
            GUI.DrawTexture(point, MakeTexture2D(barColor));
            GUI.DrawTexture(point2, MakeTexture2D(barColor));
            GUI.DrawTexture(point3, MakeTexture2D(barColor));
            GUI.DrawTexture(bar, MakeTexture2D(Color.Lerp(defaultColor, barColor, .2f)));
            GUI.DrawTexture(box, MakeTexture2D(barColor));
            GUI.color = Color.white;
        }

        private float GetElementHeight(int index)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (element.isExpanded)
                return EditorGUI.GetPropertyHeight(element) + 40;
            return EditorGUI.GetPropertyHeight(element) + 5;
        }

        private void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0) return;
            var defaultColor = !_isProSkin ? new Color(.7f, .7f, .7f) : new Color(.25f, .25f, .25f);
            var barColor = _feedbackList.List[index].BarColor;
            var themeColor = Color.Lerp(defaultColor, barColor, .2f);
            var backRect = new Rect(rect.x, rect.y, rect.width, rect.height - 1);
            var topLineRect = new Rect(rect.x, rect.y, rect.width, 1.5f);

            EditorGUI.DrawRect(backRect, defaultColor);
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight + 4),
                MakeTexture2D(themeColor));
            EditorGUI.DrawRect(topLineRect, Color.black * .3f);
        }

        private Texture2D MakeTexture2D(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            try
            {
                if (_feedbackList == null || _saveProperty == null ||
                    property.propertyPath != _saveProperty.propertyPath) Initialize(property);
            }
            catch (Exception)
            {
                Initialize(property);
            }

            return _reorderableList.GetHeight() + 30;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                if (_feedbackList == null || _saveProperty == null ||
                    property.propertyPath != _saveProperty.propertyPath) Initialize(property);
            }
            catch (Exception)
            {
                Initialize(property);
            }

            position.x += 2;
            property.serializedObject.Update();
            _reorderableList?.DoList(position);
            property.serializedObject.ApplyModifiedProperties();
            if (GUI.Button(
                    new Rect(position.x + (EditorGUIUtility.currentViewWidth - 250) * .5f - 5,
                        position.y + _reorderableList.GetHeight() - 10, 250, 23), new GUIContent("Add Feedback")))
            {
                var dropdown = new FeedbackDropDown(new AdvancedDropdownState());
                dropdown.OnSelect += item => _feedbackList.ProcessSelection(item.name);
                var mousePos = Event.current.mousePosition;
                dropdown.Show(new Rect(new Vector2(mousePos.x - 100, mousePos.y), new Vector2(250, 20)));
            }
        }
    }
}