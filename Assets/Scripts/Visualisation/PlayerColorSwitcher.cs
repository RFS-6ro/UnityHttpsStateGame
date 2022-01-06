using UnityEngine;

namespace Core.Visualisation
{
    [RequireComponent(typeof(Renderer))]
    public class PlayerColorSwitcher : MonoBehaviour
    {
        public void Awake()
        {
            Vector3 color = Random.insideUnitSphere;
            GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z);
        }
    }
}
