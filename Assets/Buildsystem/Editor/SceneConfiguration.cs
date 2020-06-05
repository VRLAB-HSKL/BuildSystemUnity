using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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
    

    private List<Scene> allScenes = new List<Scene>();

    [MenuItem("Buildsystem/Platform/Configuration")]
    static void Init()
    {
        SceneConfiguration window =
            (SceneConfiguration)EditorWindow.GetWindow(typeof(SceneConfiguration), true,
                "Scene Configuration");
        window.Show();
    }

    void OnEnable()
    {
        initSceneConfiguration();
    }
    void OnGUI()
    {
        drawSceneConfiguration();
    }


    void drawSceneConfiguration()
    {
        GUILayout.BeginArea(new Rect(10, 10, 250, 250));
        GUILayout.Label("Scene Configuration");

        bt = (OptionsBuildTarget)EditorGUILayout.EnumPopup("BuildTarget :",bt);
        btg = (OptionsTargetGroup)EditorGUILayout.EnumPopup("BuildTargetGroup :", btg);
        assignVIU = GUILayout.Toggle(assignVIU, "VIU");
        assignGvR = GUILayout.Toggle(assignGvR, "Google VR");
        assignWaveSDK = GUILayout.Toggle(assignWaveSDK, "Wave SDK");
        GUILayout.EndArea();
    }

    void initSceneConfiguration()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            allScenes.Add(EditorSceneManager.GetSceneAt(i));
            Debug.Log(EditorSceneManager.GetSceneAt(i).name);
        }
    }

}
