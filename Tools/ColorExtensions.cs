using UnityEngine;

namespace Status92.Tools
{
    public static class ColorExtensions
    {

        public static Color withA(this Color color, float a)
        {
            var c = color;
            c.a = a;
            return c;
        }
        
        public static Color withR(this Color color, float r)
        {
            var c = color;
            c.r = r;
            return c;
        }
        
        public static Color withG(this Color color, float g)
        {
            var c = color;
            c.g = g;
            return c;
        }
        
        public static Color withB(this Color color, float b)
        {
            var c = color;
            c.b = b;
            return c;
        }
        
    }
}