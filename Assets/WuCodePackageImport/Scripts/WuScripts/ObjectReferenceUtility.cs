using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectReferenceUtility
{
    public const string NoObjectOfTypeInSceneFormat = "No object(s) type {0} in scene!";
    public const string FoundTypeRefInSceneFormat = "Found first instance of {0} in scene named {1}.";
    public const string FoundTypeRefsInSceneFormat = "Found instance(s) of {0} in scene (count: {1}).";

    #region MANAGE UNITY RUNTIOME ERRORS
    private static bool isQuitting;
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        Application.quitting += Quit;
    }
    public static void Quit()
    {
        isQuitting = true;
    }
    #endregion

    public static Type FindReferenceIfNull<Type>(ref Type obj, bool logWarnings = true, bool logErrors = true) where Type : Object
    {
        if (obj == null)
        {
            obj = Object.FindObjectOfType<Type>();

            string typeName = typeof(Type).Name;
            if (obj == null && !isQuitting)
            {
                if (logErrors)
                    Debug.LogErrorFormat(NoObjectOfTypeInSceneFormat, typeName);
                if (logWarnings)
                    Debug.LogWarningFormat(FoundTypeRefInSceneFormat, typeName, obj.name);
            }
        }

        return obj;
    }
    public static Type[] FindReferencesIfNull<Type>(ref Type[] obj, bool logWarnings = true, bool logErrors = true) where Type : Object
    {
        if (obj == null)
        {
            obj = Object.FindObjectsOfType<Type>();

            string typeName = typeof(Type).Name;
            if (obj == null && !isQuitting)
            {
                if (logErrors)
                    Debug.LogErrorFormat(NoObjectOfTypeInSceneFormat, typeName);
                if (logWarnings)
                    Debug.LogWarningFormat(FoundTypeRefsInSceneFormat, typeName, obj.Length);
            }
        }

        return obj;
    }
}
