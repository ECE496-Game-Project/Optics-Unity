using UnityEngine;
using ParameterTransfer;
using System.Collections.Generic;

namespace Profiles {
	[CreateAssetMenu(menuName = "Wave/New Param UI Profile")]
	public class SO_ParamTransfer : ScriptableObject {
		public List<ParameterInfoBase> List;
	}
}