using System;
using UnityEngine;

namespace Core.Data
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private int _id;
        public int Id => _id;

        [SerializeField] private string _name = "Uninited";
        public string Name => _name;

        public void Initialize(int id)
        {
            Debug.Log($"initialized with id {id}");
            Initialize(id, id.ToString());
        }

        public void Initialize(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }
}
