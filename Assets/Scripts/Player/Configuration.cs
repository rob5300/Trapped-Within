using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Configuration
{
    private static Configuration _configuration;
    private static string path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "UserData" + Path.DirectorySeparatorChar;
    private static string pathAndFile = path + "config.xml";

    public static void LoadConfig()
    {
        if (File.Exists(pathAndFile))
        {
            FileStream loadStream = new FileStream(pathAndFile, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            object load = serializer.Deserialize(loadStream);
            if (load is Configuration)
            {
                _configuration = (Configuration) load;
#if UNITY_EDITOR
                Debug.Log("Configuration Loaded Successfully.");
#endif
                loadStream.Close();
                return;
            }
            //File is corrupt or different type
            File.Delete(path);
        }
        //File was bad or no file, make new instance.
        _configuration = new Configuration();
    }

    public static void SaveConfig()
    {
        //Don't save if the config instance is null.
        if (_configuration == null) return;

        XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        TextWriter writer = new StreamWriter(pathAndFile);
        serializer.Serialize(writer, _configuration);
        writer.Close();
    }

    public static Configuration GetConfigInstance()
    {
        if(_configuration == null) LoadConfig();
        return _configuration;
    }

    //Instance Members
    
}
