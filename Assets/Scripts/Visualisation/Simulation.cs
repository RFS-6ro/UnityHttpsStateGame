using System;
using System.Collections;
using Core.Data;
using Core.World;
using FlatBuffers;
using GUI.ViewModel;
using JetBrains.Annotations;
using Serialization.Physics;
using UnityEngine;
using Utils.Extensions;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Core.Visualisation
{
	//TODO: move to simulation manager
	public class Simulation : MonoBehaviour
	{
		private int _id;

		private EntityManager _entityManager;
		private InputSender _inputSender;
		private BoundaryManager _boundaryManager;

		private GameObject _groundGO;

		private readonly WaitForSeconds _deltaTime = new WaitForSeconds(0.0333f);
		private readonly WaitForSeconds _inputAppliance = new WaitForSeconds(9f);
		private readonly WaitForSeconds _nextRetrieveTime = new WaitForSeconds(1f);

		public void Initialize(
			int id,
			[NotNull] EntityManager entityManager,
			[NotNull] InputSender inputSender,
			[NotNull] BoundaryManager boundaryManager)
		{
			_id = id;
			_entityManager = entityManager;
			_inputSender = inputSender;
			_boundaryManager = boundaryManager;
		}

		public void Show(byte[] data, Action onEnd)
		{
			StartCoroutine(SimulationLoop(data, onEnd));
		}

		private IEnumerator SimulationLoop(byte[] data, Action onEnd)
		{
			var resultPeriod = BattlePeriod.GetRootAsBattlePeriod(new ByteBuffer(data));
#if UNITY_EDITOR
			Debug.Log($"Frames Length = {resultPeriod.FramesLength}");
#endif
			SetGround(resultPeriod.Ground);

			for (int i = 0; i < resultPeriod.FramesLength; i++)
			{
				var frame = resultPeriod.Frames(i);
				float radius = frame?.AreaRadius ?? 150f;
				_boundaryManager.SetRadius(radius);

				for (int j = 0; j < frame?.PlayersLength; j++)
				{
					var player = frame?.Players(j);
					var id = player?.Id ?? _id;

					Vector3 positionFromServer = player?.Position?.ToUnityVector3() ?? Vector3.zero;
					Quaternion rotationFromServer = player?.Rotation?.ToUnityQuaternion() ?? Quaternion.identity;
					Entity entity = _entityManager.GetEntity(id);
					entity.GetComponent<PlayerPointer>().Enable();
					entity.SetPositionAndRotation(positionFromServer, rotationFromServer);
				}

				yield return _deltaTime;
			}

			_inputSender.StartInput(CheckPlayerInputAvaliability(resultPeriod));
			yield return _inputAppliance; //TODO: show input read time
			_inputSender.StopInput();
			yield return _nextRetrieveTime; //TODO: show sync time
			onEnd.SafeInvoke();
		}

		private bool CheckPlayerInputAvaliability(BattlePeriod resultPeriod)
		{
			for (int i = 0; i < resultPeriod.PlayersInfoLength; i++)
			{
				var playerInfo = resultPeriod.PlayersInfo(i);
				if (playerInfo?.Id != _id || playerInfo?.CollectInput == true) continue;
				return false;
			}

			return true;
		}

		private void SetGround(Ground? ground)
		{
			if (ground == null)
			{
				//TODO: show plane
				return;
			}

			try
			{
				if (_groundGO != null)
				{
					if (_groundGO.name != ground?.Model)
					{
						Destroy(_groundGO);
						_groundGO = LoadGround(ground?.Model, ground?.Scale ?? 450f);
					}
				}
				else
				{
					_groundGO = LoadGround(ground?.Model, ground?.Scale ?? 450f);
				}
			}
			catch
			{
				Debug.LogError($"Wrong ground naming or missing prefab {ground?.Model}");
				//TODO: show plane
			}
		}

		private GameObject LoadGround(string model, float scale)
		{
			//TODO: consider replace with addressables
			GameObject groundModelPrefab = Resources.Load($@"Prefabs/{model}") as GameObject;
			GameObject groundModelGO = Instantiate(groundModelPrefab);
			groundModelGO.transform.position = Vector3.zero;
			groundModelGO.transform.localScale = Vector3.one * scale;
			return groundModelGO;
		}
	}
}
