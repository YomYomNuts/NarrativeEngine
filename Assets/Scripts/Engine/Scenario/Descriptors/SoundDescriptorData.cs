using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundDescriptorData : IDescriptorData
{
    public AudioClip _Sound;
    
    public override void Read()
    {
        base.Read();

        ReaderScenario.Instance.ChangeFX(_Sound);
    }

    public override void Unload()
    {
        base.Unload();

        ReaderScenario.Instance.ChangeFX(null);
    }
}
