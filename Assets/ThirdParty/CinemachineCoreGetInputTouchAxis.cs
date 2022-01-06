using GUI.Input;
using UnityEngine;

public class CinemachineCoreGetInputTouchAxis : MonoBehaviour
{
	public float yMin = -60;
	public float yMax = 60;
	private float _xRotation = 0;
	private float _yRotation = 0;
	private Transform _target;
	public float Distance = 5.0f;

	private InputListener _cameraListener;
	private Vector2 _cameraInput;

	public Vector3 PositionOffset;
	public Vector2 RotationOffset;

	public void Init(Transform target, InputListener cameraListener)
	{
		_target = target;
		_cameraListener = cameraListener;
		_cameraListener.InputUpdateEvent += UpdateCameraInput;
	}

	void UpdateCameraInput(Vector2 cameraInput)
	{
		_cameraInput = cameraInput;
	}

	void Update()
	{
		Vector2 controlInput = _cameraInput;
		controlInput /= 3f;
		_xRotation += Mathf.Repeat(controlInput.x, 360.0f);
		_yRotation -= controlInput.y;
		_yRotation = Mathf.Clamp(_yRotation, yMin, yMax);
		Quaternion newRotation = Quaternion.AngleAxis(_xRotation + RotationOffset.x, Vector3.up);
		newRotation *= Quaternion.AngleAxis(_yRotation + RotationOffset.y, Vector3.right);
		transform.rotation = newRotation;
		transform.position = _target.position - (transform.forward * Distance) + PositionOffset;
		_cameraInput = Vector2.zero;
	}

	private void OnDestroy()
	{
		_cameraListener.InputUpdateEvent -= UpdateCameraInput;
	}
}
