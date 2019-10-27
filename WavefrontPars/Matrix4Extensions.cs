using osuTK;

namespace WavefrontPars
{
    public static class Matrix4Extensions
    {
        public static Vector3 MultiplyVector3(this Matrix4 m, Vector3 v)
        {
            var o = new Vector3
            {
                X = (v.X * m[0, 0]) + (v.Y * m[1, 0]) + (v.Z * m[2, 0]) + m[3, 0],
                Y = (v.X * m[0, 1]) + (v.Y * m[1, 1]) + (v.Z * m[2, 1]) + m[3, 1],
                Z = (v.X * m[0, 2]) + (v.Y * m[1, 2]) + (v.Z * m[2, 2]) + m[3, 2],
            };
            var w = (v.X * m[0, 3]) + (v.Y * m[1, 3]) + (v.Z * m[2, 3]) + m[3, 3];

            if (w != 0)
            {
                o.X /= w;
                o.Y /= w;
                o.Z /= w;
            }

            return o;
        }
    }
}
