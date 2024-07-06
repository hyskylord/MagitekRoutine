using ff14bot.Managers;

namespace Magitek.Utilities
{
    public static class MagitekActionResourceManager
    {
        public static class BlackMage
        {
            public static double PolyGlotTimer => ActionResourceManager.BlackMage.PolyglotTimer.TotalMilliseconds;
        }
        public static class DarkKnight
        {
            public static bool DarkArts => ActionResourceManager.DarkKnight.DarkArts;
        }

        public static class Ninja
        {

            public static int NinkiGauge => ActionResourceManager.Ninja.NinkiGauge;

        }
    }
}