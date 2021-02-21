﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotate : MonoBehaviour
{

	public float angle = 1;
	public bool rot = true;

	void LateUpdate()
	{
		if (rot)
		{
			transform.rotation *= Quaternion.AngleAxis(angle, Vector3.back);
		}
		else
		{
			transform.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}