using UnityEngine;
using CommonUtils;

namespace Profiles {
	[CreateAssetMenu(menuName = "Wave/New WaveProfile")]
	public class WaveProfile : ScriptableObject {
		public WaveParam Parameters;
	}
}