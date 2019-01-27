using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IntTagModifierDescriptorData : IDescriptorData
{
    [HideInInspector]
    public IntTagData _Data;
    [HideInInspector]
    public int _ToAdd = 1;
    
    public override void Read()
    {
        base.Read();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(IntTagModifierDescriptorData))]
class IntTagModifierDescriptorDataEditor : Editor
{
    IntTagModifierDescriptorData m_intTagModifier;
    int m_choiceIndex;

    void OnEnable()
    {
        m_intTagModifier = target as IntTagModifierDescriptorData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        List<IntTagData> intTags = TagsManager.Instance.GetTagsOfType<IntTagData>();
        List<string> tagsName = intTags.Select(i => i._Name).ToList();
        if (tagsName.Count == 0)
            tagsName.Add("None");

        // Find value
        m_choiceIndex = 0;
        if (m_intTagModifier._Data != null)
            m_choiceIndex = tagsName.FindIndex(tag => tag == m_intTagModifier._Data._Name);

        m_choiceIndex = EditorGUILayout.Popup("Tag", m_choiceIndex, tagsName.ToArray());
        m_intTagModifier._ToAdd = EditorGUILayout.IntField("ToAdd", m_intTagModifier._ToAdd);
        
        // Apply changement
        string newValue = tagsName[m_choiceIndex];
        if ((m_intTagModifier._Data == null && newValue != "None")
            || (m_intTagModifier._Data != null && newValue != m_intTagModifier._Data._Name))
        {
            if (newValue == "None")
            {
                m_intTagModifier._Data = null;
            }
            else
            {
                m_intTagModifier._Data = intTags.Where(tag => tag._Name == newValue).First();
            }
            EditorUtility.SetDirty(m_intTagModifier);
        }
    }
}
#endif
