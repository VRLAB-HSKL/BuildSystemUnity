using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


/// <summary>
/// This class provides a build process. It allows the user to start a direct build based on his configuration
/// </summary>
public class BuildWindow : EditorWindow
{

    //Enum: provides BuildTarget options
    public OptionsBuildTarget bt;

    //Enum: provied BuildTargetGroup options
    public OptionsTargetGroup btg;
    
    //buildtarget android
    private BuildTarget btAndroid;
    
    //buildtarget windows
    private BuildTarget btWindows;

    //buildtarget group android
    private BuildTargetGroup btgAndroid;

    //buildtarget group windows
    private BuildTargetGroup btgWindows;

    //switch between windows or android
    private string buildProcess;

    //folder path
    private string folderPath;

    //Application name
    private string appName;

    //path to scene
    private string scenePath;

    //scene ending ".unity"
    private string sceneEnding;

    //full path to destionation
    private string destinationFile;

    //bool to switch between the default- or direct to device build
    private bool usbAndroid;

    /// <summary>
    /// <see cref="PlatformDataManager"/> SceneConfManager
    /// </summary>
    PlatformDataManager PlatformDataManager;

    //contains all active scenes
    private string[] allScenesPath;
    
    //index for active scenes example: [Index 1: SceneName: TestScene1], [Index 2: SceneName: TestScene2]
    private int index;


    /// <summary>
    /// this method called once 
    /// </summary>
    void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// similar to the update method
    /// </summary>
    void OnGUI()
    {
        LoadActiveScenes();
        ShowBuildWindow();
    }

    /// <summary>
    /// initialization of the variable
    /// </summary>
    void Init()
    {
        this.destinationFile = "";
        this.sceneEnding = ".unity";
        this.scenePath = "Assets/Scenes/";
        this.buildProcess = "";
        this.index = 0;
        this.folderPath = "";
        this.appName = "";
        this.usbAndroid = false;
    }

    /// <summary>
    /// Setter for <see cref="SceneConfManager"/> SceneConfManager
    /// </summary>
    /// <param name="platformDataManager"></param>
    public void SetDataManager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    /// <summary>
    /// loads all active scenes in unity project (active means the scenes are enabled in Buildprocess)
    /// </summary>
    private void LoadActiveScenes()
    {
        this.allScenesPath = this.PlatformDataManager.GetScenesPath();
    }

    /// <summary>
    ///This method provides the userinterace 
    /// </summary>
    void ShowBuildWindow()
    {
        GUI.Box(new Rect(0, 0, 290, 170), "Build Configuration: ");

        
        EditorGUI.LabelField( new Rect(0,0,160,80),"Choose Destination Folder:");
        if(GUI.Button(new Rect(160,30,120,20), "Search"))
        {
            folderPath = EditorUtility.OpenFolderPanel("Select Build Folder", "", "");
            Debug.Log("Path: " + folderPath);
        }

        GUILayout.BeginArea(new Rect(0, 70, 250, 250));

        appName = EditorGUILayout.TextField("Application Name:", appName);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 100, 250, 250));
        index = EditorGUILayout.Popup(
            "Choose Scene:",
            index,
            allScenesPath);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 130, 250, 250));
        bt = (OptionsBuildTarget)EditorGUILayout.EnumPopup("BuildTarget :", bt);
        btg = (OptionsTargetGroup)EditorGUILayout.EnumPopup("Platform :", btg);
        GetBuildTarget(bt);
        GetBuildTargetGroupOption(btg);
        GUILayout.EndArea();
        
        GUI.Box(new Rect(0,180,290,55), "USB-Build for Android");
        GUILayout.BeginArea(new Rect(5, 205, 250, 250));
        usbAndroid = GUILayout.Toggle(usbAndroid, "USB Android");
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 245, 290, 30), "");
        if (GUI.Button(new Rect(5, 250, 120, 20), "Build"))
        {

            string fullScenePath = this.scenePath + allScenesPath[index] + this.sceneEnding;
            Debug.Log("BuildProcess: ScenePath: " + fullScenePath + ", Destination: " + destinationFile);

            if (buildProcess == "Windows")
            {
                this.destinationFile = folderPath + "/" + appName + ".exe";
                StartWindowsBuild(fullScenePath, destinationFile);
            }

            if (buildProcess == "Android")
            {

                this.destinationFile = folderPath + "/" + appName + ".apk";

                if (usbAndroid)
                {
                    StartAndroidAutoBuild(fullScenePath, destinationFile);
                } else {
                    
                    StartAndroidBuild(fullScenePath, destinationFile);
                }
                
            }
        }

        if (GUI.Button(new Rect(160, 250, 120, 20), "Close"))
        {
            this.Close();
        }
    }

    /// <summary>
    /// This method starts the Android build and assembles the variables specified in the user interface
    /// </summary>
    /// <param name="fullScenePath"> path to the choosen scene </param>
    /// <param name="destinationFile"> path to build destination </param>
    void StartAndroidBuild(string fullScenePath, string destinationFile)
    {
        string[] scenesPath = new[] { fullScenePath };
        BuildPipeline.BuildPlayer(scenesPath, destinationFile, BuildTarget.Android, BuildOptions.None);
    }

    /// <summary>
    /// This method starts the Android build and assembles the variables specified in the user interface.
    /// The special thing about this build is that it is deployed directly on a connected device.
    /// </summary>
    /// <param name="fullScenePath"> path to the choosen scene </param>
    /// <param name="destinationFile">path to build destination </param>
    void StartAndroidAutoBuild(string fullScenePath, string destinationFile)
    {
        string[] scenesPath = new[] { fullScenePath };
        BuildPipeline.BuildPlayer(scenesPath, destinationFile, BuildTarget.Android, BuildOptions.AutoRunPlayer);
    }

    /// <summary>
    /// This method starts the Windows build and assembles the variables specified in the user interface
    /// </summary>
    /// <param name="fullScenePath"> path to the choosen scene </param>
    /// <param name="destinationFile"> path to build destination </param>
    void StartWindowsBuild(string fullScenePath, string destinationFile)
    {
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { fullScenePath };
        buildPlayerOptions.locationPathName = destinationFile;
        buildPlayerOptions.target = this.btWindows;
        buildPlayerOptions.targetGroup = this.btgWindows;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
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
                btgAndroid = BuildTargetGroup.Android;
                break;
            case OptionsTargetGroup.Standalone:
                btgWindows = BuildTargetGroup.Standalone;
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
                btAndroid = BuildTarget.Android;
                buildProcess = "Android";
                break;
            case OptionsBuildTarget.StandaloneWindows64:
                btWindows = BuildTarget.StandaloneWindows64;
                buildProcess = "Windows";
                break;
        }
    }
}
