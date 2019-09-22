using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
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
        public float Fov = 70;
        public float AspectRatio => Width / Height;
        public float FovRad => 1f / MathF.Tan(Fov * 0.5f / 180 * MathF.PI);

        public SpatialSpaceDisplay() => Texture = Texture.WhitePixel;

        private Mat4x4 matProj;

        public List<Mesh> Meshes = new List<Mesh>();

        protected override void LoadComplete()
        {
            Meshes.Add(Mesh.Cube);
            Meshes.Add(Mesh.Cube);
            //{ Translation = new Vector3(0.5f, 1f, 2) });

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
            var mesh = Meshes[0];
            mesh.Translation.X = MathF.Sin((float)Time.Current / 1000) - 0.5f;
            mesh.Translation.Y = MathF.Sin((float)Time.Current / 1521) - 0.5f;
            mesh.Translation.Z = MathF.Sin((float)Time.Current / 876) + 4;
            mesh = Meshes[1];
            mesh.Translation.X = MathF.Sin((float)Time.Current / 400) - 0.5f;
            mesh.Translation.Y = MathF.Sin((float)Time.Current / 721) - 0.5f;
            mesh.Translation.Z = MathF.Sin((float)Time.Current / 1276) + 5;
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

                        var priTri = new Graphics.Primitives.Triangle(triProjected.P1.Xy + Vector2.One, triProjected.P2.Xy + Vector2.One, triProjected.P3.Xy + Vector2.One);

                        priTri = new Graphics.Primitives.Triangle(priTri.P0 * source.DrawSize * 0.5f,
                            priTri.P1 * source.DrawSize * 0.5f,
                            priTri.P2 * source.DrawSize * 0.5f);

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
                const float lineThickness = 3;
                var pp = new Vector2(-directionOfLine.Y, directionOfLine.X) * lineThickness * 0.5f;
                DrawQuad(Texture.WhitePixel, new Graphics.Primitives.Quad(p1 - pp, p1 + pp, p2 - pp, p2 + pp), Color4.Goldenrod);
            }
        }
    }
}
