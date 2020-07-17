using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EditPlatformDataWindow : EditorWindow
{
    // flag to update the data to edit
    bool updateOnce = false;

    //Enum: provides BuildTarget options
    public OptionsBuildTarget bt;

    //Enum: provied BuildTargetGroup options
    public OptionsTargetGroup btg;

    //bool vive input utility
    bool assignVIU = false;

    //bool goole vr
    bool assignGvR = false;

    //bool wave sdk
    bool assignWaveSDK = false;

    //bool middleVR
    bool assignMiddleVR = false;

    //index
    int storedIndex;

    //buildtargetgroup name 
    private string buildTargetGroupName;

    //buildtarget name
    private string buildTargetName;

    //project name
    private string projectName;

    //description
    private string description;

    //index
    private int index;

    //scene name
    private string sceneName;

    //contains the names from all scenes
    private string[] allScenesPath;

    //configuration name
    private string configName;

    //platform configuration
    private PlatformData platformData;

    /// <summary>
    /// <see cref="PlatformDataManager"/> SceneConfManager
    /// </summary>
    PlatformDataManager PlatformDataManager;
        
    /// <summary>
    /// Setter for <see cref="PlatformDataManager"/> PlatformDataManager
    /// </summary>
    /// <param name="platformDataManager"></param>
    public void SetDataManager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformData"></param>
    public void SetPlatformDataToEdit(PlatformData platformData)
    {
        this.platformData = platformData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void SetIndex(int index)
    {
        this.index = index;
        this.storedIndex = index;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateDataToEdit()
    {
        if(!updateOnce)
        {
            this.platformData = new PlatformData();
            this.platformData = PlatformDataManager.GetPlatformDataFromIndex(this.index);
            this.projectName = platformData.projectName;
            this.description = platformData.description;
            this.configName = platformData.configurationName;
            this.assignVIU = platformData.viu;
            this.assignGvR = platformData.gvr;
            this.index = platformData.index;
            this.assignWaveSDK = platformData.wavevr;
            this.assignMiddleVR = platformData.middlevr;

            if (platformData.buildTargetGroup == "Android")
            {
                btg = OptionsTargetGroup.Android;
            }

            if (platformData.buildTargetGroup == "Standalone")
            {
                btg = OptionsTargetGroup.Standalone;
            }

            if (platformData.buildTarget == "Android")
            {
                bt = OptionsBuildTarget.Android;
            }

            if (platformData.buildTarget == "StandaloneWindows64")
            {
                bt = OptionsBuildTarget.StandaloneWindows64;
            }

            updateOnce = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        

    }

    /// <summary>
    /// 
    /// </summary>
    private void OnGUI()
    {
        UpdateDataToEdit();
        LoadActiveScenes();
        ShowCreateConfiguration();
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowCreateConfiguration()
    {

        GUILayout.BeginArea(new Rect(0, 0, 250, 250));
        GUILayout.Label("Create Configuration:");
        configName = EditorGUILayout.TextField("Config. Name:", configName);
        description = EditorGUILayout.TextField("Description: ", description);
        projectName = EditorGUILayout.TextField("Product Name: ", projectName);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 80, 250, 250));
        index = EditorGUILayout.Popup(
            "Choose Scene:",
            index,
            allScenesPath);
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0, 100, 250, 250));
        bt = (OptionsBuildTarget)EditorGUILayout.EnumPopup("BuildTarget :", bt);
        btg = (OptionsTargetGroup)EditorGUILayout.EnumPopup("Platform :", btg);
        assignVIU = GUILayout.Toggle(assignVIU, "VIU");
        assignGvR = GUILayout.Toggle(assignGvR, "Google VR");
        assignWaveSDK = GUILayout.Toggle(assignWaveSDK, "Wave SDK");
        assignMiddleVR = GUILayout.Toggle(assignMiddleVR, "MiddleVR");

        if (GUI.Button(new Rect(0, 200, 50, 25), "Save"))
        {
            PlatformData platformData = new PlatformData();
            platformData.configurationName = configName;
            platformData.description = description;
            platformData.projectName = projectName;
            platformData.sceneName = allScenesPath[index];
            GetBuildTarget(bt);
            GetBuildTargetGroupOption(btg);
            platformData.buildTarget = buildTargetName;
            platformData.buildTargetGroup = buildTargetGroupName;
            platformData.viu = assignVIU;
            platformData.gvr = assignGvR;
            platformData.wavevr = assignWaveSDK;
            platformData.middlevr = assignMiddleVR;
            PlatformDataManager.UpdatePlatformData(platformData);
            this.Close();
        }

        if (GUI.Button(new Rect(50, 200, 50, 25), "Cancel"))
        {
            this.Close();
        }
        GUILayout.EndArea();
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadActiveScenes()
    {
        this.allScenesPath = this.PlatformDataManager.GetScenesPath();
    }

    /// <summary>
    /// set the user input buildtargetgroup
    /// </summary>
    /// <param name="btg"><see cref="BuildTargetGroup"/>buildtargetgroup</param>
    void GetBuildTargetGroupOption(OptionsTargetGroup btg)
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

    /// <summary>
    /// set the user input buildtarget
    /// </summary>
    /// <param name="bt"><see cref="BuildTarget"/>buildtarget</param>
    void GetBuildTarget(OptionsBuildTarget bt)
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
