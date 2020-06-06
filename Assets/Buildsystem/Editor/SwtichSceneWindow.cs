using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SwtichSceneWindow : EditorWindow
{
    SceneConfManager sceneConfManager;
    int index = 0;
    public string[] sceneDatas;

    public void setSceneConfManager(SceneConfManager confManager)
    {
        this.sceneConfManager = confManager;
    }

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        drawSceneSwitchWindow();
    }

    void drawSceneSwitchWindow()
    {
        sceneDatas = sceneConfManager.getSceneDataLoadAsArray();
        index = EditorGUI.Popup(
            new Rect(10, 10, 250, 250),
            "Switch Scene:",
            index,
            sceneDatas);

        if (GUI.Button(new Rect(0, 25, 50, 50 - 26), "Load"))
        {
            switchActiveSceneAndPlatform(index);
            this.Close();
        }
    }

    void switchActiveSceneAndPlatform(int i)
    {
        string sceneName = sceneDatas[i];
        SceneData sceneData = new SceneData();
        sceneData = sceneConfManager.getSceneDataConfiguration(sceneName);

        prepareBuildTarget(sceneData.buildtarget, sceneData.buildtargetGroup);
        prepareScene(sceneData.sceneName);
        prepareAssets(sceneData.viu, sceneData.gvr, sceneData.wavevr);
    }

    void prepareScene(string name)
    {
        string path = EditorSceneManager.GetSceneByName(name).path;
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/"+name+".unity");
        
    }

    void prepareAssets(bool viu, bool gvr, bool wave)
    {
        if(viu)
        {
            AssetDatabase.DeleteAsset("Assets/WaveVR");
            AssetDatabase.DeleteAsset("Assets/GoogleVR");
            AssetDatabase.ImportPackage("Assets/Resources/ViveInputUtility_v1.10.7.unitypackage", false);
        }
        

        if (gvr)
        {
            AssetDatabase.DeleteAsset("Assets/WaveVR");
            AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
            AssetDatabase.ImportPackage("Assets/Resources/GoogleVRForUnity_1.200.1.unitypackage", false);
        }
        

        if(wave)
        {
            AssetDatabase.DeleteAsset("Assets/GoogleVR");
            AssetDatabase.ImportPackage("Assets/Resources/ViveInputUtility_v1.10.7.unitypackage", false);
            AssetDatabase.ImportPackage("Assets/Resources/wvr_unity_sdk.unitypackage", false);
        }

        if(!viu && !gvr && !wave)
        {
            AssetDatabase.DeleteAsset("Assets/WaveVR");
            AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
            AssetDatabase.DeleteAsset("Assets/GoogleVR");
        }
        
    }

    void prepareBuildTarget(string buildTarget, string buildTargetGroup)
    {
        if(buildTarget == "Android" && buildTargetGroup == "Android")
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        if(buildTarget == "Standalone" && buildTargetGroup == "StandaloneWindows64")
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        }
    }
}