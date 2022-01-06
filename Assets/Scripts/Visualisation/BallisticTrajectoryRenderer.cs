using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Visualisation
{
    [RequireComponent(typeof(LineRenderer))]
    public class BallisticTrajectoryRenderer : MonoBehaviour
    {
        //cache
        private LineRenderer _renderer;

        private Vector3 _startPosition;
        private Vector3 _startVelocity;

        [SerializeField] private Transform _hitIndicator;

        // Step distance for the trajectory
        [SerializeField] private float trajectoryVertDist = 0.25f;

        // Max length of the trajectory
        [SerializeField] private float maxCurveLength = 5;

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            _hitIndicator?.gameObject.SetActive(true);
        }

        public void SetBallisticValues(Vector3 startPosition, Vector3 startVelocity)
        {
            _startPosition = startPosition;
            _startVelocity = startVelocity;
        }

        public void DrawTrajectory()
        {
            // Create a list of trajectory points
            var curvePoints = new List<Vector3>();
            curvePoints.Add(_startPosition);

            // Initial values for trajectory
            var currentPosition = _startPosition;
            var currentVelocity = _startVelocity;

            // Init physics variables
            RaycastHit hit;

            Ray ray = new Ray(currentPosition, currentVelocity.normalized);

            // Loop until hit something or distance is too great
            while (!Physics.Raycast(ray, out hit, trajectoryVertDist) &&
                   Vector3.Distance(_startPosition, currentPosition) < maxCurveLength)
            {
                // Time to travel distance of trajectoryVertDist
                var t = trajectoryVertDist / currentVelocity.magnitude;

                // Update position and velocity
                currentVelocity = currentVelocity + t * Physics.gravity;
                currentPosition = currentPosition + t * currentVelocity;

                // Add point to the trajectory
                curvePoints.Add(currentPosition);

                // Create new ray
                ray = new Ray(currentPosition, currentVelocity.normalized);
            }

            // If something was hit, add last point there
            if (hit.transform)
            {
                _hitIndicator.gameObject.SetActive(true);
                _hitIndicator.position = hit.point;
                curvePoints.Add(hit.point);
            }
            else
            {
                _hitIndicator.gameObject.SetActive(false);
            }

            // Display line with all points
            _renderer.positionCount = curvePoints.Count;
            _renderer.SetPositions(curvePoints.ToArray());
        }

        public void ClearTrajectory()
        {
            // Hide line
            _renderer.positionCount = 0;
        }

        private void OnDisable()
        {
            _hitIndicator?.gameObject.SetActive(false);
        }
    }
}
