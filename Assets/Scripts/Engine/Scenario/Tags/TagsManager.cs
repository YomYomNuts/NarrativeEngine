using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
#endif

public class TagsManager : ScriptableObject
{
    #region Singleton
    private static TagsManager _instance = null;
    public static TagsManager Instance
    {
        get
        {
            if (TagsManager._instance == null)
            {
                TagsManager[] tags = Resources.FindObjectsOfTypeAll<TagsManager>();
                TagsManager._instance = tags.FirstOrDefault();
            } 
            return TagsManager._instance;
        }
    }
    #endregion Singleton

    public List<ITagData> _Tags = new List<ITagData>();


    public TagsManager()
    {
#if UNITY_EDITOR
        m_ShowTags = new AnimBool(true);
#endif
    }

    #region Helper
    public List<T> GetTagsOfType<T>() where T : ITagData
    {
        List<T> objects = new List<T>();
        foreach (ITagData tag in _Tags)
        {
            if (tag is T)
                objects.Add(tag as T);
        }
        return objects;
    }
    #endregion Helper

    #region Editor
#if UNITY_EDITOR
    [MenuItem("Narrative/TagsManager")]
    static void createInstance()
    {
        ScriptableObjectUtility.CreateAsset<TagsManager>();
    }

    public AnimBool m_ShowTags;
#endif
    #endregion Editor
}


#if UNITY_EDITOR
[CustomEditor(typeof(TagsManager))]
class TagsManagerEditor : Editor
{
    List<string> _ListTypeTag;

    void OnEnable()
    {
        TagsManager.Instance.m_ShowTags.valueChanged.AddListener(new UnityAction(base.Repaint));
        _ListTypeTag = ReflectiveEnumerator.GetEnumerableOfType<ITagData>();
    }

    public override void OnInspectorGUI()
    {
        #region Tags
        TagsManager.Instance.m_ShowTags.target = EditorGUILayout.ToggleLeft("Tags", TagsManager.Instance.m_ShowTags.target);
        using (var group = new EditorGUILayout.FadeGroupScope(TagsManager.Instance.m_ShowTags.faded))
        {
            if (group.visible)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < TagsManager.Instance._Tags.Count; ++i)
                {
                    ITagData tag = TagsManager.Instance._Tags[i];
                    {
                        Editor _editor = Editor.CreateEditor(tag);
                        _editor.OnInspectorGUI();
                    }

                    var oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(tag, true);
                        TagsManager.Instance._Tags.Remove(tag);
                        --i;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(TagsManager.Instance));
                    }
                    GUI.backgroundColor = oldColor;
                    EditorGUILayout.Space();
                }

                foreach (string type in _ListTypeTag)
                {
                    if (GUILayout.Button("Add " + type))
                    {
                        string currentPath = AssetDatabase.GetAssetPath(TagsManager.Instance);
                        ITagData newAsset = ScriptableObject.CreateInstance(type) as ITagData;
                        AssetDatabase.AddObjectToAsset(newAsset, currentPath);
                        TagsManager.Instance._Tags.Add(newAsset);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
                        EditorUtility.SetDirty(TagsManager.Instance);
                    }
                }
                --EditorGUI.indentLevel;
            }
        }
        #endregion Tags
    }
}
#endif
