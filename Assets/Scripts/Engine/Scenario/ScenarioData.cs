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

public class ScenarioData : ScriptableObject
{
    public List<PageData> _Pages = new List<PageData>();


    public ScenarioData()
    {
#if UNITY_EDITOR
        m_ShowPages = new AnimBool(true);
#endif
    }

#if UNITY_EDITOR
    [MenuItem("Narrative/ScenarioData")]
    static void createInstance()
    {
        ScriptableObjectUtility.CreateAsset<ScenarioData>();
    }

    public AnimBool m_ShowPages;
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(ScenarioData))]
class ScenarioDataEditor : Editor
{
    ScenarioData m_scenarioData;

    void OnEnable()
    {
        m_scenarioData = target as ScenarioData;
        m_scenarioData.m_ShowPages.valueChanged.AddListener(new UnityAction(base.Repaint));
    }

    public override void OnInspectorGUI()
    {
        #region Pages
        m_scenarioData.m_ShowPages.target = EditorGUILayout.ToggleLeft("Pages", m_scenarioData.m_ShowPages.target);
        using (var group = new EditorGUILayout.FadeGroupScope(m_scenarioData.m_ShowPages.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < m_scenarioData._Pages.Count; ++i)
                {
                    PageData page = m_scenarioData._Pages[i];
                    if (page == null)
                        continue;

                    page._Show = EditorGUILayout.Foldout(page._Show, page.name);
                    if (page._Show)
                    {
                        ++EditorGUI.indentLevel;
                        page.GetEditor().OnInspectorGUI();

                        var oldColor = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Remove"))
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(page));
                            m_scenarioData._Pages.RemoveAt(i);
                            --i;
                            EditorUtility.SetDirty(m_scenarioData);
                        }
                        GUI.backgroundColor = oldColor;
                        --EditorGUI.indentLevel;
                        EditorGUILayout.Space();
                    }
                }

                if (GUILayout.Button("Add Page"))
                {
                    PageData newAsset = ScriptableObjectUtility.CreateAsset<PageData>();
                    m_scenarioData._Pages.Add(newAsset);
                    EditorUtility.SetDirty(m_scenarioData);
                }
                --EditorGUI.indentLevel;
            }
        }
        #endregion Pages
    }
}
#endif
