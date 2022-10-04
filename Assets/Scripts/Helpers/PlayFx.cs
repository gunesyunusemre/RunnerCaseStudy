namespace Helpers
{
    public static class PlayFx
    {
        public static void AddStack()
        {
            Haptic.Soft();
        }

        public static void RemoveStack()
        {
            Haptic.Fail();
        }

        public static void UpgradeStacks()
        {
            Haptic.Success();
        }

        public static void Finish()
        {
            Haptic.Success();
        }
    }
}