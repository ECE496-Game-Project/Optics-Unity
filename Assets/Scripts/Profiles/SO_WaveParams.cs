using UnityEngine;
using WaveUtils;

namespace Profiles {
	[CreateAssetMenu(menuName = "SO Profile/New WaveProfile")]
	public class SO_WaveParams : ScriptableObject {
		public WaveParams Parameters;
	}
}