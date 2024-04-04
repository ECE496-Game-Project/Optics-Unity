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

/*
 * Data to Save and Load
 * 1. Wave Source parameter
 * 
 * 2. Track Devices List
 * 2.1. Device Position
 * 2.2. Device Type
 * 2.3. Device Parameter
 *
 * 3. Tutorial Variable
 * 
 * 4. Self Create Scene List 
 */