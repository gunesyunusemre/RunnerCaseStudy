using MoreMountains.NiceVibrations;

namespace Helpers
{
    public static class Haptic
    {
        public static void Success()
        {
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        
        public static void Fail()
        {
            MMVibrationManager.Haptic(HapticTypes.Failure);
        }
        
        public static void Soft()
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }
        
        public static void Medium()
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
        
        public static void Heavy()
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
        
        public static void Selection()
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}