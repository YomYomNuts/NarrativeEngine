using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageDescriptorData : IDescriptorData
{
    public Sprite _Sprite;
    
    public override void Read()
    {
        base.Read();

        ReaderScenario.Instance.SetImage(_Sprite);
    }

    public override void Unload()
    {
        base.Unload();

        ReaderScenario.Instance.SetImage(null);
    }
}
