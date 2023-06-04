using UnityEditor;
using UnityEngine;

namespace CommonUtils {
	[CreateAssetMenu(menuName = "Wave/New WaveProfile")]
	public class WaveProfile : ScriptableObject {
		public WaveParam waveParam;
	}
}