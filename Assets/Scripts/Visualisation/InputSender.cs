using Core.Networking;
using Core.Networking.Protocols;
using GUI.Input;
using GUI.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Visualisation
{
    public class InputSender : MonoBehaviour
    {
        private int _id;
        private Camera _camera;
        private Transform _renderTarget;

        private Transform _target;
        private InputListener _trajectoryListener;
        private TextView _commitButtonText;

        public float ControlSensitivityMultiplyer = 2f / 3f;
        public float yMin = -90;
        public float yMax = 90;
        public float Distance = 10.0f;

        private float _xRotation = 0;
        private float _yRotation = 0;
        private bool _started;
        private bool _commited;

        [SerializeField] private BallisticTrajectoryRenderer _updateRenderer;
        [SerializeField] private BallisticTrajectoryRenderer _commitRenderer;
        [SerializeField] private Button _inputStartIndicator;
        [SerializeField] private Slider _multiplier;

#if PHYSICS_DEBUG
        private Vector3 _nextRandomInput;
#endif

        private void Awake()
        {
            _multiplier.gameObject.SetActive(
#if PHYSICS_DEBUG
                false
#else
                true
#endif
            );

            _commitButtonText = _inputStartIndicator.GetComponentInChildren<TextView>();
            _target = new GameObject("FakeInputTarget").transform;
        }

        public void Initialize(
            int id,
            [NotNull] Transform renderTarget,
            [NotNull] Camera camera,
            [NotNull] InputListener trajectoryListener)
        {
            _id = id;
            _renderTarget = renderTarget;
            _camera = camera;
            _trajectoryListener = trajectoryListener;
        }

        private void UpdateTrajectoryDirection(Vector2 trajectoryInput)
        {
            Vector2 controlInput = Input.touchCount > 0
                ? trajectoryInput
                : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            controlInput *= ControlSensitivityMultiplyer;

            _target.rotation = CalculateTargetRotation(controlInput);
        }

        private Quaternion CalculateTargetRotation(Vector2 input)
        {
            _xRotation += Mathf.Repeat(input.x, 360.0f);
            _yRotation -= input.y;
            _yRotation = Mathf.Clamp(_yRotation, yMin, yMax);
            return Quaternion.AngleAxis(_xRotation, Vector3.up) * Quaternion.AngleAxis(_yRotation, Vector3.right);
        }

        public void StartInput(bool isInputAvaliable)
        {
            (_commitButtonText ? _commitButtonText : _inputStartIndicator.GetComponentInChildren<TextView>())
                .SetText(isInputAvaliable ? "commit input": "input declined");

            _inputStartIndicator.gameObject.SetActive(true);
            _inputStartIndicator.interactable = isInputAvaliable;

            if (isInputAvaliable == false)
            {
                return;
            }

            SetInputHandlersOnStart();
            ClearAllPreviousTrajectories();
            _trajectoryListener.InputUpdateEvent += UpdateTrajectoryDirection;
            _inputStartIndicator.onClick.AddListener(CommitAndSendInput);
            _started = true;
            _commited = false;
#if PHYSICS_DEBUG
            _nextRandomInput = UnityEngine.Random.insideUnitSphere;
#endif
        }

        private void SetInputHandlersOnStart()
        {
            _updateRenderer.gameObject.SetActive(true);
            _commitRenderer.gameObject.SetActive(false);
            _inputStartIndicator.gameObject.SetActive(true);
        }

        private void ClearAllPreviousTrajectories()
        {
            _updateRenderer.ClearTrajectory();
            _commitRenderer.ClearTrajectory();
        }

        private void LateUpdate()
        {
            if (_started == false)
            {
                return;
            }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.Space))
            {
                CommitAndSendInput();
            }
#endif

            _updateRenderer.SetBallisticValues(_renderTarget.position, GetVelocity());
            _updateRenderer.DrawTrajectory();
        }

        private Vector3 GetVelocity()
        {
            float velocityMultiplier = 50f;
            return GetDirection() * velocityMultiplier;
        }

        private Vector3 GetDirection()
        {
            float defaultMultiplierValue = 0.5f;
            float _multiplierValue = (_multiplier == null) ? defaultMultiplierValue : _multiplier.value;
            Vector3 localForward = transform.forward;

            if (_camera != null)
            {
#if PHYSICS_DEBUG
                localForward = _nextRandomInput;
#else
                localForward = _target.forward;
#endif
            }

            return localForward * _multiplierValue;
        }

        public void StopInput()
        {
            _inputStartIndicator.gameObject.SetActive(false);
            if (_started == false)
            {
                return;
            }

            _started = false;
            _updateRenderer.ClearTrajectory();

            _inputStartIndicator.onClick.RemoveListener(CommitAndSendInput);

            if (_commited == false)
            {
                CommitAndSendInput();
            }

            _updateRenderer.gameObject.SetActive(false);
            _trajectoryListener.InputUpdateEvent -= UpdateTrajectoryDirection;
        }

        public void CommitAndSendInput()
        {
            _commitRenderer.gameObject.SetActive(true);
            _commitRenderer.SetBallisticValues(_renderTarget.position, GetVelocity());
            _commitRenderer.DrawTrajectory();

            string input = GetInputAsString();
            SendPlayerInput(input);

            _commited = true;
        }

        private string GetInputAsString()
        {
            Vector3 direction = GetDirection();
            return $"({direction.x:0.###},{direction.y:0.###},{direction.z:0.###})";
        }

        private void SendPlayerInput(string input)
        {
            HttpProtocol.RetrieveData(
                $"{NetworkManager.SERVER_URL}/input?uid={_id}&direction={input}",
                inputResult =>
                {
#if UNITY_EDITOR
                    Debug.Log("successfully sended");
#endif
                },
                errorMessage =>
                {
#if UNITY_EDITOR
                    Debug.Log("Error in sending input");
#endif
                }
            );
        }
    }
}
