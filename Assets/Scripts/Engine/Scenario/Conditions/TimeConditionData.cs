using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TimeConditionData : IConditionData
{
    public float _Duration;
    [HideInInspector]
    public float _Time;
    
    public override void Initialize(LinkData link)
    {
        base.Initialize(link);

        _Time = Time.time;
    }

    public override bool Validate()
    {
        return base.Validate() && Time.time >= _Time + _Duration;
    }
}
