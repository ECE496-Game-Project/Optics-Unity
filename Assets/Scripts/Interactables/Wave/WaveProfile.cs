using UnityEditor;
using UnityEngine;

namespace Wave {
	[CreateAssetMenu(menuName = "Wave/New WaveProfile")]
	public class WaveProfile : ScriptableObject {
		public WaveParam waveParam;
	}
}