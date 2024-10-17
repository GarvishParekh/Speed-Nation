// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("1WgK9LtYlpBrsggeNzZ++C8jGyiN59yvCAmhEoxzUCfWMxYzkPuyskDDzcLyQMPIwEDDw8Jd92d9qBg3mDmMB5I+DVhW4JRhhs/JBxsGHg3yQMPg8s/Ey+hEikQ1z8PDw8fCwczOy1d/6nhEN2P9RdGISd1bjs61kejbCcb0rbEw0r7ogCVYFRgN3jZxX5GrBQSRB3y6a20/FQ9Du4GA3yzvr0ZjPXddpua/piCwQaOuopj06N2tGz5WkYpelq/HqwYBSWMCY7fLVKdT7UVy03jNZRTEYhCTxmNiMQBOkL3iHywhOo5laINdQF/i/SeK/UbS0xT0EG7WWMO/RT0SHFfSfmq931ZfYpv133xQz8XjJim9KKG+xEaBdzo+1DbUc8DBw8LD");
        private static int[] order = new int[] { 8,7,7,6,8,11,8,7,12,13,10,11,13,13,14 };
        private static int key = 194;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
