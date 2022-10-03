using System;
using Dreamteck.Splines;
using Helpers;
using Managers;
using UnityEngine;

public class Level : MonoBehaviour
{
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float initialDistance;
        
        
        
        private LevelManagerEvents levelManagerEvents;
        public void Construct(LevelManagerEvents managerEvents)
        {
                levelManagerEvents = managerEvents;
        }

        private void Start()
        {
                levelManagerEvents.FireOnLevelStarted(splineComputer, initialDistance);
        }
}