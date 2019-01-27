using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
#endif

public class PageData : ScriptableObject
{
    public List<IDescriptorData> _Descriptors = new List<IDescriptorData>();
    public List<IConditionData> _Conditions = new List<IConditionData>();


    public PageData()
    {
#if UNITY_EDITOR
        _Show = true;
        m_ShowDescriptors = new AnimBool(true);
        m_ShowConditions = new AnimBool(false);
#endif
    }

    #region Helpers
    public void Read()
    {
        foreach (IDescriptorData descriptor in _Descriptors)
        {
            descriptor.Read();
        }
        foreach (IConditionData condition in _Conditions)
        {
            condition.Initialize();
        }
    }

    public void Unload()
    {
        foreach (IDescriptorData descriptor in _Descriptors)
        {
            descriptor.Unload();
        }
    }

    public void Update()
    {
        foreach (IConditionData condition in _Conditions)
        {
            if (condition.Check())
                break;
        }
    }
    #endregion Helpers

#if UNITY_EDITOR
    [System.NonSerialized]
    private Editor _editor;
    public Editor GetEditor()
    {
        if (_editor == null)
            _editor = Editor.CreateEditor(this);
        return _editor;
    }
    [HideInInspector]
    public bool _Show;
    [HideInInspector]
    public AnimBool m_ShowDescriptors;
    [HideInInspector]
    public AnimBool m_ShowConditions;
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(PageData))]
class PageDataEditor : Editor
{
    PageData m_pageData;
    List<string> _ListTypeDescriptors;
    List<string> _ListTypeConditions;

    void OnEnable()
    {
        m_pageData = target as PageData;
        m_pageData.m_ShowDescriptors.valueChanged.AddListener(new UnityAction(base.Repaint));
        _ListTypeDescriptors = ReflectiveEnumerator.GetEnumerableOfType<IDescriptorData>();
        m_pageData.m_ShowConditions.valueChanged.AddListener(new UnityAction(base.Repaint));
        _ListTypeConditions = ReflectiveEnumerator.GetEnumerableOfType<IConditionData>();
    }

    public override void OnInspectorGUI()
    {
        // Name
        string prevName = m_pageData.name;
        m_pageData.name = EditorGUILayout.TextField("Name", prevName);
        if (prevName != m_pageData.name)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(m_pageData), m_pageData.name);
        }

        #region Descriptors
        m_pageData.m_ShowDescriptors.target = EditorGUILayout.ToggleLeft("Descriptors", m_pageData.m_ShowDescriptors.target);
        using (var group = new EditorGUILayout.FadeGroupScope(m_pageData.m_ShowDescriptors.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < m_pageData._Descriptors.Count; ++i)
                {
                    IDescriptorData descriptor = m_pageData._Descriptors[i];
                    {
                        Editor _editor = Editor.CreateEditor(descriptor);
                        _editor.OnInspectorGUI();
                    }

                    var oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(descriptor, true);
                        m_pageData._Descriptors.Remove(descriptor);
                        --i;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(m_pageData));
                    }
                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.Space();
                }
                
                foreach (string type in _ListTypeDescriptors)
                {
                    if (GUILayout.Button("Add " + type))
                    {
                        string currentPath = AssetDatabase.GetAssetPath(m_pageData);
                        IDescriptorData newAsset = ScriptableObject.CreateInstance(type) as IDescriptorData;
                        AssetDatabase.AddObjectToAsset(newAsset, currentPath);
                        m_pageData._Descriptors.Add(newAsset);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
                        EditorUtility.SetDirty(m_pageData);
                    }
                }
                --EditorGUI.indentLevel;
                EditorGUILayout.Space();
            }
        }
        #endregion Descriptors

        #region Conditions
        m_pageData.m_ShowConditions.target = EditorGUILayout.ToggleLeft("Conditions", m_pageData.m_ShowConditions.target);
        using (var group = new EditorGUILayout.FadeGroupScope(m_pageData.m_ShowConditions.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < m_pageData._Conditions.Count; ++i)
                {
                    IConditionData condition = m_pageData._Conditions[i];
                    {
                        Editor _editor = Editor.CreateEditor(condition);
                        _editor.OnInspectorGUI();
                    }

                    var oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(condition, true);
                        m_pageData._Conditions.Remove(condition);
                        --i;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(m_pageData));
                    }
                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.Space();
                }
                
                foreach (string type in _ListTypeConditions)
                {
                    if (GUILayout.Button("Add " + type))
                    {
                        string currentPath = AssetDatabase.GetAssetPath(m_pageData);
                        IConditionData newAsset = ScriptableObject.CreateInstance(type) as IConditionData;
                        AssetDatabase.AddObjectToAsset(newAsset, currentPath);
                        m_pageData._Conditions.Add(newAsset);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
                        EditorUtility.SetDirty(m_pageData);
                    }
                }
                --EditorGUI.indentLevel;
                EditorGUILayout.Space();
            }
        }
        #endregion Descriptors
    }
}
#endif
