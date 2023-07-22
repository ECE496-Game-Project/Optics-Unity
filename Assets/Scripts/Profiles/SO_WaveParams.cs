using UnityEngine;
using WaveUtils;

namespace Profiles {
	[CreateAssetMenu(menuName = "Wave/New WaveProfile")]
	public class SO_WaveParams : ScriptableObject {
		public WaveParams Parameters;
	}
}