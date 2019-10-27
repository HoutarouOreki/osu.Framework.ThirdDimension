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
        public Quaternion CameraRotation = new Quaternion(new Vector3(0));

        private float fNear = 0.1f;
        private float fFar = 500f;
        private float fov = 90;

        private Mat4x4 projectionMatrix;

        public float FNear { get => fNear; set { fNear = value; UpdateProjectionMatrix(); } }
        public float FFar { get => fFar; set { fFar = value; UpdateProjectionMatrix(); } }
        public float Fov { get => fov; set { fov = value; UpdateProjectionMatrix(); } }
        public float AspectRatio => DrawWidth / DrawHeight;
        public float FovRad => 1f / (float)Math.Tan(Fov * 0.5f / 180 * Math.PI);

        public SpatialSpaceDisplay() => Texture = Texture.WhitePixel;

        public List<Object3D> Meshes = new List<Object3D>();

        protected override void LoadComplete() => UpdateProjectionMatrix();

        private void UpdateProjectionMatrix()
        {
            projectionMatrix.M00 = AspectRatio * FovRad;
            projectionMatrix.M11 = FovRad;
            projectionMatrix.M22 = FFar / (FFar - FNear);
            projectionMatrix.M32 = -FFar * FNear / (FFar - FNear);
            projectionMatrix.M23 = 1;
            projectionMatrix.M33 = 0;
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
                    foreach (var tri in mesh.Triangles)
                    {
                        var triangle = new Triangle3D(tri.P1 + mesh.Translation, tri.P2 + mesh.Translation, tri.P3 + mesh.Translation);

                        var line1 = triangle.P2 - triangle.P1;

                        var line2 = triangle.P3 - triangle.P1;

                        var absNormal = Vector3.Cross(line1, line2);

                        var triTranslated = new Triangle3D(triangle.P1 - source.CameraPosition, triangle.P2 - source.CameraPosition, triangle.P3 - source.CameraPosition);

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

                        var triProjected = new Triangle3D();

                        Mat4x4.MultiplyMatrixVector(triTranslated.P1, out triProjected.P1, source.projectionMatrix);
                        Mat4x4.MultiplyMatrixVector(triTranslated.P2, out triProjected.P2, source.projectionMatrix);
                        Mat4x4.MultiplyMatrixVector(triTranslated.P3, out triProjected.P3, source.projectionMatrix);

                        var priTri = new Graphics.Primitives.Triangle(triProjected.P1.Xy + Vector2.One, triProjected.P2.Xy + Vector2.One, triProjected.P3.Xy + Vector2.One);

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
