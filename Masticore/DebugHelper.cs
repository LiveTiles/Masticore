namespace Masticore
{
    public static class DebugHelper
    {
        public static bool IsDebug =>
#if DEBUG
            true;
#else
            false;
#endif
    }
}
