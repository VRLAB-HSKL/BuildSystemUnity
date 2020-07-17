using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;

/// <summary>
/// 
/// </summary>
public class PlatformConfigurationManager : EditorWindow
{

    //
    private int index;

    //
    private List<PlatformData> platformDatas;

    //
    private string[] platFormConfigs;

    //
    private PlatformDataManager PlatformDataManager;

    //
    private bool loadPlatformConfigurationData = true;

    //
    private Build build;

    //
    private string platformConfigurationURI;

    //
    private string webAppURI;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformDataManager"></param>
    internal void SetPlatformDataMangager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    /// <summary>
    /// 
    /// </summary>
    void Init()
    {
        this.build = new Build();
        this.platFormConfigs = new string[100];
        this.platformConfigurationURI = "http://localhost:8080/api/unity/platformconfigurationservice";
        this.webAppURI = "http://localhost:3000/";
    }

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        ShowPlatformConfigurationManager();
        this.platFormConfigs = PlatformDataManager.GetAllConfigurationNamesAsArray();
        LoadPlatformConfigurationXML();
    }

    /// <summary>
    /// 
    /// </summary>
    void LoadPlatformConfigurationXML()
    {
        if(loadPlatformConfigurationData)
        {
            Debug.Log("File loaded once");
            PlatformDataManager.LoadData();
            this.loadPlatformConfigurationData = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void ShowPlatformConfigurationManager()
    {
       
        GUI.Box(new Rect(0, 0, 260, 140), "Platform Configuration: ");
        
        index = EditorGUI.Popup(new Rect(0, 25, 255, 15), "Configurations:", index, platFormConfigs);

        if (GUI.Button(new Rect(5, 65, 55, 24), "Create"))
        {
            CreateConfiguration createConfigurationWindow =
            (CreateConfiguration)EditorWindow.GetWindow(typeof(CreateConfiguration), true,
            "Create Configuration");
            createConfigurationWindow.SetDataManager(this.PlatformDataManager);
            createConfigurationWindow.Show();
        }

        if (GUI.Button(new Rect(65,65,55,24), "Edit"))
        {
            Debug.Log("Edit index:" + index);
            EditPlatformDataWindow editConfigurationWindow =
            (EditPlatformDataWindow)EditorWindow.GetWindow(typeof(EditPlatformDataWindow), true,
            "Edit Configuration");
            editConfigurationWindow.SetDataManager(this.PlatformDataManager);
            //editConfigurationWindow.SetPlatformDataToEdit(this.PlatformDataManager.getPlatformDataFromIndex(this.index));
            editConfigurationWindow.SetIndex(index);
            editConfigurationWindow.Show();
        }

        if (GUI.Button(new Rect(5, 95, 55, 24), "Save"))
        {
            PlatformDataManager.SaveData();
            this.Close();
        }

        if (GUI.Button(new Rect(65, 95, 55, 24), "Delete"))
        {
            DeleteSelectedPlatformConfiguration(index);
        }

        GUI.Box(new Rect(265, 0, 120, 200), "Load Configurations: ");

        if (GUI.Button(new Rect(267, 65, 115, 24), "Open Selected"))
        {
            Debug.Log("Load index:" + index);
            PrepareLoadConfigurationSetup(index);
        }

        if (GUI.Button(new Rect(267, 95, 115, 24), "Build"))
        {
            BuildWindow buildWindow =
                (BuildWindow)EditorWindow.GetWindow(typeof(BuildWindow), true, "Build");
            buildWindow.SetDataManager(PlatformDataManager);
            buildWindow.Show();
        }

        GUI.Box(new Rect(390, 0, 125, 200), "Buildsystem WebApp: ");

        if (GUI.Button(new Rect(392, 40, 120, 24), "Load"))
        {
            //LoadFromBuildsystemServer();
            LoadWindow loadWindow =
                (LoadWindow)EditorWindow.GetWindow(typeof(LoadWindow), true, "Load Platform configuration");
            loadWindow.SetDataManager(this.PlatformDataManager);
            loadWindow.Show();
        }

        if (GUI.Button(new Rect(392, 70, 120, 24), "Store"))
        {
            StoreToBuildsystemServer();
        }

        if (GUI.Button(new Rect(392, 100, 120, 24), "Store Selected"))
        {
            StoreSelectedToBuildsystemServer();
        }


        if (GUI.Button(new Rect(392, 130, 120, 24), "WebApp"))
        {
            Application.OpenURL(webAppURI);
        }

        GUI.Box(new Rect(0, 145, 260, 55), "");
        if (GUI.Button(new Rect(5, 165, 117 , 24), "Close"))
        {
            this.Close();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    void PrepareLoadConfigurationSetup(int index)
    {

        PlatformData dataToLoad = new PlatformData();
        dataToLoad = PlatformDataManager.GetPlatformDataFromIndex(index);
        Debug.Log(dataToLoad.sceneName);
        LoadScene(dataToLoad.sceneName);
        PrepareBuildSettings(dataToLoad.buildTarget, dataToLoad.buildTargetGroup);
        this.Close();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    void DeleteSelectedPlatformConfiguration(int index)
    {
        PlatformData dataToDelete = new PlatformData();
        dataToDelete = PlatformDataManager.GetPlatformDataFromIndex(index);
        Debug.Log("i: " + index + " => " + "File: " + dataToDelete.configurationName);
        PlatformDataManager.DeleteSelectedPlatformDataByData(dataToDelete);
    }

    /// <summary>
    /// This Method opend the selected scene
    /// </summary>
    /// <param name="name"> scene name</param>
    void LoadScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }


    /// <summary>
    /// this method changes the build target based on the scene selected by the user
    /// </summary>
    /// <param name="buildTarget">unity buildtarget</param>
    /// <param name="buildTargetGroup">unity buildtargetgroup</param>
    void PrepareBuildSettings(string buildTarget, string buildTargetGroup)
    {
        

        if (buildTarget == "Android" && buildTargetGroup == "Android")
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        if (buildTarget == "StandaloneWindows64" && buildTargetGroup == "Standalone")
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        }
    }


    void StoreSelectedToBuildsystemServer()
    {

        PlatformData dataToSend = new PlatformData();
        dataToSend = PlatformDataManager.GetPlatformDataFromIndex(index);
        String jsonString = JsonUtility.ToJson(dataToSend);
        Debug.Log(jsonString);
        EditorCoroutineUtility.StartCoroutine(Upload(jsonString), this);
        //PlatformDataManager.PlatformDataList
    }

    void StoreToBuildsystemServer()
    {
        List<PlatformData> datalist = PlatformDataManager.PlatformDataList.platformDatas;

        foreach(PlatformData data in datalist)
        {
            String jsonString = JsonUtility.ToJson(data);
            Debug.Log(jsonString);
            EditorCoroutineUtility.StartCoroutine(Upload(jsonString), this);
        }

        
    }

    /// <summary>
    /// this method sends the project information to the webserver
    /// </summary>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    IEnumerator Upload(String jsonString)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(platformConfigurationURI, jsonString))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
