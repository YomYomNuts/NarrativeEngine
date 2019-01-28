using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
#endif

public class LinkData : ScriptableObject
{
    public string _Name;
    public PageData _NextPage;
    [HideInInspector]
    public List<IConditionData> _Conditions = new List<IConditionData>();

    public LinkData()
    {
#if UNITY_EDITOR
        m_ShowConditions = new AnimBool(false);
#endif
    }

    #region Helpers
    public bool Check()
    {
        if (_NextPage != null)
        {
            foreach (IConditionData condition in _Conditions)
            {
                if (!condition.Validate())
                    return false;
            }
            ReaderScenario.Instance.ChangePage(_NextPage);
            return true;
        }
        return false;
    }

    public virtual void Initialize()
    {
        foreach (IConditionData condition in _Conditions)
        {
            condition.Initialize(this);
        }
    }
    #endregion Helpers

#if UNITY_EDITOR
    [HideInInspector]
    public AnimBool m_ShowConditions;
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(LinkData))]
class LinkDataEditor : Editor
{
    LinkData m_linkData;
    List<string> _ListTypeConditions;

    void OnEnable()
    {
        m_linkData = target as LinkData;
        m_linkData.m_ShowConditions.valueChanged.AddListener(new UnityAction(base.Repaint));
        _ListTypeConditions = ReflectiveEnumerator.GetEnumerableOfType<IConditionData>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        #region Conditions
        m_linkData.m_ShowConditions.target = EditorGUILayout.ToggleLeft("Conditions", m_linkData.m_ShowConditions.target);
        using (var group = new EditorGUILayout.FadeGroupScope(m_linkData.m_ShowConditions.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < m_linkData._Conditions.Count; ++i)
                {
                    IConditionData condition = m_linkData._Conditions[i];
                    {
                        Editor _editor = Editor.CreateEditor(condition);
                        _editor.OnInspectorGUI();
                    }

                    var oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(condition, true);
                        m_linkData._Conditions.Remove(condition);
                        --i;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(m_linkData));
                    }
                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.Space();
                }
                
                foreach (string type in _ListTypeConditions)
                {
                    if (GUILayout.Button("Add " + type))
                    {
                        string currentPath = AssetDatabase.GetAssetPath(m_linkData);
                        IConditionData newAsset = ScriptableObject.CreateInstance(type) as IConditionData;
                        AssetDatabase.AddObjectToAsset(newAsset, currentPath);
                        m_linkData._Conditions.Add(newAsset);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
                        EditorUtility.SetDirty(m_linkData);
                    }
                }
                --EditorGUI.indentLevel;
                EditorGUILayout.Space();
            }
        }
        #endregion Conditions
    }
}
#endif