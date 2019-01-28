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
    [HideInInspector]
    public List<IDescriptorData> _Descriptors = new List<IDescriptorData>();
    [HideInInspector]
    public List<LinkData> _Links = new List<LinkData>();


    public PageData()
    {
#if UNITY_EDITOR
        _Show = true;
        m_ShowDescriptors = new AnimBool(true);
        m_ShowLinks = new AnimBool(false);
#endif
    }

    #region Helpers
    public void Read()
    {
        foreach (IDescriptorData descriptor in _Descriptors)
        {
            descriptor.Read();
        }
        foreach (LinkData link in _Links)
        {
            link.Initialize();
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
        foreach (LinkData link in _Links)
        {
            if (link.Check())
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
    public AnimBool m_ShowLinks;
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(PageData))]
class PageDataEditor : Editor
{
    PageData m_pageData;
    List<string> _ListTypeDescriptors;
    List<string> _ListTypeLinks;

    void OnEnable()
    {
        m_pageData = target as PageData;
        m_pageData.m_ShowDescriptors.valueChanged.AddListener(new UnityAction(base.Repaint));
        _ListTypeDescriptors = ReflectiveEnumerator.GetEnumerableOfType<IDescriptorData>();
        m_pageData.m_ShowLinks.valueChanged.AddListener(new UnityAction(base.Repaint));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

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

        #region Links
        m_pageData.m_ShowLinks.target = EditorGUILayout.ToggleLeft("Links", m_pageData.m_ShowLinks.target);
        using (var group = new EditorGUILayout.FadeGroupScope(m_pageData.m_ShowLinks.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < m_pageData._Links.Count; ++i)
                {
                    LinkData link = m_pageData._Links[i];
                    {
                        Editor _editor = Editor.CreateEditor(link);
                        _editor.OnInspectorGUI();
                    }

                    var oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(link, true);
                        m_pageData._Links.Remove(link);
                        --i;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(m_pageData));
                    }
                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.Space();
                }
                
                if (GUILayout.Button("Add Link"))
                {
                    string currentPath = AssetDatabase.GetAssetPath(m_pageData);
                    LinkData newAsset = ScriptableObject.CreateInstance<LinkData>();
                    AssetDatabase.AddObjectToAsset(newAsset, currentPath);
                    m_pageData._Links.Add(newAsset);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
                    EditorUtility.SetDirty(m_pageData);
                }
                --EditorGUI.indentLevel;
                EditorGUILayout.Space();
            }
        }
        #endregion Links

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
