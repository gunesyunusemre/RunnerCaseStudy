using System.Runtime.CompilerServices;
using UnityEngine;

namespace Helpers
{
    public static class Logger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(this object msg)
        {
#if UNITY_EDITOR
            Debug.Log(msg);
#endif
        }
    }
}