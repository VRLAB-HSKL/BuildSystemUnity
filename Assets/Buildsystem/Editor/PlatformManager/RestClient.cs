using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 
/// </summary>
public class RestClient
{
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator GetFromURL(String uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Request Error: " + request.error);
            }
            else
            {
                string jsonString = request.downloadHandler.text;
                Debug.Log(jsonString);
                PlatformData data = JsonUtility.FromJson<PlatformData>(jsonString);
                Debug.Log(data.configurationName);

                //PlatformDataRoot dataRoot = JsonUtility.FromJson<PlatformDataRoot>(jsonString);
            }
        }
    }

    
}
