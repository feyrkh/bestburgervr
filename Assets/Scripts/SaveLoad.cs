using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoad
{

    //it's static so we can call it from anywhere
    public static void Save(Object saveGame, string filename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(UnityEngine.Application.persistentDataPath + "/" + filename); //you can call it anything you want
        bf.Serialize(file, saveGame);
        file.Close();
    }

    public static T Load<T>(string filename)
    {
        if (File.Exists(UnityEngine.Application.persistentDataPath + "/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(UnityEngine.Application.persistentDataPath + "/" + filename, FileMode.Open);

            try
            {
                T deserialized = (T)bf.Deserialize(file);
                return deserialized;
            }
            finally
            {
                file.Close();
            }
        }
        return default(T);
    }
}