using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[System.Serializable]
public class Buildsystem : MonoBehaviour
{
    public static SceneConfManager sceneConfManager = new SceneConfManager();

    [MenuItem("Buildsystem/Platform/Standalone - Win64")]
    private static void LoadStandaloneWin64Build()
    {
        AssetDatabase.DeleteAsset("Assets/GoogleVR");
        AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/WinStandalone.unity");
    }

    [MenuItem("Buildsystem/Platform/LoadVIU - VivePro-Win64")]
    private static void LoadViuViveProWin64Build()
    {
        AssetDatabase.DeleteAsset("Assets/GoogleVR");
        AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        //Open the Scene in the Editor (do not enter Play Mode)
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/VIU-Win.unity");
        AssetDatabase.ImportPackage("Assets/Resources/ViveInputUtility_v1.10.7.unitypackage", false);
    }

    [MenuItem("Buildsystem/Platform/LoadVIU - ViveFocusPro - Android")]
    private static void LoadViveFocusProAndroidBuild()
    {
        AssetDatabase.DeleteAsset("Assets/GoogleVR");
        AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        //Open the Scene in the Editor (do not enter Play Mode)
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/VIU-Android.unity");
        AssetDatabase.ImportPackage("Assets/Resources/ViveInputUtility_v1.10.7.unitypackage", false);
    }

    [MenuItem("Buildsystem/Platform/LoadGVR - Cardboard - Android")]
    private static void LoadGvrCardboardAndroidBuild()
    {
        AssetDatabase.DeleteAsset("Assets/GoogleVR");
        AssetDatabase.DeleteAsset("Assets/HTC.UnityPlugin");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorSceneManager.OpenScene("Assets/Buildsystem/Scenes/GVR.unity");
        AssetDatabase.ImportPackage("Assets/Resources/GoogleVRForUnity_1.200.1.unitypackage", false);
    }

    [MenuItem("Buildsystem/Platform/Configuration")]
    static void ShowSceneConfigurationManger()
    {
        SceneConfigurationManager window =
            (SceneConfigurationManager)EditorWindow.GetWindow(typeof(SceneConfigurationManager), true,
                "Scene Manager");
        window.setSceneConfManager(sceneConfManager);
        window.Show();
    }

    [MenuItem("Buildsystem/Platform/SwitchScene")]
    static void SwitchPlatformAndScene()
    {
        SwtichSceneWindow swtichSceneWindow =
            (SwtichSceneWindow)EditorWindow.GetWindow(typeof(SwtichSceneWindow), true,
            "Switch between Scenes");
        swtichSceneWindow.setSceneConfManager(sceneConfManager);
        swtichSceneWindow.Show();
    }
}
