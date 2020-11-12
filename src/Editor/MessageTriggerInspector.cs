using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GFramework.Editor.Inspectors
{
    [CustomEditor(typeof(MessageTrigger))]
    public class MessageTriggerInspector : UnityEditor.Editor
    {
        private int[] _moduleIDArray;
        private string[] _moduleNameArray;

        private int[] _messageIDArray;
        private string[] _messageNameArray;

        private void OnEnable()
        {
            var fields = typeof(ModuleID).GetFields(BindingFlags.Public | BindingFlags.Static);

            _moduleIDArray = new int[fields.Length];
            _moduleNameArray = new string[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                _moduleIDArray[i] = (int)fields[i].GetRawConstantValue();
                _moduleNameArray[i] = fields[i].Name;
            }

            fields = typeof(DefinedID).GetFields(BindingFlags.Public | BindingFlags.Static);

            _messageIDArray = new int[fields.Length];
            _messageNameArray = new string[fields.Length + 1];

            _messageNameArray[0] = "None";
            for (int i = 0; i < fields.Length; i++)
            {
                _messageIDArray[i] = (int)fields[i].GetRawConstantValue();
                _messageNameArray[i+1] = fields[i].Name;
            }
        }

        public override void OnInspectorGUI()
        {
            var trigger = target as MessageTrigger;
            var flag = Array.IndexOf(_moduleIDArray, trigger.moduleID);

            flag = EditorGUILayout.Popup("Modules", flag, _moduleNameArray);
            trigger.moduleID = _moduleIDArray[flag];

            EditorGUILayout.BeginVertical();

            for (int i = 0; i < trigger.Triggers.Count; i++)
            {
                var msg = trigger.Triggers[i];

                EditorGUILayout.BeginHorizontal();

                msg.type = (EventTriggerType)EditorGUILayout.EnumPopup(msg.type);

                flag = Array.IndexOf(_messageIDArray, msg.id);
                flag = EditorGUILayout.Popup(flag + 1, _messageNameArray);
                if (flag != 0) { msg.id = _messageIDArray[flag - 1]; }
                else { msg.id = 0; }

                msg.content = EditorGUILayout.TextField(msg.content);

                if (!msg.Equals(trigger.Triggers[i]))
                {
                    Undo.RecordObject(trigger, "change trigger");
                    trigger.Triggers[i] = msg;
                    EditorUtility.SetDirty(trigger);
                }

                if(GUILayout.Button("-", EditorStyles.miniButton))
                {
                    Undo.RecordObject(trigger, "remove trigger");
                    trigger.Triggers.RemoveAt(i);
                    EditorUtility.SetDirty(trigger);
                }

                EditorGUILayout.EndHorizontal();

            }

            if (GUILayout.Button("+"))
            {
                Undo.RecordObject(trigger, "add trigger");
                trigger.Triggers.Add(new MessageTrigger.MessageData());
                EditorUtility.SetDirty(trigger);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
