using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCameraY : MonoBehaviour
{
	public delegate void ParallaxCameraDelegate(float deltaMovement);
	public ParallaxCameraDelegate onCameraTranslate;

	private float oldPosition;

	void Start()
	{
		oldPosition = transform.position.y;
	}

	void FixedUpdate()
	{
		if (transform.position.y != oldPosition)
		{
			if (onCameraTranslate != null)
			{
				float delta = oldPosition - transform.position.y;
				onCameraTranslate(delta);
			}

			oldPosition = transform.position.y;
		}
	}
}