using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using WavefrontPars;

namespace osu.Framework.ThirdDimension
{
    public class SpatialSpaceDisplay : Sprite
    {
        public Vector3 CameraPosition = new Vector3(0, 0, -3);
        public Vector3 CameraRotation = Vector3.Zero;

        public float FNear = 0.1f;
        public float FFar = 500f;
        public float Fov = 90;
        public float AspectRatio => Width / Height;
        public float FovRad => 1f / (float)Math.Tan(Fov * 0.5f / 180 * Math.PI);

        public SpatialSpaceDisplay() => Texture = Texture.WhitePixel;

        private Mat4x4 matProj;

        public List<Object3D> Meshes = new List<Object3D>();

        protected override void LoadComplete()
        {
            var cube = Object3DSamples.GetNewCube();
            //cube.Translation = new Vector3(0, 0, 3);
            Meshes.Add(cube);
            cube = Object3DSamples.GetNewCube();
            Meshes.Add(cube);
            //{ Translation = new Vector3(0.5f, 1f, 2) });
            var plane = Object3DSamples.GetNewPlane();
            Meshes.Add(plane);
            plane.Translation = new Vector3(0, 2, 2);

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
            //    base.Update();
            //var mesh = Meshes[0];
            //    mesh.Translation.X = (float)Math.Sin((float)Time.Current / 1000) - 0.5f;
            //    mesh.Translation.Y = (float)Math.Sin((float)Time.Current / 1521) - 0.5f;
            //    mesh.Translation.Z = (float)Math.Sin((float)Time.Current / 876);
            var mesh = Meshes[1];
            mesh.Translation.X = (float)Math.Sin((float) Time.Current / 400) - 0.5f;
            mesh.Translation.Y = (float)Math.Sin((float) Time.Current / 721) - 0.5f;
            mesh.Translation.Z = (float)Math.Sin((float) Time.Current / 1276);
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

                var matRotZ = new Mat4x4();
                var matRotX = new Mat4x4();
                var fTheta = (float)source.Time.Current * 0.001f;

                // rotZ
                matRotZ.M00 = (float)Math.Cos(fTheta);
                matRotZ.M01 = (float)Math.Sin(fTheta);
                matRotZ.M10 = -(float)Math.Sin(fTheta);
                matRotZ.M11 = (float)Math.Cos(fTheta);
                matRotZ.M22 = 1;
                matRotZ.M33 = 1;

                // rotX
                matRotX.M00 = 1;
                matRotX.M11 = (float)Math.Cos(fTheta * 0.5f);
                matRotX.M12 = (float)Math.Sin(fTheta * 0.5f);
                matRotX.M21 = -(float)Math.Sin(fTheta * 0.5f);
                matRotX.M22 = (float)Math.Cos(fTheta * 0.5f);
                matRotX.M33 = 1;

                foreach (var mesh in source.Meshes)
                {
                    foreach (var tri in mesh.Triangles)
                    {
                        var triProjected = new Triangle3D();

                        var ttri = new Triangle3D(tri.P1 + mesh.Translation, tri.P2 + mesh.Translation, tri.P3 + mesh.Translation);

                        var triRotatedZ = new Triangle3D();
                        var triRotatedZX = new Triangle3D();

                        if (mesh.Triangles.Count > 2)
                        {
                            Mat4x4.MultiplyMatrixVector(ttri.P1, out triRotatedZ.P1, matRotZ);
                            Mat4x4.MultiplyMatrixVector(ttri.P2, out triRotatedZ.P2, matRotZ);
                            Mat4x4.MultiplyMatrixVector(ttri.P3, out triRotatedZ.P3, matRotZ);

                            Mat4x4.MultiplyMatrixVector(triRotatedZ.P1, out triRotatedZX.P1, matRotX);
                            Mat4x4.MultiplyMatrixVector(triRotatedZ.P2, out triRotatedZX.P2, matRotX);
                            Mat4x4.MultiplyMatrixVector(triRotatedZ.P3, out triRotatedZX.P3, matRotX);
                        }
                        else
                            triRotatedZX = ttri;

                        var line1 = triRotatedZX.P2 - triRotatedZX.P1;

                        var line2 = triRotatedZX.P3 - triRotatedZX.P1;

                        var absNormal = Vector3.Cross(line1, line2);

                        var triTranslated = new Triangle3D(triRotatedZX.P1 - source.CameraPosition, triRotatedZX.P2 - source.CameraPosition, triRotatedZX.P3 - source.CameraPosition);

                        // after camera normals
                        line1 = triTranslated.P2 - triTranslated.P1;

                        line2 = triTranslated.P3 - triTranslated.P1;

                        var cameraNormal = Vector3.Cross(line1, line2).Normalized();

                        if (Vector3.Dot(cameraNormal, triTranslated.P1) > 0)
                            continue;

                        // light
                        var lightDirection = new Vector3(0, -0.5f, -0.2f);
                        lightDirection.Normalize();

                        const float lightAmountModifier = 0.7f;
                        var lightAmount = (Vector3.Dot(absNormal, lightDirection) + 1) * 0.5f;
                        lightAmount *= lightAmountModifier;

                        Mat4x4.MultiplyMatrixVector(triTranslated.P1, out triProjected.P1, source.matProj);
                        Mat4x4.MultiplyMatrixVector(triTranslated.P2, out triProjected.P2, source.matProj);
                        Mat4x4.MultiplyMatrixVector(triTranslated.P3, out triProjected.P3, source.matProj);

                        var priTri = new Graphics.Primitives.Triangle(new Vector2(triProjected.P1.X, triProjected.P1.Y) + Vector2.One, new Vector2(triProjected.P2.X, triProjected.P2.Y) + Vector2.One, new Vector2(triProjected.P3.X, triProjected.P3.Y) + Vector2.One);

                        priTri = new Graphics.Primitives.Triangle(priTri.P0 * source.DrawSize * 0.5f,
                            priTri.P1 * source.DrawSize * 0.5f,
                            priTri.P2 * source.DrawSize * 0.5f);

                        DrawTriangle(Texture, priTri,
                        new Color4(lightAmount, lightAmount, lightAmount, 1), null, null, new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height));
                        DrawLine(priTri.P0, priTri.P1);
                        DrawLine(priTri.P1, priTri.P2);
                        DrawLine(priTri.P2, priTri.P0);
                    }
                }
            }

            private void DrawLine(Vector2 p1, Vector2 p2)
            {
                var directionOfLine = (p2 - p1).Normalized();
                const float lineThickness = 4;
                var pp = new Vector2(-directionOfLine.Y, directionOfLine.X) * lineThickness * 0.5f;
                DrawQuad(Texture.WhitePixel, new Graphics.Primitives.Quad(p1 - pp, p1 + pp, p2 - pp, p2 + pp), Color4.Goldenrod);
            }
        }
    }
}
