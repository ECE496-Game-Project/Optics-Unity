using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using Profiles;

[InitializeOnLoad]
public static class GameInitializer
{
    static GameInitializer(){
        ResetGameInfo();
    }

    static void ResetGameInfo(){
        SO_GameInfo gameInfo = Resources.Load<SO_GameInfo>("Data/GameInfo/GameInfo");
        gameInfo.currPage = SO_GameInfo.PAGENAME.Home;    
    }
}
