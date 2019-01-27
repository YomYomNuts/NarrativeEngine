using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IDescriptorData : ScriptableObject
{
    public void OnValidate()
    {
        ReaderScenario.Instance.ReadAgain();
    }

    public virtual void Read()
    {
    }

    public virtual void Unload()
    {
    }
}
