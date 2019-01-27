using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IntTagConditionData : IConditionData
{
    [HideInInspector]
    public IntTagData _Data;
    [HideInInspector]
    public CheckInt _Action;
    [HideInInspector]
    public int _Value;

    #region Helpers
    public override bool Validate()
    {
        return base.Validate() && _Data.CheckValue(_Action, _Value);
    }
    #endregion Helpers
}

#if UNITY_EDITOR
[CustomEditor(typeof(IntTagConditionData))]
class IntTagConditionDataEditor : Editor
{
    IntTagConditionData m_intTagCondition;
    int m_choiceIndex;

    void OnEnable()
    {
        m_intTagCondition = target as IntTagConditionData;
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
        if (m_intTagCondition._Data != null)
            m_choiceIndex = tagsName.FindIndex(tag => tag == m_intTagCondition._Data._Name);

        m_choiceIndex = EditorGUILayout.Popup("Tag", m_choiceIndex, tagsName.ToArray());
        m_intTagCondition._Action = (CheckInt)EditorGUILayout.EnumPopup("Action", m_intTagCondition._Action);
        m_intTagCondition._Value = EditorGUILayout.IntField("Value Test", m_intTagCondition._Value);
        
        // Apply changement
        string newValue = tagsName[m_choiceIndex];
        if ((m_intTagCondition._Data == null && newValue != "None")
            || (m_intTagCondition._Data != null && newValue != m_intTagCondition._Data._Name))
        {
            if (newValue == "None")
            {
                m_intTagCondition._Data = null;
            }
            else
            {
                m_intTagCondition._Data = intTags.Where(tag => tag._Name == newValue).First();
            }
            EditorUtility.SetDirty(m_intTagCondition);
        }
    }
}
#endif
