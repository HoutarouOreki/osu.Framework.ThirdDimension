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

        public static Mesh Cube => new Mesh(new[]
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
        });
    }
}
