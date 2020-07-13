using Boo.Lang;
using System;

/// <summary>
/// This class contains the data structure of the XML representation
/// </summary>

[Serializable]
public class PlatformData 
{
    public long id;

    public string configurationName;

    public string sceneName;

    public string projectName;

    public string description;

    public bool viu;

    public bool gvr;

    public bool wavevr;

    public bool middlevr;

    public string buildTargetGroup;

    public string buildTarget;

    public int index;
}

[Serializable]
public class PlatformDataRoot
{
    public PlatformData[] platformDatas;
}
