using System;
using UnityEngine;


public class PlayerFollow : MonoBehaviour
{
    public Transform target;
	public float offcenterX = 0;
	public float offcenterY = 1.0f;

    private Vector3 m_CurrentVelocity;
	private Vector3 TargetPos;
	private bool allowFollow = true;

	public GameObject currentLevel;

	private Vector3 minBounds;
	private Vector3 maxBounds;

    
    private void Start()
    {
        transform.parent = null;
		SetCameraBounds ();
    }
		

    private void Update()
    {
		if (allowFollow) {
			TargetPos = target.position;

			TargetPos.x = Mathf.Clamp(target.position.x + offcenterX, minBounds.x, maxBounds.x);
			TargetPos.y = Mathf.Clamp(target.position.y + offcenterY, minBounds.y, maxBounds.y);
			TargetPos.z = transform.position.z;

			transform.position = TargetPos;
		}
    }

	private void SetCameraBounds(){
		Bounds bounds = new Bounds(currentLevel.transform.position, Vector3.zero);

		foreach(Renderer renderer in currentLevel.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(renderer.bounds);
		}

		minBounds = GetVertexWorldPosition (bounds.min, currentLevel.transform);
		maxBounds = GetVertexWorldPosition (bounds.max, currentLevel.transform);

		float height = 2f * Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		minBounds = new Vector3 (minBounds.x + width/2, minBounds.y + height/2);
		maxBounds = new Vector3 (maxBounds.x - width/2, maxBounds.y);
	}

	public Vector3 GetVertexWorldPosition(Vector3 vertex, Transform owner)
	{
		return owner.localToWorldMatrix.MultiplyPoint3x4(vertex);
	}

	public void StopFollowing(){
		allowFollow = false;
	}
	public void StartFollowing(){
		allowFollow = true;
	}
}

