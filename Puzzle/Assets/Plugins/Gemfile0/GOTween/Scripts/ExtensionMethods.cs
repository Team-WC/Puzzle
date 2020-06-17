﻿using System.Collections.Generic;
using UnityEngine;
using System;

public static class ExtensionMethods 
{
	public static GOTween GOLocalMove(this Transform transform, Vector3 endValue, float duration)
	{
		return GOTween.To(
			() => transform.localPosition, 
			value => transform.localPosition = value, 
			endValue,
			duration
		);
	
	}
	public static GOTween GOMove(this Transform transform, Vector3 endValue, float duration)
	{
		return GOTween.To(
			() => transform.position, 
			value => transform.position = value, 
			endValue,
			duration
		);
	}
}   