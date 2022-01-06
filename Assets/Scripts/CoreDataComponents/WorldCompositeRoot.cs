using Cinemachine;
using Core.Data;
using Core.Networking;
using Core.Networking.Protocols;
using Core.Visualisation;
using GUI;
using GUI.Input;
using GUI.ViewModel.DebugGUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace Core.World
{
	public class WorldCompositeRoot : MonoBehaviour
	{
		[SerializeField] private Entity _playerPrefab;

		[SerializeField] private GUIManager _guiManager;
		[SerializeField] private Simulation _simulation;
		[SerializeField] private InputSender _inputSender;
		[SerializeField] private BoundaryManager _boundaryManager;


		[SerializeField] private InputListener _cameraListener;
		[SerializeField] private InputListener _trajectoryListener;
		[SerializeField] private Camera _camera;

#if DEBUG
		[SerializeField] private DebugViewModel _debugViewModel;
#endif

		private void Awake()
		{
			//TODO: add check and load prefab from resources
			EntityManager entityManager = new EntityManager(_playerPrefab);
			new LoginManager().CreateId();
			new NetworkManager(LoginManager.Id, _simulation
#if DEBUG
				, _debugViewModel
#endif
			);

			//Camera
			Entity entity = entityManager.GetEntity(LoginManager.Id);
			_camera.gameObject.GetOrAddComponent<CinemachineCoreGetInputTouchAxis>().Init(entity.transform, _cameraListener);

            _simulation.Initialize(LoginManager.Id, entityManager, _inputSender, _boundaryManager);
            _inputSender.Initialize(LoginManager.Id, entity.transform, _camera, _trajectoryListener);

			//TODO: replace with prefab loading
			_guiManager.Initialize(_debugViewModel);

			//TODO: create some kind of simulation manager to handle movement visualisation and world shape
		}

		public void ResetWorld()
		{
			HttpProtocol.RetrieveData($"{NetworkManager.SERVER_URL}/reset", null, null);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
