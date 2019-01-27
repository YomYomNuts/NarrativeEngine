using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoolTagModifierDescriptorData : IDescriptorData
{
    [HideInInspector]
    public BoolTagData _Data;
    [HideInInspector]
    public ChangeBool _Action;
    
    public override void Read()
    {
        base.Read();

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BoolTagModifierDescriptorData))]
class BoolTagModifierDescriptorDataEditor : Editor
{
    BoolTagModifierDescriptorData m_boolTagModifier;
    int m_choiceIndex;

    void OnEnable()
    {
        m_boolTagModifier = target as BoolTagModifierDescriptorData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        List<BoolTagData> boolTags = TagsManager.Instance.GetTagsOfType<BoolTagData>();
        List<string> tagsName = boolTags.Select(i => i._Name).ToList();
        if (tagsName.Count == 0)
            tagsName.Add("None");

        // Find value
        m_choiceIndex = 0;
        if (m_boolTagModifier._Data != null)
            m_choiceIndex = tagsName.FindIndex(tag => tag == m_boolTagModifier._Data._Name);

        m_choiceIndex = EditorGUILayout.Popup("Tag", m_choiceIndex, tagsName.ToArray());
        m_boolTagModifier._Action = (ChangeBool)EditorGUILayout.EnumPopup("Action", m_boolTagModifier._Action);
        
        // Apply changement
        string newValue = tagsName[m_choiceIndex];
        if ((m_boolTagModifier._Data == null && newValue != "None")
            || (m_boolTagModifier._Data != null && newValue != m_boolTagModifier._Data._Name))
        {
            if (newValue == "None")
            {
                m_boolTagModifier._Data = null;
            }
            else
            {
                m_boolTagModifier._Data = boolTags.Where(tag => tag._Name == newValue).First();
            }
            EditorUtility.SetDirty(m_boolTagModifier);
        }
    }
}
#endif
