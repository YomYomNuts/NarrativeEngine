using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArduinoMessageDescriptorData : IDescriptorData
{
    public string _Text;
    
    public override void Read()
    {
        base.Read();

    }
}
