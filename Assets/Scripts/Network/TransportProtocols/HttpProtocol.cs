using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Core.Networking.Protocols
{
    public class HttpProtocol : MonoBehaviour
    {
        private static HttpProtocol _instance;

        public delegate void OnError(string message);

        public static void RetrieveData(string ip, System.Action<byte[]> onSuccess, OnError onError)
        {
            CheckInstance();
            _instance.StartCoroutine(_instance.GetData(ip, onSuccess, onError));
        }

        public static void RetrieveStringData(string ip, System.Action<string> onSuccess, OnError onError)
        {
            CheckInstance();
            _instance.StartCoroutine(_instance.GetData(ip, onSuccess, onError));
        }

        IEnumerator GetData(string ip, System.Action<byte[]> onSuccess, OnError onError)
        {
            yield return GetResponce(ip, out UnityWebRequest www);

            if (www.isNetworkError || www.isHttpError)
            {
                onError?.Invoke($"HttpProtocol Received {www.error}");
                yield break;
            }

            byte[] results = www.downloadHandler.data;
            onSuccess?.Invoke(results);
        }

        IEnumerator GetData(string ip, System.Action<string> onSuccess, OnError onError)
        {
            yield return GetResponce(ip, out UnityWebRequest www);

            if (www.isNetworkError || www.isHttpError)
            {
                onError?.Invoke($"HttpProtocol Received {www.error}");
                yield break;
            }

            string results = www.downloadHandler.text;
            onSuccess?.Invoke(results);
        }

        public static YieldInstruction GetResponce(string ip, out UnityWebRequest www)
        {
            www = UnityWebRequest.Get(ip);
            return www.SendWebRequest();
        }

        private static void CheckInstance()
        {
            if (_instance == null)
            {
                _instance = ComponentUtils.CreateGameObjectWithComponent<HttpProtocol>();
                _instance.gameObject.name = nameof(HttpProtocol);
                DontDestroyOnLoad(_instance.gameObject);
            }
        }
    }
}
