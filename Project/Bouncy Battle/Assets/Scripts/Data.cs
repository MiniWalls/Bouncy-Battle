using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Data : MonoBehaviour
{
    public static void SaveProfile(ProfileData _profile)
    {
        try
        {
            string path = Application.persistentDataPath + "/profiledata.txt";

            if (File.Exists(path)) File.Delete(path);
            FileStream fs = File.Create(path);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _profile);
            fs.Close();
        }
        catch
        { Debug.Log("SOMETHING WRONG, PROFILE NOT SAVED"); }
    }

    public static ProfileData LoadProfile()
    {
            ProfileData data = new ProfileData();
        try
        {

            string path = Application.persistentDataPath + "/profiledata.txt";
            if (File.Exists(path))
            {
                FileStream fs = File.Open(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                data = (ProfileData)bf.Deserialize(fs);
            }
        }
        catch { Debug.Log("SOMETHING WRONG, PROFILE NOT LOADED"); }

        return data;
    }

    public static void SaveSettings(SettingsData _settings)
    {
        try
        {
            string path = Application.persistentDataPath + "/settingsdata.txt";

            if (File.Exists(path)) File.Delete(path);
            FileStream fs = File.Create(path);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _settings);
            fs.Close();
        }
        catch
        { Debug.Log("SOMETHING WRONG, SETTINGS NOT SAVED"); }
    }

    public static SettingsData LoadSettings()
    {
        SettingsData data = new SettingsData();

        try
        {
            string path = Application.persistentDataPath + "/settingsdata.txt";
            if(File.Exists(path))
            {
                FileStream fs = File.Open(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                data = (SettingsData)bf.Deserialize(fs);
            }
        }
        catch { Debug.Log("SOMETHING WRONG, SETTINGS NOT LOADED"); }

        return data;

    }
}
