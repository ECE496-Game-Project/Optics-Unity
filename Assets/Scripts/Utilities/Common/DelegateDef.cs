﻿using System.Collections;
using UnityEngine;

namespace CommonUtils {
	public delegate Vector3 del_Vec3ParamVec3Getter(in Vector3 r);
	
	public static class Del_Default{
		public static Vector3 DefaultVec3ParamVec3Getter(in Vector3 r) {
			DebugLogger.Warning("Del_Default_Methods", "del_Vec3Getter Invoke DefaultVec3ParamVec3Getter, return Vector3.zero");
			return Vector3.zero;
		}
	}
}