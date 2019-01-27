using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoolTagConditionData : IConditionData
{
    [HideInInspector]
    public BoolTagData _Data;
    [HideInInspector]
    public CheckBool _Action;

    #region Helpers
    public override bool Validate()
    {
        return base.Validate() && _Data.CheckValue(_Action);
    }
    #endregion Helpers
}

#if UNITY_EDITOR
[CustomEditor(typeof(BoolTagConditionData))]
class BoolTagConditionDataEditor : Editor
{
    BoolTagConditionData m_boolTagCondition;
    int m_choiceIndex;

    void OnEnable()
    {
        m_boolTagCondition = target as BoolTagConditionData;
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
        if (m_boolTagCondition._Data != null)
            m_choiceIndex = tagsName.FindIndex(tag => tag == m_boolTagCondition._Data._Name);

        m_choiceIndex = EditorGUILayout.Popup("Tag", m_choiceIndex, tagsName.ToArray());
        m_boolTagCondition._Action = (CheckBool)EditorGUILayout.EnumPopup("Action", m_boolTagCondition._Action);
        
        // Apply changement
        string newValue = tagsName[m_choiceIndex];
        if ((m_boolTagCondition._Data == null && newValue != "None")
            || (m_boolTagCondition._Data != null && newValue != m_boolTagCondition._Data._Name))
        {
            if (newValue == "None")
            {
                m_boolTagCondition._Data = null;
            }
            else
            {
                m_boolTagCondition._Data = boolTags.Where(tag => tag._Name == newValue).First();
            }
            EditorUtility.SetDirty(m_boolTagCondition);
        }
    }
}
#endif
