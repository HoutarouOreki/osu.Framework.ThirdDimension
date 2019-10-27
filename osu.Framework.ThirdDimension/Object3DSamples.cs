﻿using osuTK;
using System.Linq;
using WavefrontPars;

namespace osu.Framework.ThirdDimension
{
    public class Object3DSamples
    {
        public static Object3D GetNewCube() => new Object3D("Cube", new[]
        {
            new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f)),
            new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f)),

            new Triangle3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f)),
            new Triangle3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f)),

            new Triangle3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)),
            new Triangle3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f)),

            new Triangle3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f)),
            new Triangle3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f)),

            new Triangle3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f)),
            new Triangle3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f)),

            new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f)),
            new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f)),
        });

        public static Object3D GetNewPlane() => new Object3D("Plane", new[]
        {
            new Triangle3D(new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 0, -0.5f), new Vector3(0.5f, 0, 0.5f)),
            new Triangle3D(new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 0, 0.5f), new Vector3(-0.5f, 0, 0.5f)),
        });

        public static Object3D GetNewCone() => Parser.ParseText(@"# Blender v2.80 (sub 75) OBJ File: ''
# www.blender.org
o Cone
v 0.000000 -1.000000 -1.000000
v 0.195090 -1.000000 -0.980785
v 0.382683 -1.000000 -0.923880
v 0.555570 -1.000000 -0.831470
v 0.707107 -1.000000 -0.707107
v 0.831470 -1.000000 -0.555570
v 0.923880 -1.000000 -0.382683
v 0.980785 -1.000000 -0.195090
v 1.000000 -1.000000 -0.000000
v 0.980785 -1.000000 0.195090
v 0.923880 -1.000000 0.382683
v 0.831470 -1.000000 0.555570
v 0.707107 -1.000000 0.707107
v 0.555570 -1.000000 0.831470
v 0.382683 -1.000000 0.923880
v 0.195090 -1.000000 0.980785
v -0.000000 -1.000000 1.000000
v -0.195091 -1.000000 0.980785
v -0.382684 -1.000000 0.923879
v -0.555571 -1.000000 0.831469
v -0.707107 -1.000000 0.707106
v -0.831470 -1.000000 0.555570
v -0.923880 -1.000000 0.382683
v 0.000000 1.000000 0.000000
v -0.980785 -1.000000 0.195089
v -1.000000 -1.000000 -0.000001
v -0.980785 -1.000000 -0.195091
v -0.923879 -1.000000 -0.382684
v -0.831469 -1.000000 -0.555571
v -0.707106 -1.000000 -0.707108
v -0.555569 -1.000000 -0.831470
v -0.382682 -1.000000 -0.923880
v -0.195089 -1.000000 -0.980786
s off
f 1 24 2
f 2 24 3
f 3 24 4
f 4 24 5
f 5 24 6
f 6 24 7
f 7 24 8
f 8 24 9
f 9 24 10
f 10 24 11
f 11 24 12
f 12 24 13
f 13 24 14
f 14 24 15
f 15 24 16
f 16 24 17
f 17 24 18
f 18 24 19
f 19 24 20
f 20 24 21
f 21 24 22
f 22 24 23
f 23 24 25
f 25 24 26
f 26 24 27
f 27 24 28
f 28 24 29
f 29 24 30
f 30 24 31
f 31 24 32
f 32 24 33
f 33 24 1
f 8 16 25
f 33 1 2
f 2 3 4
f 4 5 6
f 6 7 4
f 7 8 4
f 8 9 10
f 10 11 8
f 11 12 8
f 12 13 16
f 13 14 16
f 14 15 16
f 16 17 18
f 18 19 20
f 20 21 22
f 22 23 25
f 25 26 27
f 27 28 29
f 29 30 33
f 30 31 33
f 31 32 33
f 33 2 4
f 16 18 25
f 18 20 25
f 20 22 25
f 25 27 33
f 27 29 33
f 33 4 8
f 8 12 16
f 33 8 25
")[0];
    }
}
