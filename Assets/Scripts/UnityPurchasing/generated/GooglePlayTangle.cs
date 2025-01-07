// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3beM/1hZ8ULcIwB3hmNGY8Cr4uIhD8H7VVTBVyzqOz1vRV8T69HQj5yemwcvuigUZzOtFYHYGY0L3p7lfL//FjNtJw32tu/2cOAR8/7yyKQQk52SohCTmJAQk5OSDac3LfhIZ7iN/UtuBsHaDsb/l/tWURkzUjPnwbiLWZak/eFggu640HUIRUhdjmZQHsDtsk98cWreNTjTDRAPsq132oU4WqTrCMbAO+JYTmdmLqh/c0t4mwT3A70VIoMonTVElDJAw5YzMmHtjwYPMsuljywAn5WzdnntePHulK0WgoNEpEA+hgiT7xVtQkwHgi46ohCTsKKflJu4FNoUZZ+Tk5OXkpHIadxXwm5dCAawxDHWn5lXS1ZOXRbRJ2puhGaEI5CRk5KT");
        private static int[] order = new int[] { 7,2,11,12,11,13,11,13,8,10,12,12,13,13,14 };
        private static int key = 146;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
