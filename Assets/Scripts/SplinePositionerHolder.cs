using Dreamteck.Splines;
using NaughtyAttributes;
using UnityEngine;

public class SplinePositionerHolder : MonoBehaviour
{
    [SerializeField] private float yOffset;
        
    [SerializeField][Range(-3.5f, 3.5f)] private float xPosition;
    [SerializeField, ReadOnly] private SplinePositioner positioner;


    private void OnValidate()
    {
        if (positioner == null)
            positioner = GetComponent<SplinePositioner>();

        if (positioner == null)
            return;

        var offset = positioner.motion.offset;
        offset.x = xPosition;
        offset.y = yOffset;
        positioner.motion.offset = offset;
    }
}