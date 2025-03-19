using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private Vector3 offsetPosition;

	private void Update()
	{
		Refresh();
	}

	public void Refresh()
	{
		if(target == null)
		{
			Debug.LogWarning("Missing target ref !", this);

			return;
		}

		  Vector3 TargetPosition = target.position;
			TargetPosition.y = transform.position.y;
			transform.position = TargetPosition + offsetPosition;
	}
}
