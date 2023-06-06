using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteConfigTemplate
{
    [System.Serializable]
    public class Condition
    {
        public string name;
        public string expression;
    }

    [System.Serializable]
    public class UpdateUser
    {
        public string email;
    }

    [System.Serializable]
    public class Version
    {
        public string versionNumber;
        public System.DateTime updateTime;
        public UpdateUser updateUser;
        public string updateOrigin;
        public string updateType;
    }

    [System.Serializable]
    public class DefaultValue
    {
        public string value;
        public bool useInAppDefault;
    }

    public Dictionary<string, Dictionary<string, DefaultValue>> parameters = new Dictionary<string, Dictionary<string, DefaultValue>>();
    public List<Condition> conditions = new List<Condition>();
    public Version version = new Version();
}
