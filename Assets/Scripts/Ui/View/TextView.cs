using System;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.View
{
	[RequireComponent(typeof(Text))]
	public class TextView : MonoBehaviour
	{
		[SerializeField] private Text _text;

		private void Awake()
		{
			if (_text == null)
			{
				_text = GetComponent<Text>();
			}
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}
