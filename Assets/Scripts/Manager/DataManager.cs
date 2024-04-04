using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager{
    public static void SaveData(string key, string value){
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static string LoadData(string key){
        return PlayerPrefs.GetString(key);
    }
}
