using osuTK;
using System.Collections.Generic;

namespace WavefrontPars
{
    public class Object3D
    {
        public string Name { get; set; }
        public List<Triangle3D> Triangles { get; } = new List<Triangle3D>();
        public Vector3 Translation;

        public Object3D(string name, IEnumerable<Triangle3D> triangles)
        {
            Name = name;
            Triangles.AddRange(triangles);
        }

        public Object3D() { }
    }
}
