using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DatabaseLoader : ScriptableObject
{

#if UNITY_EDITOR
    protected virtual IEnumerator DataUpdateCor(string docID, string gid, UnityAction<string> updateMethod)
    {
        if ((object)updateMethod == null) yield break;

        UnityWebRequest www = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{docID}/export?format=csv&gid={gid}");

        yield return www.SendWebRequest();

        if (www.result.Equals(UnityWebRequest.Result.ConnectionError) ||
            www.result.Equals(UnityWebRequest.Result.ProtocolError))
        {
            yield break;
        }

        updateMethod(www.downloadHandler.text);
    }
#endif
}
