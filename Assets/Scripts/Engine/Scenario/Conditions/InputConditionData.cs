using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputConditionData : IConditionData
{
    public KeyCode _Input;

    public override void Initialize(LinkData link)
    {
        base.Initialize(link);

        switch (_Input)
        {
            case KeyCode.UpArrow:
            ReaderScenario.Instance.SetUp(link._Name);
            break;
            case KeyCode.DownArrow:
            ReaderScenario.Instance.SetDown(link._Name);
            break;
            case KeyCode.RightArrow:
            ReaderScenario.Instance.SetRight(link._Name);
            break;
            case KeyCode.LeftArrow:
            ReaderScenario.Instance.SetLeft(link._Name);
            break;
            case KeyCode.Space:
            ReaderScenario.Instance.SetButton(link._Name);
            break;
        }
    }
    
    public override bool Validate()
    {
        KeyCode newInput = _Input;
        switch (newInput)
        {
            case KeyCode.UpArrow:
            newInput = KeyCode.Z;
            break;
            case KeyCode.DownArrow:
            newInput = KeyCode.D;
            break;
            case KeyCode.RightArrow:
            newInput = KeyCode.S;
            break;
            case KeyCode.LeftArrow:
            newInput = KeyCode.Q;
            break;
        }

        return base.Validate() && Input.GetKeyDown(newInput);
    }
}
