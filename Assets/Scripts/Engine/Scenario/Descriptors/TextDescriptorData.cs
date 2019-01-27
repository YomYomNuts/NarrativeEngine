using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextDescriptorData : IDescriptorData
{
    public string _Text;
    public Color _TextColor = Color.white;
    
    public override void Read()
    {
        base.Read();

        string formatText = _Text;
        formatText = formatText.Replace("\\n", "\n");
        formatText = formatText.Replace("\n ", "\n");
        ReaderScenario.Instance.SetText(formatText, _TextColor);
    }
}
