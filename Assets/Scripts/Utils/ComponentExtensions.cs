using UnityEngine;

namespace Utils.Extensions
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Transform container)
            where T : Component =>
            container.gameObject.GetOrAddComponent<T>();

        public static T GetOrAddComponent<T>(this GameObject container)
            where T : Component =>
            container.GetComponent<T>() ?? container.AddComponent<T>();
    }
}
