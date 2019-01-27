using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum CheckInt
{
    Equals,
    NotEquals,
    Superior,
    EqualsSuperior,
    Inferior,
    EqualsInferior,
}

public class IntTagData : ITagData
{
    public int _InitialValue;
    [HideInInspector]
    public int _Value;

    public void Add(int additionalValue)
    {
        _Value += additionalValue;
    }

    public bool CheckValue(CheckInt action, int valueTest)
    {
        switch (action)
        {
            case CheckInt.Equals:
                return _Value == valueTest;
            case CheckInt.NotEquals:
                return _Value != valueTest;
            case CheckInt.Superior:
                return _Value > valueTest;
            case CheckInt.EqualsSuperior:
                return _Value >= valueTest;
            case CheckInt.Inferior:
                return _Value < valueTest;
            case CheckInt.EqualsInferior:
                return _Value <= valueTest;
        }
        return false;
    }

    public void Reset()
    {
        _Value = _InitialValue;
    }
}
