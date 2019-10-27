namespace osu.Framework.ThirdDimension.Test
{
    public class Demo3D : Game
    {
        private SpatialSpaceDisplay spatialDisplay;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(spatialDisplay = new SpatialSpaceDisplay { RelativeSizeAxes = Graphics.Axes.Both });
            var cube = Object3DSamples.GetNewCube();
            spatialDisplay.Meshes.Add(cube);
            cube.Translation = new osuTK.Vector3(1);
        }
    }
}
