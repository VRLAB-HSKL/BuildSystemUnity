using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class PlatformDataManager 
{
    //private List<PlatformData> platformDatas;

    public PlatformDataList PlatformDataList;

    //path of all scenes
    public string[] allScenesPath;

    
    //provides a list of all scenes
    private List<string> allScenes = new List<string>();


    public PlatformDataManager()
    {
        //this.platformDatas = new List<PlatformData>();
        this.PlatformDataList = new PlatformDataList();
        this.PlatformDataList.platformDatas = new List<PlatformData>();

        loadActiveScenes();
    }

    


    public string[] getAllConfigurationNamesAsArray()
    {
        /**
        PlatformData[] dataArray = platformDatas.ToArray();
        string[] configNameArray = new string[platformDatas.Count];

        for (int i = 0; i < platformDatas.Count; i++)
        {
            configNameArray[i] = dataArray[i].configurationName;
        }

        return configNameArray;
        **/
        PlatformData[] dataArray = PlatformDataList.platformDatas.ToArray();
        string[] configNameArray = new string[PlatformDataList.platformDatas.Count];

        for (int i = 0; i < PlatformDataList.platformDatas.Count; i++)
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
        //if(!platformDatas.Contains(platformData)) platformDatas.Add(platformData);
        if (!PlatformDataList.platformDatas.Contains(platformData)) PlatformDataList.platformDatas.Add(platformData);
    }

    public PlatformData[] getPlatformDataAsArray()
    {
        //return this.platformDatas.ToArray();
        return this.PlatformDataList.platformDatas.ToArray();
    }

    public PlatformData getPlatformDataFromIndex(int index)
    {
        Debug.Log("Index: " + index);
        //Debug.Log("Count-Datas: " + platformDatas.Count);
        Debug.Log("Count-Datas: " + PlatformDataList.platformDatas.Count);
        // PlatformData[] platformDataArray = this.platformDatas.ToArray();
        PlatformData[] platformDataArray = this.PlatformDataList.platformDatas.ToArray();
        return platformDataArray[index];
    }

    public void updatePlatformDataByIndex(int index, PlatformData data)
    {
        //PlatformData[] platformDataArray = this.platformDatas.ToArray();
        PlatformData[] platformDataArray = this.PlatformDataList.platformDatas.ToArray();
        platformDataArray[index] = data;
        Debug.Log("Edit Done");
        
    }

    public void updatePlatformDataByData(string configName, PlatformData editData)
    {
       PlatformData datatosave;
       //datatosave = platformDatas.Find(data => data.configurationName == configName);
       //platformDatas.Remove(datatosave);
       //platformDatas.Add(editData);

        datatosave = PlatformDataList.platformDatas.Find(data => data.configurationName == configName);
        PlatformDataList.platformDatas.Remove(datatosave);
        PlatformDataList.platformDatas.Add(editData);
    }

    public void DeleteSelectedPlatformDataByData(PlatformData toDelete)
    {
        PlatformData dataTodelete;
        dataTodelete = PlatformDataList.platformDatas.Find(data => data.configurationName == toDelete.configurationName);
        PlatformDataList.platformDatas.Remove(dataTodelete);
        Debug.Log("Delete data Success");
    }

    /// <summary>
    /// returned all Scenes in Buildpath
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// This Method save the platform configurations in a xml file
    /// </summary>
    public void saveData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlatformDataList));
        using (FileStream stream = new FileStream(Application.dataPath +
            "/Buildsystem/StreamingFiles/XML/save_platformConfig.xml", FileMode.Create))
        {
            serializer.Serialize(stream, PlatformDataList);
            stream.Close();
        }
        Debug.Log("Stream save Closed");
    }

    /// <summary>
    /// this method loads the scene configuration file
    /// </summary>
    public void loadData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlatformDataList));
        string path = Application.dataPath + "/Buildsystem/StreamingFiles/XML/save_platformConfig.xml";
        if(File.Exists(path)) {
            Debug.Log("File exists and ready to load");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {

                PlatformDataList = serializer.Deserialize(stream) as PlatformDataList;
                stream.Close();
            }
            Debug.Log("Stream load Closed");
        } else
        {
            Debug.Log("No platform config found");
        }
        
    }

}
