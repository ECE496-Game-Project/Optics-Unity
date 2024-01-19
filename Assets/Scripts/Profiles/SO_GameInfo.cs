using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Profiles{
    [CreateAssetMenu(fileName = "GameInfo", menuName = "SO Profile/New Game Info", order = 1)]
    public class SO_GameInfo : ScriptableObject {
        public enum PAGENAME {
            Home = 1,
            Free_Scene = 2,
            Tutorial = 3
        }

        public PAGENAME currPage = PAGENAME.Home;
    }
}