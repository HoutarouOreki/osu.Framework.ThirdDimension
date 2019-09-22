using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace osu.Framework.ThirdDimension
{
    public class SpatialSpaceDisplay : Sprite
    {
        public Vector3 CameraPosition = Vector3.Zero;
        public Vector3 CameraRotation = Vector3.Zero;

        public float FNear = 0.1f;
        public float FFar = 500f;
        public float Fov = 90f;
        public float AspectRatio => Width / Height;
        public float FovRad => 1f / MathF.Tan(Fov * 0.5f / 180 * MathF.PI);

        public SpatialSpaceDisplay() => Texture = Texture.WhitePixel;

        private Mat4x4 matProj;

        public List<Mesh> Meshes = new List<Mesh>();

        protected override void LoadComplete()
        {
            Meshes.Add(new Mesh(new[]
            {
                new Triangle3D(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)),
                new Triangle3D(new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)),

                new Triangle3D(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1)),
                new Triangle3D(new Vector3(1, 0, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1)),

                new Triangle3D(new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1)),
                new Triangle3D(new Vector3(1, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1)),

                new Triangle3D(new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0)),
                new Triangle3D(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0)),

                new Triangle3D(new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1)),
                new Triangle3D(new Vector3(0, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 1, 0)),

                new Triangle3D(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1)),
                new Triangle3D(new Vector3(0, 0, 0), new Vector3(1, 0, 1), new Vector3(1, 0, 0)),
            })
            { Translation = new Vector3(0.5f, 1f, 2) });

            matProj.M00 = AspectRatio * FovRad;
            matProj.M11 = FovRad;
            matProj.M22 = FFar / (FFar - FNear);
            matProj.M32 = -FFar * FNear / (FFar - FNear);
            matProj.M23 = 1;
            matProj.M33 = 0;

            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();
            foreach (var mesh in Meshes)
            {
                mesh.Translation.X = MathF.Sin((float)Time.Current / 1000) + 0.5f;
            }
        }

        protected override DrawNode CreateDrawNode() => new SpatialSpaceDrawNode(this);

        private class SpatialSpaceDrawNode : SpriteDrawNode
        {
            public SpatialSpaceDrawNode(SpatialSpaceDisplay source) : base(source)
            {
            }

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                var source = Source as SpatialSpaceDisplay;
                foreach (var mesh in source.Meshes)
                {
                    foreach (var tri in mesh.Tris)
                    {
                        var triProjected = new Triangle3D();

                        var ttri = new Triangle3D(tri.P1 + mesh.Translation, tri.P2 + mesh.Translation, tri.P3 + mesh.Translation);

                        Mat4x4.MultiplyMatrixVector(ttri.P1, out triProjected.P1, source.matProj);
                        Mat4x4.MultiplyMatrixVector(ttri.P2, out triProjected.P2, source.matProj);
                        Mat4x4.MultiplyMatrixVector(ttri.P3, out triProjected.P3, source.matProj);

                        var priTri = new Graphics.Primitives.Triangle(triProjected.P1.Xy * source.DrawSize * 0.5f,
                            triProjected.P2.Xy * source.DrawSize * 0.5f,
                            triProjected.P3.Xy * source.DrawSize * 0.5f);

                        //DrawTriangle(Texture, priTri,
                        //Color4.Gray, null, null, new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height));
                        DrawLine(priTri.P0, priTri.P1);
                        DrawLine(priTri.P1, priTri.P2);
                        DrawLine(priTri.P2, priTri.P0);
                    }
                }
            }

            private void DrawLine(Vector2 p1, Vector2 p2)
            {
                var directionOfLine = (p2 - p1).Normalized();
                var pp = new Vector2(-directionOfLine.Y, directionOfLine.X);
                DrawQuad(Texture.WhitePixel, new Graphics.Primitives.Quad(p1 - pp, p1 + pp, p2 - pp, p2 + pp), Color4.Blue);
            }
        }
    }
}
