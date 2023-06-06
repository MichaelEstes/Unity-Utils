using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RemoteConfigParams
{
    [System.Serializable]
    public class RemoteConfigParam
    {
        public string key;
        public string value;

        public RemoteConfigParam(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public List<RemoteConfigParam> remoteConfigParams = new List<RemoteConfigParam>();

    public static void AddRemoteConfigParam(string key, string value)
    {
        string filename = "./RemoteConfigParams.json";

        RemoteConfigParams tempRemoteConfigParams;
        if (File.Exists(filename))
        {
            FileStream dataStream = new FileStream(filename, FileMode.Open);
            StreamReader reader = new StreamReader(dataStream);
            string text = reader.ReadToEnd();
            dataStream.Close();
            reader.Close();
            tempRemoteConfigParams = JsonUtility.FromJson<RemoteConfigParams>(text);
        }
        else
        {
            tempRemoteConfigParams = new RemoteConfigParams();
        }

        foreach (RemoteConfigParam configParam in tempRemoteConfigParams.remoteConfigParams)
        {
            if (configParam.key == key)
            {
                tempRemoteConfigParams.remoteConfigParams.Remove(configParam);
            }

        }

        tempRemoteConfigParams.remoteConfigParams.Add(new RemoteConfigParam(key, value));

        FileStream createDataStream = new FileStream(filename, FileMode.Create);
        StreamWriter sWriter = new StreamWriter(createDataStream);

        string jsonString = JsonUtility.ToJson(tempRemoteConfigParams);
        sWriter.Write(jsonString);
        sWriter.Close();
        createDataStream.Close();
    }
}
