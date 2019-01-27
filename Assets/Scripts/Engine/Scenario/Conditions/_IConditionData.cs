using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IConditionData : ScriptableObject
{
    public string _Name;
    public PageData _NextPage;

    #region Helpers
    public bool Check()
    {
        if (_NextPage != null && Validate())
        {
            ReaderScenario.Instance.ChangePage(_NextPage);
            return true;
        }
        return false;
    }

    public virtual void Initialize()
    {
    }

    public virtual bool Validate()
    {
        return true;
    }
    #endregion Helpers
}


#if UNITY_EDITOR
[CustomEditor(typeof(IConditionData))]
class IConditionDataEditor : Editor
{
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif
