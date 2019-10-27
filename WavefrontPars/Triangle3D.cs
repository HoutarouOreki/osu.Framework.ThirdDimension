using osuTK;

namespace WavefrontPars
{
    public struct Triangle3D
    {
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;

        public Vector3 Normal => Vector3.Cross(P2 - P1, P3 - P1).Normalized();

        public Triangle3D(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public static Triangle3D operator +(Triangle3D tri, Vector3 v) => new Triangle3D
        {
            P1 = tri.P1 + v,
            P2 = tri.P2 + v,
            P3 = tri.P3 + v
        };

        public static Triangle3D operator *(Triangle3D tri, Matrix4 m) => new Triangle3D
        {
            P1 = m.MultiplyVector3(tri.P1),
            P2 = m.MultiplyVector3(tri.P2),
            P3 = m.MultiplyVector3(tri.P3)
        };

        public static Triangle3D operator *(Triangle3D tri, Vector3 v) => new Triangle3D
        {
            P1 = tri.P1 * v,
            P2 = tri.P2 * v,
            P3 = tri.P3 * v
        };
    }
}
