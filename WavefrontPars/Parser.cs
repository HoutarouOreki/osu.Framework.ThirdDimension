using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WavefrontPars
{
    public static class Parser
    {
        public static List<Object3D> ParseText(string s)
        {
            var currObjName = (string)null;
            var currObjVectors = new List<Vector3>();
            var currObjFaces = new List<Triangle3D>();
            var objects = new List<Object3D>();
            foreach (var line in s.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var lineSplit = line.Split(' ');
                switch (lineSplit.ElementAtOrDefault(0))
                {
                    case "o": // new object
                        if (!string.IsNullOrEmpty(currObjName))
                        {
                            var obj = new Object3D { Name = currObjName };
                            obj.Triangles.AddRange(currObjFaces);
                            objects.Add(obj);
                            currObjVectors.Clear();
                            currObjFaces.Clear();
                        } // added the last object, starting new
                        currObjName = line.Substring(lineSplit.ElementAt(0).Length);
                        break;
                    case "v": // new point
                        if (lineSplit.Length != 4)
                            throw new ArgumentOutOfRangeException("Not handling non-triangulated meshes currently.");
                        currObjVectors.Add(new Vector3(float.Parse(lineSplit[1]), float.Parse(lineSplit[2]), float.Parse(lineSplit[3])));
                        break;
                    case "f": // new face from points
                        currObjFaces.Add(new Triangle3D
                        {
                            P1 = currObjVectors[int.Parse(lineSplit[1]) - 1],
                            P2 = currObjVectors[int.Parse(lineSplit[2]) - 1],
                            P3 = currObjVectors[int.Parse(lineSplit[3]) - 1],
                        });
                        break;
                }
            }
            var lastObj = new Object3D { Name = currObjName };
            lastObj.Triangles.AddRange(currObjFaces);
            objects.Add(lastObj);
            currObjVectors.Clear();
            currObjFaces.Clear();
            return objects;
        }
    }
}
