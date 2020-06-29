using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformDataManager 
{
    private List<PlatformData> platformDatas;

    //path of all scenes
    public string[] allScenesPath;

    
    //provides a list of all scenes
    private List<string> allScenes = new List<string>();


    public PlatformDataManager()
    {
        this.platformDatas = new List<PlatformData>();
        loadActiveScenes();
    }

    


    public string[] getAllConfigurationNamesAsArray()
    {
        PlatformData[] dataArray = platformDatas.ToArray();
        string[] configNameArray = new string[platformDatas.Count];

        for (int i = 0; i < platformDatas.Count; i++)
        {
            configNameArray[i] = dataArray[i].configurationName;
        }

        return configNameArray;
    }

    /// <summary>
    /// add a platform configuration to the List
    /// </summary>
    /// <param name="platformData"></param>
    public void addPlatformConfiguration(PlatformData platformData)
    {
        if(!platformDatas.Contains(platformData)) platformDatas.Add(platformData);
    }


    public string[] getScenesPath()
    {
        return this.allScenesPath;
    }

    public void loadActiveScenes()
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            Debug.Log(sceneName);
            allScenes.Add(sceneName);
        }

        allScenesPath = allScenes.ToArray();
    }

}
