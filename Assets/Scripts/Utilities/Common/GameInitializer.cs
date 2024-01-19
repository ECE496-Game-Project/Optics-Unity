using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Profiles;
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
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
