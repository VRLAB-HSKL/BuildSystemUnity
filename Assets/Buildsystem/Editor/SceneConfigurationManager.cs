using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SceneConfigurationManager : EditorWindow
{
    public SceneConfig SceneConfig = new SceneConfig();
    public string[] allScenesPath;
    int index = 0;
    private List<string> allScenes = new List<string>();
    private SceneConfManager SceneConfManager;

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
        
        index = EditorGUI.Popup(
            new Rect(10, 10, 250, 250),
            "Component:",
            index,
            allScenesPath);

        if (GUI.Button(new Rect(0, 25, 50, 50 - 26), "Edit"))
        {
            showSceneConfiguraiton(index);
        }

        if (GUI.Button(new Rect(55, 25, 50, 50 - 26), "Save"))
        {
            SceneConfManager.saveData();
            this.Close();
        }
    }


    void showSceneConfiguraiton(int i)
    {
        string actualSceneName = allScenesPath[i];
        SceneConfiguration sceneConfigurationWindow = 
            (SceneConfiguration)EditorWindow.GetWindow(typeof(SceneConfiguration), true, "Scene Configuration");
        sceneConfigurationWindow.SetSceneName(actualSceneName);
        sceneConfigurationWindow.SetConfigManager(SceneConfManager);
        sceneConfigurationWindow.Show();
        SceneConfManager.showSceneDataCount();
    }

    public void initSceneConfiguration()
    {
       
        //for (int i = 0; i < SceneManager.sceneCount; i++)
        //{
        //allScenes.Add(SceneManager.GetSceneAt(i));
        //Debug.Log(SceneManager.sceneCount);
        //actualSceneName = SceneManager.GetSceneAt(i).name;
        //Debug.Log(actualSceneName);

        //}

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            Debug.Log(sceneName);
            allScenes.Add(sceneName);
        }

        allScenesPath = allScenes.ToArray();


        //var sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
        //var pathList = sceneGUIDs.Select(AssetDatabase.GUIDToAssetPath);

        //   foreach (var guid in sceneGUIDs)
        //   {
        //       var path = AssetDatabase.GUIDToAssetPath(guid);
        //       Debug.Log(path);
        //   }

    }

    public void setSceneConfManager(SceneConfManager confManager)
    {
        this.SceneConfManager = confManager;
    }

    void saveIntoJson(string jsontosave)
    {
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/SceneConfig.json", jsontosave);
    }

}
