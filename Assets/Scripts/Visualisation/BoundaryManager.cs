using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Visualisation
{
    public class BoundaryManager : MonoBehaviour
    {
        [SerializeField] [NotNull] private Transform _bound;

        public void SetBound(Transform bound)
        {
            _bound = bound;
        }

        public void SetRadius(float radius)
        {
            _bound.transform.localScale = Vector3.one * radius;
        }
    }
}

