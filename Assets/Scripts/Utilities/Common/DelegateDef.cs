using System.Collections;
using UnityEngine;

namespace CommonUtils {
	public delegate float del_floatGetter();
	public delegate Vector3 del_Vec3Getter();
	public delegate Vector3 del_Vec3GetterVec3Param(in Vector3 r);
}