using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GMessage.Editor
{
    [Serializable]
    public class Node
    {
        public string _comment;
        public string _name;
    }

    public class GenerateAUniqueID : EditorWindow
    {
        private static Regex _IDNameRegex = new Regex("[^A-Z_]");
        private static string _filePath;

        private List<Node> _list;

        private Vector2 _tempScrollPos;
        private static readonly Color DefaultColor = GUI.backgroundColor;

        [MenuItem("Framework/生成新的ID")]
        static void Init()
        {
            var window = GetWindow<GenerateAUniqueID>();
            window.titleContent = new GUIContent("生成新的ID");
            window.Show();
        }

        void OnEnable()
        {
            _list = new List<Node>();
        }

        void OnGUI()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                EditorGUILayout.LabelField("请选择文件位置");
                if (GUILayout.Button("选择文件"))
                {
                    _filePath = EditorUtility.OpenFilePanel("cs文件", Application.dataPath + "/Scripts/Framework/Define", "cs");
                }
                return;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.SelectableLabel(_filePath, EditorStyles.whiteLargeLabel);
            if (GUILayout.Button("选择文件", GUILayout.Width(64f)))
            {
                _filePath = EditorUtility.OpenFilePanel("cs文件", Application.dataPath + "/Scripts/Framework/Define", "cs");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();


            int id2bDelete = -1;
            _tempScrollPos = EditorGUILayout.BeginScrollView(_tempScrollPos);
            for (int i = 0; i < _list.Count; i++)
            {
                var node = _list[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("注释", GUILayout.Width(64));
                GUI.backgroundColor = new Color(0.22745f, 0.235294f, 0.10196f);
                node._comment = EditorGUILayout.TextArea(node._comment, EditorStyles.whiteBoldLabel);
                GUI.backgroundColor = DefaultColor;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID名称", GUILayout.Width(64));
                node._name = EditorGUILayout.TextField(node._name);
                if (!string.IsNullOrEmpty(node._name))
                {
                    node._name = _IDNameRegex.Replace(node._name, string.Empty);
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("删除"))
                {
                    id2bDelete = i;
                }
            }
            EditorGUILayout.EndScrollView();
            if (id2bDelete != -1)
            {
                _list.RemoveAt(id2bDelete);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加", EditorStyles.miniButtonLeft))
            {
                _list.Add(new Node());
            }

            if (GUILayout.Button("生成", EditorStyles.miniButtonRight))
            {
                Create();
            }

            EditorGUILayout.EndHorizontal();
        }

        void Create()
        {
            string newString = string.Empty;
            for (int i = 0; i < _list.Count; i++)
            {
                DateTime dt = DateTime.UtcNow.AddMilliseconds(-100 * i);
                var node = _list[i];
                newString += string.Format(
                    "\r\n\r\n" +
                    "       /// <summary>\r\n" +
                    "       /// {0}\r\n" +
                    "       /// </summary>\r\n" +
                    "       [System.ComponentModel.Description(\"{0}\")]\r\n" +
                    "       public const int {1} = {2};\r\n"
                    , node._comment, node._name, (int)dt.ToBinary());
            }
            if (!File.Exists(_filePath))
            {
                Debug.LogWarningFormat("{0}不存在，请先创建文件", _filePath);
            }
            else
            {
                string s;
                try
                {
                    using (StreamReader sr = new StreamReader(_filePath, Encoding.UTF8))
                    {
                        s = sr.ReadToEnd();
                        int index = FindIndexToInsert(s);
                        if (index > -1)
                        {
                            s = s.Insert(index, newString);
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(_filePath))
                    {
                        sw.Write(s);
                        sw.Flush();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            AssetDatabase.Refresh();

            Close();
        }

        private int FindIndexToInsert(string s)
        {
            int id = s.LastIndexOf("class", StringComparison.Ordinal);
            if (id < 0) { return id; }

            id = s.IndexOf("{", id, StringComparison.Ordinal);
            if (id < 0) { return id; }
            return id + 1;
        }
    }
}
