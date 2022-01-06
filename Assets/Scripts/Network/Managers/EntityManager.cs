using Core.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Core.World
{
    public class EntityManager
    {
        private Dictionary<int, Entity> _entities;

        private Entity _prefab;

        public EntityManager(Entity prefab)
        {
            _entities = new Dictionary<int, Entity>();
            _prefab = prefab;
        }

        public bool Contains(int id) => _entities.ContainsKey(id);

        public Entity GetEntity(int id) => Contains(id) ? _entities[id] : RegisterNewEntity(id);

        public Entity RegisterNewEntity(int id)
        {
            if (Contains(id))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Player with id {id} is already registered");
#endif

                return GetEntity(id);
            }

            Entity newEntity = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, null);
            newEntity.Initialize(id);
            _entities.Add(id, newEntity);
            return GetEntity(id);
        }

        public void UnregisterEntity(int id)
        {
            Entity entityToRemove = GetEntity(id);

            if (entityToRemove == null)
            {
                return;
            }

            //TODO: pass event for destroying.
            //TODO: add new DestroyComponent instead of manually destroying
            GameObject.Destroy(entityToRemove.gameObject);
        }
    }
}
