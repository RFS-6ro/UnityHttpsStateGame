using GUI.View;
using UnityEngine;

namespace GUI.ViewModel
{
	public class SimulationStatusViewModel : MonoBehaviour, IGUIInitializer
	{
		//TODO: consider replacement with Timer class
		[SerializeField] private TextView _timerTextView;
		[SerializeField] private TextView _id;

		private bool _isEnabled = true;
		public bool IsEnabled => _isEnabled;

		public void InitializeWithRoot(GUIManager guiManager)
		{
			_id.SetText(LoginManager.Id.ToString());
		}
	}
}
