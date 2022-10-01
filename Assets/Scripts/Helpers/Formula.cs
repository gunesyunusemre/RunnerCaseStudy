using UnityEngine;

namespace Helpers
{
    public class Formula
    {
        public static double Map(double x, double inMin, double inMax, double outMin, double outMax) => (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        
        public static Vector3 SmoothCurveX(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(Func(t) + Mathf.Lerp(start.x, end.x, t), mid.y, mid.z);
        }

        public static Vector3 SmoothCurveY(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector3 SmoothCurveZ(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, mid.y, Func(t) + Mathf.Lerp(start.z, end.z, t));
        }
    }
}