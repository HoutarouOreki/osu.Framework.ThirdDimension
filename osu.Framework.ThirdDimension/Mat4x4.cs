using osuTK;

namespace osu.Framework.ThirdDimension
{
    public struct Mat4x4
    {
        public float M00;
        public float M01;
        public float M02;
        public float M03;

        public float M10;
        public float M11;
        public float M12;
        public float M13;

        public float M20;
        public float M21;
        public float M22;
        public float M23;

        public float M30;
        public float M31;
        public float M32;
        public float M33;

        public static void MultiplyMatrixVector(Vector3 i, out Vector3 o, Mat4x4 m)
        {
            o = new Vector3
            {
                X = (i.X * m.M00) + (i.Y * m.M10) + (i.Z * m.M20) + m.M30,
                Y = (i.X * m.M01) + (i.Y * m.M11) + (i.Z * m.M21) + m.M31,
                Z = (i.X * m.M02) + (i.Y * m.M12) + (i.Z * m.M22) + m.M32,
            };
            var w = (i.X * m.M03) + (i.Y * m.M13) + (i.Z * m.M23) + m.M33;

            if (w != 0)
            {
                o.X /= w;
                o.Y /= w;
                o.Z /= w;
            }
        }
    }
}
