using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Helpers
{
    //https://www.youtube.com/watch?v=GfDiX2OSkQA
    public static class Haptic
    {
        private static float _hapticTime = 0;
        private static float _threshold = .5f;
        
        public static void Success()
        {
            if (CheckHapticThreshold())
                return;

            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        
        public static void Fail()
        {
            if (CheckHapticThreshold())
                return;
            MMVibrationManager.Haptic(HapticTypes.Failure);
        }
        
        public static void Soft()
        {
            if (CheckHapticThreshold())
                return;
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }
        
        public static void Medium()
        {
            if (CheckHapticThreshold())
                return;
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
        
        public static void Heavy()
        {
            if (CheckHapticThreshold())
                return;
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
        
        public static void Selection()
        {
            if (CheckHapticThreshold())
                return;
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }

        private static bool CheckHapticThreshold()
        {
            var check = Time.time - _hapticTime < _threshold;
            if (!check)
                _hapticTime = Time.time;
            
            return false;
        }
    }
}