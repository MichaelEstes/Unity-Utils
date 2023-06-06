using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Firebase.Extensions;

public class RemoteScriptableObject : ScriptableObject
{
    [NonSerialized]
    private bool hasFinalValues = false;

#if UNITY_EDITOR
    [NonSerialized]
    private bool updateConfig = false;
#endif

    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        if (updateConfig)
        {
            RemoteConfigParams.AddRemoteConfigParam(name, JsonUtility.ToJson(this));
            Debug.Log("Writing config file");
        }
#else
        if (Application.isPlaying)
        {
            // FetchAndActivateConfig();
            ForceFetchAndActivateConfig();
        }
#endif
    }

    void FetchAndActivateConfig()
    {
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
        fetchTask.ContinueWithOnMainThread((task) =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("Unable to fetch remote config values for: " + name);
                return;
            }

            // SetConfigValues();
            SetFromJson();
        });
    }

    void ForceFetchAndActivateConfig()
    {
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        fetchTask.ContinueWithOnMainThread((task) =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("Unable to fetch remote config values for: " + name);
                return;
            }

            System.Threading.Tasks.Task activateTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            activateTask.ContinueWithOnMainThread((task) =>
            {
                if (!task.IsCompleted)
                {
                    Debug.LogError("Unable to activate remote config values for: " + name);
                    return;
                }

                // SetConfigValues();
                SetFromJson();
            });

        });
    }

    void SetFromJson()
    {
        string key = name;

        if (!Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.AllValues.ContainsKey(key))
        {
            Debug.LogWarning("Parameter not found in Firebase");
            return;
        }

        Firebase.RemoteConfig.ConfigValue configValue = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);

        string json = configValue.StringValue;
        if (!string.IsNullOrEmpty(json))
        {
            JsonUtility.FromJsonOverwrite(configValue.StringValue, this);
        }

        hasFinalValues = true;
    }

    void SetConfigValues()
    {
        // Debug.Log(name + " : " + Application.isPlaying);
        Type type = this.GetType();
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            Type fieldType = fieldInfo.FieldType;

            //Skip other RemoteScriptableObject and Enums
            if (typeof(RemoteScriptableObject).IsAssignableFrom(fieldType) || fieldType.IsEnum) continue;

            //Skip values with NonRemote attribute
            HashSet<Type> customAttributes = new HashSet<Type>();
            foreach (CustomAttributeData customAttribute in fieldInfo.CustomAttributes) customAttributes.Add(customAttribute.AttributeType);
            if (customAttributes.Contains(typeof(NonRemote))) continue;

            string key = string.Concat(name, "_", fieldInfo.Name);

            if (!Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.AllValues.ContainsKey(key)) continue;

            Firebase.RemoteConfig.ConfigValue configValue = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);

#if UNITY_EDITOR

#else
            SetRemoteValue(fieldInfo, fieldType, configValue);
#endif

        }

        hasFinalValues = true;
    }

    void SetRemoteValue(FieldInfo fieldInfo, Type fieldType, Firebase.RemoteConfig.ConfigValue configValue)
    {
        if (fieldType.IsPrimitive)
        {
            if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(this, configValue.BooleanValue);
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(this, Convert.ToInt32(configValue.LongValue));
            }
            else if (fieldType == typeof(float))
            {
                fieldInfo.SetValue(this, (float)configValue.DoubleValue);
            }
            else if (fieldType == typeof(double))
            {
                fieldInfo.SetValue(this, configValue.DoubleValue);
            }
            else
            {
                Debug.LogError("Not a supported primitive for Remote Object");
            }

        }
        else if (fieldType.IsValueType)
        {
            Debug.Log("Structs not supported");
        }
        else if (fieldType.IsClass)
        {
            if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(this, configValue.StringValue);
            }
            else
            {
                Debug.Log("Classes not supported");
            }
        }
    }

    public bool HasFinalValues()
    {
        return hasFinalValues;
    }
}