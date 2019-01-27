using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ChangeBool
{
    SetTrue,
    SetFalse,
    Reverse,
}

public enum CheckBool
{
    IsTrue,
    IsFalse,
}

public class BoolTagData : ITagData
{
    public bool _InitialValue;
    [HideInInspector]
    public bool _Value;

    public void DoAction(ChangeBool action)
    {
        switch (action)
        {
            case ChangeBool.SetTrue:
                _Value = true;
                break;
            case ChangeBool.SetFalse:
                _Value = false;
                break;
            case ChangeBool.Reverse:
                _Value = !_Value;
                break;
        }
    }

    public bool CheckValue(CheckBool action)
    {
        switch (action)
        {
            case CheckBool.IsTrue:
                return _Value;
            case CheckBool.IsFalse:
                return !_Value;
        }
        return false;
    }

    public void Reset()
    {
        _Value = _InitialValue;
    }
}
