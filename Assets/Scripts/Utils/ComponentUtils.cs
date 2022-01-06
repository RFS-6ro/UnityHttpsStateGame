using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
	public class ComponentUtils : MonoBehaviour
	{
		public static IEnumerable<T> FindObjectsWithInterface<T>() =>
			FindObjectsOfType<MonoBehaviour>().OfType<T>();

		public static T CreateGameObjectWithComponent<T>()
			where T : Component =>
			new GameObject().AddComponent<T>();
	}
}
