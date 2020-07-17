using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoadWindow : EditorWindow
{

    private string loadConfigId;
    private int index;
    private string[] showLoadedConfigs;
    private string buildsystemUri = "http://localhost:8080/api/unity/getplatformconfigurationbyid";
    private List<PlatformData> platformDatas;
    private bool updateList;
    private PlatformDataManager PlatformDataManager;

    /// <summary>
    /// Setter for <see cref="SceneConfManager"/> SceneConfManager
    /// </summary>
    /// <param name="platformDataManager"></param>
    public void SetDataManager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    private void OnEnable()
    {
        updateList = false;
        showLoadedConfigs = new string[10];
        platformDatas = new List<PlatformData>();
    }

    private void OnGUI()
    {
        ShowLoadWindow();
        UpdateList();
    }

    private void UpdateList()
    {
        if (updateList)
        {
            PlatformData[] dataArray = platformDatas.ToArray();
            string[] configNameArray = new string[platformDatas.Count];

            for (int i = 0; i < platformDatas.Count; i++)
            {
                configNameArray[i] = dataArray[i].configurationName;
            }

            this.showLoadedConfigs = configNameArray;

            this.updateList = false;
        }
    }

    private void ShowLoadWindow()
    {
        GUI.Box(new Rect(0, 0, 320, 45), "");
        GUILayout.BeginArea(new Rect(0, 5, 250, 250));
        GUILayout.Label("Choose Configurations:");
        if (GUI.Button(new Rect(150, 0, 100, 20), "Open WebApp"))
        {
            Application.OpenURL("http://localhost:3000/unityconfigs");
        }
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 50, 320, 75), "");
        GUILayout.BeginArea(new Rect(0, 50, 250, 250));
        GUILayout.Label("Load Configuration:");
        loadConfigId = EditorGUILayout.TextField("Config. ID:", loadConfigId);
        if (GUI.Button(new Rect(150, 40, 100, 20), "Load"))
        {
            LoadFromBuildsystemServer(loadConfigId);
        }
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 130, 320, 75), "");
        GUILayout.BeginArea(new Rect(0, 130, 250, 250));
        GUILayout.Label("Show Loaded Configurations:");
        index = EditorGUILayout.Popup(
           "Loaded Configs:",
           index,
           showLoadedConfigs);

        if (GUI.Button(new Rect(50, 40, 100, 20), "Store"))
        {
            StoreLoadedConfigurations();
        }

        if (GUI.Button(new Rect(150, 40, 100, 20), "Delete"))
        {
            Debug.Log("Data to Delete: " + showLoadedConfigs[index]);
            DeleteLoadedData(showLoadedConfigs[index]);
        }

        GUILayout.EndArea();

        GUI.Box(new Rect(0, 210, 320, 40), "");
        GUILayout.BeginArea(new Rect(0, 180, 250, 250));
        if (GUI.Button(new Rect(100, 40, 100, 20), "Close"))
        {
            this.Close();
        }

        GUILayout.EndArea();
    }

    void StoreLoadedConfigurations()
    {
        foreach (PlatformData data in platformDatas)
        {
            PlatformDataManager.AddPlatformConfiguration(data);
        }
    }

    void LoadFromBuildsystemServer(string id)
    {
        string url = this.buildsystemUri + "?id=" + id;
        EditorCoroutineUtility.StartCoroutine(GetFromURL(url), this);
    }

    void DeleteLoadedData(string configName)
    {
        PlatformData dataTodelete;
        dataTodelete = platformDatas.Find(data => data.configurationName == configName);
        platformDatas.Remove(dataTodelete);

        updateList = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator GetFromURL(string uri)
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
                platformDatas.Add(data);
                this.updateList = true;
                //PlatformDataRoot dataRoot = JsonUtility.FromJson<PlatformDataRoot>(jsonString);
            }
        }
    }
}
