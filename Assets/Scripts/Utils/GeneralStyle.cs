using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneralStyle : ScriptableObject
{
    #region Singleton
    private static GeneralStyle _instance = null;
    public static GeneralStyle Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<GeneralStyle>().FirstOrDefault();
            return _instance;
        }
    }
    #endregion Singleton

    public GUIStyle redButton = new GUIStyle(GUI.skin.button);
}
