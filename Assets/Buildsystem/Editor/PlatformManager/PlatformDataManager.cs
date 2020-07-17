using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlatformDataManager 
{
    // Platform configuration storing class
    public PlatformDataList PlatformDataList;

    //path of all scenes
    public string[] allScenesPath;

    //provides a list of all scenes
    private List<string> allScenes;

    //path to platform configuration xml file
    private string pathConfigXML;


    /// <summary>
    /// standard ctor
    /// Initial the local storing Data with a List
    /// </summary>
    public PlatformDataManager()
    {
        this.PlatformDataList = new PlatformDataList();
        this.PlatformDataList.platformDatas = new List<PlatformData>();
        this.pathConfigXML = "/Buildsystem/StreamingFiles/XML/save_platformConfig.xml";
        this.allScenes = new List<string>();
        LoadActiveScenes();
    }

    /// <summary>
    /// This Method returns a List of string with all platformconfiguration names from the stored platformdata configurations. 
    /// </summary>
    /// <returns>a string[] witch contains all Names from the stored platform configurations</returns>
    public string[] GetAllConfigurationNamesAsArray()
    {
       
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
    public void AddPlatformConfiguration(PlatformData platformData)
    {
        //if(!platformDatas.Contains(platformData)) platformDatas.Add(platformData);
        if (!PlatformDataList.platformDatas.Contains(platformData)) PlatformDataList.platformDatas.Add(platformData);
    }

    /// <summary>
    /// this method provides a specific configuration based on the passed parameter
    /// </summary>
    /// <param name="index"> index </param>
    /// <returns> returns a configurations based on the passed parameter </returns>
    public PlatformData GetPlatformDataFromIndex(int index)
    {
        Debug.Log("Index: " + index);
        Debug.Log("Count-Datas: " + PlatformDataList.platformDatas.Count);
        PlatformData[] platformDataArray = this.PlatformDataList.platformDatas.ToArray();
        return platformDataArray[index];
    }

    /// <summary>
    /// This Method allows user to edit a specific configuration
    /// </summary>
    /// <param name="dataToEdit"></param>
    public void UpdatePlatformData(PlatformData dataToEdit)
    {
                
        foreach (PlatformData data in PlatformDataList.platformDatas)
        {
            if (data.configurationName == dataToEdit.configurationName)
            {
                data.sceneName = dataToEdit.sceneName;
                data.description = dataToEdit.description;
                data.viu = dataToEdit.viu;
                data.gvr = dataToEdit.gvr;
                data.wavevr = dataToEdit.wavevr;
                data.middlevr = dataToEdit.middlevr;
                data.buildTargetGroup = dataToEdit.buildTargetGroup;
                data.buildTarget = dataToEdit.buildTarget;
            }
        }

        Debug.Log("Edit Done");
     
    }

    /// <summary>
    /// This Method deletes a specific platform configuration
    /// </summary>
    /// <param name="toDelete"></param>
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
    public string[] GetScenesPath()
    {
        return this.allScenesPath;
    }

    /// <summary>
    /// this method contains all registred scenes in buildpath
    /// </summary>
    public void LoadActiveScenes()
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scene.path);
            Debug.Log(sceneName);
            allScenes.Add(sceneName);
        }

        allScenesPath = allScenes.ToArray();
    }

    /// <summary>
    /// This Method save the platform configurations in a xml file
    /// </summary>
    public void SaveData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlatformDataList));
        using (FileStream stream = new FileStream(Application.dataPath +
            pathConfigXML, FileMode.CreateNew))
        {
            serializer.Serialize(stream, PlatformDataList);
            stream.Close();
        }
        Debug.Log("Stream save Closed");
    }

    /// <summary>
    /// this method loads the scene configuration file
    /// </summary>
    public void LoadData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlatformDataList));
        string path = Application.dataPath + pathConfigXML;
        if(File.Exists(path)) {
            Debug.Log("File exists and ready to load");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlatformDataList = serializer.Deserialize(stream) as PlatformDataList;
                stream.Close();
            }
            Debug.Log("Stream load Closed");
        } else {
            Debug.Log("No platform config found");
        }    
    }
}