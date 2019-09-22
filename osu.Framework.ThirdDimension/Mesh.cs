using osuTK;

namespace osu.Framework.ThirdDimension
{
    public class Mesh
    {
        public Vector3 Translation;

        public readonly Triangle3D[] Tris;

        public Mesh(params Triangle3D[] triangles)
        {
            Tris = triangles;
            Translation = Vector3.Zero;
        }
    }
}
