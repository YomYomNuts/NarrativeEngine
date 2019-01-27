using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BackgroundDescriptorData : IDescriptorData
{
    public Color _BackgroundColor;

    public override void Read()
    {
        base.Read();

        ReaderScenario.Instance.SetBackground(_BackgroundColor);
    }
}
