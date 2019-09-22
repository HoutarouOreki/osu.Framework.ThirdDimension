using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Framework.ThirdDimension
{
    public class DrawableTriangle : Sprite
    {
        private Triangle3D tri;

        public DrawableTriangle(Triangle3D triangle3d)
        {
            Texture = Texture.WhitePixel;
            tri = triangle3d;
            Width = 500;
            Height = 600;
        }

        public override RectangleF BoundingBox => ToTriangle(ToParentSpace(LayoutRectangle)).AABBFloat;

        private static Triangle ToTriangle(Quad q) => new Triangle(
            (q.TopLeft + q.TopRight) / 2,
            q.BottomLeft,
            q.BottomRight);

        protected override DrawNode CreateDrawNode() => new TriangleDrawNode(this);

        private class TriangleDrawNode : SpriteDrawNode
        {
            private Triangle tri;

            public TriangleDrawNode(DrawableTriangle source) : base(source) => tri = new Triangle(source.tri.P1.Xy, source.tri.P2.Xy, source.tri.P3.Xy);

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                DrawTriangle(Texture, tri, DrawColourInfo.Colour, null, null,
                    new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height));
            }
        }
    }
}
