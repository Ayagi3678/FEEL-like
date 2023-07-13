using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GetCondFunc =
    System.Func<UnityEditor.SerializedProperty, MMCFeedbacks.Attribute.ConditionalDisableAttribute, bool>;

namespace MMCFeedbacks.Attribute
{
    [CustomPropertyDrawer(typeof(ConditionalDisableAttribute))]
    internal sealed class ConditionalDisableDrawer : PropertyDrawer
    {
        private readonly Dictionary<Type, GetCondFunc> DisableCondFuncMap = new()
        {
            { typeof(bool), (prop, attr) => attr.TrueThenDisable ? !prop.boolValue : prop.boolValue },
            {
                typeof(string),
                (prop, attr) => attr.TrueThenDisable
                    ? prop.stringValue == attr.ComparedStr
                    : prop.stringValue != attr.ComparedStr
            },
            {
                typeof(int),
                (prop, attr) => attr.TrueThenDisable
                    ? prop.intValue == attr.ComparedInt
                    : prop.intValue != attr.ComparedInt
            },
            {
                typeof(float),
                (prop, attr) => attr.TrueThenDisable
                    ? prop.floatValue <= attr.ComparedFloat
                    : prop.floatValue > attr.ComparedFloat
            }
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ConditionalDisableAttribute;
            var pro = property.serializedObject.GetIterator();
            SerializedProperty condProp = null;
            var trimString = "[]".ToCharArray();
            while (pro.NextVisible(true))
            {
                if (pro.name != attr.VariableName) continue;
                if (property.propertyPath.Split(trimString)[1] != pro.propertyPath.Split(trimString)[1]) continue;
                condProp = pro;
                break;
            }

            if (condProp == null)
            {
                Debug.LogError($"Not found '{attr.VariableName}' property");
                EditorGUI.PropertyField(position, property, label, true);
            }

            var isDisable = IsDisable(attr, condProp);
            if (attr.ConditionalInvisible && isDisable) return;
            EditorGUI.BeginDisabledGroup(isDisable);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ConditionalDisableAttribute;
            var pro = property.serializedObject.GetIterator();
            SerializedProperty prop = null;
            var trimString = "[]".ToCharArray();
            while (pro.NextVisible(true))
            {
                if (pro.name != attr.VariableName) continue;
                if (property.propertyPath.Split(trimString)[1] != pro.propertyPath.Split(trimString)[1]) continue;
                prop = pro;
                break;
            }

            if (attr.ConditionalInvisible && IsDisable(attr, prop)) return -EditorGUIUtility.standardVerticalSpacing;
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private bool IsDisable(ConditionalDisableAttribute attr, SerializedProperty prop)
        {
            GetCondFunc disableCondFunc;
            if (!DisableCondFuncMap.TryGetValue(attr.VariableType, out disableCondFunc))
            {
                Debug.LogError($"{attr.VariableType} type is not supported");
                return false;
            }

            //Debug.Log(prop);
            return disableCondFunc(prop, attr);
        }
    }
}