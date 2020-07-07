using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Configuration;
using System;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformDataManager"></param>
    internal void setPlatformDataMangager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    /// <summary>
    /// 
    /// </summary>
    void init()
    {
        this.platFormConfigs = new string[100];
    }

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        init();
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        ShowPlatformConfigurationManager();
        this.platFormConfigs = PlatformDataManager.getAllConfigurationNamesAsArray();
        loadPlatformConfigurationXML();
    }

    /// <summary>
    /// 
    /// </summary>
    void loadPlatformConfigurationXML()
    {
        if(loadPlatformConfigurationData)
        {
            Debug.Log("File loaded once");
            PlatformDataManager.loadData();
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
            PlatformDataManager.saveData();
            this.Close();
        }

        if (GUI.Button(new Rect(65, 95, 55, 24), "Delete"))
        {
            DeleteSelectedPlatformConfiguration(index);
        }

        GUI.Box(new Rect(265, 0, 120, 200), "Load Configurations: ");

        if (GUI.Button(new Rect(267, 65, 115, 24), "Open Selected"))
        {
            PrepareLoadConfigurationSetup(index);
        }

        if (GUI.Button(new Rect(267, 95, 115, 24), "Build Selected"))
        {
            
        }

        GUI.Box(new Rect(390, 0, 125, 200), "Buildsystem WebApp: ");

        if (GUI.Button(new Rect(392, 40, 120, 24), "Load"))
        {

        }

        if (GUI.Button(new Rect(392, 70, 120, 24), "Store"))
        {
            SendToBuildsystemServer();
        }

        if (GUI.Button(new Rect(392, 100, 120, 24), "WebApp"))
        {
            Application.OpenURL("http://localhost:3000/");
        }

        GUI.Box(new Rect(0, 145, 260, 55), "");
        if (GUI.Button(new Rect(5, 165, 117 , 24), "Close"))
        {
            this.Close();
        }
    }

    void PrepareLoadConfigurationSetup(int index)
    {
        PlatformData dataToLoad = new PlatformData();
        dataToLoad = PlatformDataManager.getPlatformDataFromIndex(index);
        LoadScene(dataToLoad.sceneName);
        prepareBuildSettings(dataToLoad.buildtarget, dataToLoad.buildtargetGroup);
        this.Close();

    }

    void DeleteSelectedPlatformConfiguration(int index)
    {
        PlatformData dataToDelete = new PlatformData();
        dataToDelete = PlatformDataManager.getPlatformDataFromIndex(index);
        Debug.Log("i: " + index + " => " + "File: " + dataToDelete.configurationName);
        PlatformDataManager.DeleteSelectedPlatformDataByData(dataToDelete);
    }

    /// <summary>
    /// This Method opend the selected scene
    /// </summary>
    /// <param name="name"> scene name</param>
    void LoadScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/" + sceneName + ".unity");
    }


    /// <summary>
    /// this method changes the build target based on the scene selected by the user
    /// </summary>
    /// <param name="buildTarget">unity buildtarget</param>
    /// <param name="buildTargetGroup">unity buildtargetgroup</param>
    void prepareBuildSettings(string buildTarget, string buildTargetGroup)
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


    void SendToBuildsystemServer()
    {

    }

    /// <summary>
    /// This method loads the selected assets
    /// </summary>
    /// <param name="viu">viu asset</param>
    /// <param name="gvr">gvr asset</param>
    /// <param name="wave">wave asset</param>
    void prepareAssets(bool viu, bool gvr, bool wave)
    {
        

    }
}
