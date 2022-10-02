using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Stacks
{
    public class StackParentConfigurate : MonoBehaviour
    {
        [SerializeField] private Rigidbody follow;
        [SerializeField] private ConfigurableJoint blueprint;
        [SerializeField] private int count;
        [SerializeField] private List<ConfigurableJoint> configurableJoints;
        [SerializeField] private float distance;

        private void Awake()
        {
            transform.parent = null;
        }

        [Button()]
        private void Configurate()
        {
            foreach (var configurableJoint in configurableJoints)
            {
                if (configurableJoint == null)
                    continue;
                DestroyImmediate(configurableJoint.gameObject);
            }
            configurableJoints.Clear();
            
            for (int i = 0; i < count; i++)
            {
                Transform parent = i <= 0 ? transform : configurableJoints[i -1].transform;
                var instance = Instantiate(blueprint, parent);
                instance.gameObject.name = i.ToString();
                
                var instanceTransform = instance.transform;
                var pos = instanceTransform.localPosition;
                pos.y = i == 0 ? 0 : distance;
                instanceTransform.localPosition = pos;

                Rigidbody connectedBody = i <= 0 ? follow : configurableJoints[i -1].transform.GetComponent<Rigidbody>();
                instance.connectedBody = connectedBody;
                
                configurableJoints.Add(instance);
            }
        }
    }
}
