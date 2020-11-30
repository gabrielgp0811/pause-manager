using UnityEngine;

public class Rotation : MonoBehaviour
{
	public float speed = 100f;

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(new Vector3(0f, 0f, -speed * Time.deltaTime));
	}
}