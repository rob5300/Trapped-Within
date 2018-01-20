using UnityEngine;

public class TransformManip : MonoBehaviour
{
    public Vector3 PositionAlter;
    public Vector3 RotationAlterEuler;

	public void Update ()
	{
	    transform.position += (PositionAlter * Time.deltaTime);
	    transform.rotation = transform.rotation * Quaternion.Euler(RotationAlterEuler * Time.deltaTime);
	}
}
