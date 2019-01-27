using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MusicDescriptorData : IDescriptorData
{
    public AudioClip _Sound;
    
    public override void Read()
    {
        base.Read();

        ReaderScenario.Instance.ChangeMusic(_Sound);
    }

    public override void Unload()
    {
        base.Unload();

        ReaderScenario.Instance.ChangeMusic(null);
    }
}
