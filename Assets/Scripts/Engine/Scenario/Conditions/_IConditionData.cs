using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IConditionData : ScriptableObject
{
    #region Helpers
    public virtual void Initialize(LinkData link)
    {
    }

    public virtual bool Validate()
    {
        return true;
    }
    #endregion Helpers
}
