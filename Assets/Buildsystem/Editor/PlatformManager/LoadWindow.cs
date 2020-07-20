using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class provide a user interactions to get platform configurations from the Buildsystem.
/// </summary>
public class LoadWindow : EditorWindow
{
    //This ID ensures that the correct data is loaded from the MongoDB
    private string loadConfigId;
    
    //index for the popup window
    private int index;

    //this string[] shows the user the already loaded configuration names
    private string[] showLoadedConfigs;
    
    //URI to the Buildsystem REST interface
    private string buildsystemUri = "http://localhost:8080/api/unity/getplatformconfigurationbyid";

    //This list contains all loaded platform configurations.
    private List<PlatformData> platformDatas;

    //this boolean changes its value every time a new configuration is loaded to update the data sets
    private bool updateList;

    /// <summary>
    /// <see cref="PlatformDataManager"/> SceneConfManager
    /// </summary>
    private PlatformDataManager PlatformDataManager;

    //URI to the website
    private string unityConfigsURI = "http://localhost:3000/unityconfigs";

    /// <summary>
    /// Setter for <see cref="SceneConfManager"/> SceneConfManager
    /// </summary>
    /// <param name="platformDataManager"></param>
    public void SetDataManager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    /// <summary>
    /// this method called once 
    /// </summary>
    private void OnEnable()
    {
        updateList = false;
        showLoadedConfigs = new string[10];
        platformDatas = new List<PlatformData>();
    }

    /// <summary>
    /// similar to the update method
    /// </summary>
    private void OnGUI()
    {
        ShowLoadWindow();
        UpdateList();
    }

    /// <summary>
    /// update the loaded configuration list if data changed
    /// </summary>
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

    /// <summary>
    /// this method is provided by the user interface
    /// </summary>
    private void ShowLoadWindow()
    {
        GUI.Box(new Rect(0, 0, 320, 45), "");
        GUILayout.BeginArea(new Rect(0, 5, 250, 250));
        GUILayout.Label("Choose Configurations:");
        if (GUI.Button(new Rect(150, 0, 100, 20), "Open WebApp"))
        {
            Application.OpenURL(unityConfigsURI);
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

    /// <summary>
    /// this method transfers the loaded configurations to the data manager
    /// in order to further manage them and make them available locally
    /// </summary>
    void StoreLoadedConfigurations()
    {
        foreach (PlatformData data in platformDatas)
        {
            PlatformDataManager.AddPlatformConfiguration(data);
        }
    }

    /// <summary>
    /// This method assembles the URI to load the selected data after that the method to be executed is called
    /// <param name="id"> MondoDB data id</param>
    void LoadFromBuildsystemServer(string id)
    {
        string url = this.buildsystemUri + "?id=" + id;
        EditorCoroutineUtility.StartCoroutine(GetFromURL(url), this);
    }

    /// <summary>
    /// this method allows users to delete unnecessary data
    /// </summary>
    /// <param name="configName"> platform configuration name to delete</param>
    void DeleteLoadedData(string configName)
    {
        PlatformData dataTodelete;
        dataTodelete = platformDatas.Find(data => data.configurationName == configName);
        platformDatas.Remove(dataTodelete);

        updateList = true;
    }

    /// <summary>
    /// this method loads the data and adds it to the List <see cref="platformDatas"/>
    /// </summary>
    /// <param name="uri">the correctly composed uri</param>
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
            }
        }
    }
}
