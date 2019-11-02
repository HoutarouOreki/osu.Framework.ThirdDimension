using osu.Framework.Graphics.Primitives;

namespace WavefrontPars
{
    public static class RectangleExtensions
    {
        public static bool Contains(this RectangleF rectangle, Triangle triangle)
        {
            var u1 = rectangle.TopRight.X - rectangle.TopLeft.X;
            var v1 = rectangle.TopRight.Y - rectangle.TopLeft.Y;
            var u2 = rectangle.BottomLeft.X - rectangle.TopLeft.X;
            var v2 = rectangle.BottomLeft.Y - rectangle.TopLeft.Y;
            var a1 = (rectangle.TopLeft.X * u1) + (rectangle.TopLeft.Y * v1);
            var b1 = (rectangle.TopRight.X * u1) + (rectangle.TopRight.Y * v1);
            var a2 = (rectangle.TopLeft.X * u2) + (rectangle.TopLeft.Y * v2);
            var b2 = (rectangle.BottomLeft.X * u2) + (rectangle.BottomLeft.Y * v2);
            foreach (var point in triangle.GetVertices())
            {
                var p1 = (point.X * u1) + (point.Y * v1);
                var p2 = (point.X * u2) + (point.Y * v2);
                if (a1 <= p1 && p1 <= b1 && a2 <= p2 && p2 <= b2)
                    return true;
            }
            return false;
        }
    }
}
