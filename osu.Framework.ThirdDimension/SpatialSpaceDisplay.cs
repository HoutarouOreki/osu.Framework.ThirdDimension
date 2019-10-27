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
        public Quaternion CameraRotation = new Quaternion();

        public Vector3 GlobalLightDirection;

        /// <summary>
        /// A value of 1 causes faces facing directly into the light to be their exacct colour (adds up to <see cref="AmbientLightAmount"/>).
        /// </summary>
        public float GlobalLightAmount = 0.5f;

        /// <summary>
        /// Minimal amount of light on an object (adds up to <see cref="GlobalLightAmount"/>).
        /// </summary>
        public float AmbientLightAmount = 0;

        private float fNear = 0.1f;
        private float fFar = 500f;
        private float fov = 90;

        private Matrix4 projectionMatrix;

        public float FNear { get => fNear; set { fNear = value; UpdateProjectionMatrix(); } }
        public float FFar { get => fFar; set { fFar = value; UpdateProjectionMatrix(); } }
        public float Fov { get => fov; set { fov = value; UpdateProjectionMatrix(); } }
        public float AspectRatio => DrawWidth / DrawHeight;
        public float FovRad => 1f / (float)Math.Tan(Fov * 0.5f / 180 * Math.PI);

        public SpatialSpaceDisplay() => Texture = Texture.WhitePixel;

        public List<Object3D> Objects = new List<Object3D>();

        protected override void LoadComplete()
        {
            UpdateProjectionMatrix();
            base.LoadComplete();
        }

        private void UpdateProjectionMatrix()
        {
            projectionMatrix[0, 0] = AspectRatio * FovRad;
            projectionMatrix[1, 1] = -FovRad;
            projectionMatrix[2, 2] = FFar / (FFar - FNear);
            projectionMatrix[3, 2] = -FFar * FNear / (FFar - FNear);
            projectionMatrix[2, 3] = 1;
            projectionMatrix[3, 3] = 0;
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

                foreach (var objects in source.Objects)
                {
                    foreach (var tri in objects.Triangles)
                    {
                        // translate triangle to object's coordinates
                        var triangle = tri + objects.Translation;

                        // save triangle's (in absolute position) normal for later use
                        var triangleNormalInAbsoluteSpace = triangle.Normal;

                        var triangleTranslatedAroundCamera = new Triangle3D(triangle.P1 - source.CameraPosition, triangle.P2 - source.CameraPosition, triangle.P3 - source.CameraPosition);

                        var triangleNormalRelativeToCamera = triangleTranslatedAroundCamera.Normal;

                        if (Vector3.Dot(triangleNormalRelativeToCamera, triangleTranslatedAroundCamera.P1) > 0)
                            continue;

                        // light
                        var lightAmount = source.AmbientLightAmount;
                        if (source.GlobalLightDirection.Length != 0)
                            lightAmount += (-Vector3.Dot(triangleNormalInAbsoluteSpace, source.GlobalLightDirection) + 1) * source.GlobalLightAmount * 0.5f;

                        // project 3d-processed triangle into the camera
                        var triProjected = triangleTranslatedAroundCamera * source.projectionMatrix;

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
