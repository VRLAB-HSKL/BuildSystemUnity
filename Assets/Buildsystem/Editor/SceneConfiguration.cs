using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum OptionsTargetGroup
{
    Standalone,
    Android,
}

public enum OptionsBuildTarget
{
    StandaloneWindows64,
    Android,
}


public class SceneConfiguration : EditorWindow
{

    public OptionsBuildTarget bt;
    public OptionsTargetGroup btg;
    bool assignVIU = false;
    bool assignGvR = false;
    bool assignWaveSDK = false;
    private string buildTargetGroupName;
    private string buildTargetName;
    SceneConfManager SceneConfManager;

    private string sceneName;

    public void SetConfigManager(SceneConfManager sceneConfManager)
    {
        this.SceneConfManager = sceneConfManager;
    }

    public void SetSceneName(string sceneName)
    {
        this.sceneName = sceneName;
    }

    void OnEnable()
    {
        
    }

    void OnGUI()
    {
        drawSceneConfiguration();
    }

    void drawSceneConfiguration()
    {
        GUILayout.BeginArea(new Rect(10, 10, 250, 250));
        GUILayout.Label("Scene Configuration");
        GUILayout.Label("Configuration for " + sceneName + " :");

        bt = (OptionsBuildTarget)EditorGUILayout.EnumPopup("BuildTarget :", bt);
        btg = (OptionsTargetGroup)EditorGUILayout.EnumPopup("BuildTargetGroup :", btg);
        assignVIU = GUILayout.Toggle(assignVIU, "VIU");
        assignGvR = GUILayout.Toggle(assignGvR, "Google VR");
        assignWaveSDK = GUILayout.Toggle(assignWaveSDK, "Wave SDK");
        if (GUILayout.Button("Save Config"))
        {
            getBuildTarget(bt);
            getBuildTargetGroupOtion(btg);
            SceneData sceneData = new SceneData();
            sceneData.sceneName = sceneName;
            sceneData.buildtarget = buildTargetName;
            sceneData.buildtargetGroup = buildTargetGroupName;
            sceneData.viu = assignVIU;
            sceneData.gvr = assignGvR;
            sceneData.wavevr = assignWaveSDK;
            SceneConfManager.addSceneData(sceneData);
            this.Close();
            //SceneConfig.sceneConfigs = new SceneData[] { sceneData };
            //Debug.Log("SceneConfigs saved: " + SceneConfig);
            //string saveFile = JsonUtility.ToJson(sceneData, true);
            //Debug.Log(JsonUtility.ToJson(SceneConfig, true));
            //saveIntoJson(saveFile);
        }
        GUILayout.EndArea();
    }

    void getBuildTargetGroupOtion(OptionsTargetGroup btg)
    {
        switch (btg)
        {
            case OptionsTargetGroup.Android:
                buildTargetGroupName = "Android";
                break;
            case OptionsTargetGroup.Standalone:
                buildTargetGroupName = "Standalone";
                break;

        }
    }

    void getBuildTarget(OptionsBuildTarget bt)
    {
        switch (bt)
        {
            case OptionsBuildTarget.Android:
                buildTargetName = "Android";
                break;
            case OptionsBuildTarget.StandaloneWindows64:
                buildTargetName = "StandaloneWindows64";
                break;
        }
    }
}
    

