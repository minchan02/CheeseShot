using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DataBase
{
    // Cash
    public long cash;

    // Upgrade
    public int upgrade;

    // Todo
}

public static class DBManager
{

    public static bool loaded = false;

    private static DataBase db;
    public static DataBase DB
    {
        get
        {
            if (db == null)
            {
                Init();
            }
            return db;
        }
        private set
        {
            db = value;
        }
    }

    public static void Init()
    {
        if (File.Exists(Application.persistentDataPath + "/data.save")) Load();
        else Setup();
    }

    static void Load()
    {
        Debug.Log("Loading");
        using (FileStream file = File.Open(Application.persistentDataPath + "/data.save", FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try { db = formatter.Deserialize(file) as DataBase; }
            catch (Exception e) { Debug.LogException(e); return; }
        }
        Debug.Log("Loaded");
    }

    public static void Save()
    {
        Debug.Log("Saving");
        using (FileStream stream = File.Create(Application.persistentDataPath + "/data.save"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try { formatter.Serialize(stream, DB); }
            catch (Exception e) { Debug.LogException(e); return; }
        }
        Debug.Log("Saved");
    }

    static void Setup()
    {
        Debug.Log("Creating New DB");
        db = new DataBase();

        // Todo
    }

}
