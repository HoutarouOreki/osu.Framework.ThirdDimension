using System.Numerics;

namespace osu.Framework.ThirdDimension
{
    public struct Triangle2D
    {
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        public Triangle2D(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
    }
}
