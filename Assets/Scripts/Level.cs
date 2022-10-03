using System;
using System.Collections;
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

        private IEnumerator Start()
        {
                yield return new WaitForSeconds(.5f);
                levelManagerEvents.FireOnLevelStarted(splineComputer, initialDistance);
        }
}