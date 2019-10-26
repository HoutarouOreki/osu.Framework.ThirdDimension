namespace osu.Framework.ThirdDimension.Test
{
    public class Demo3D : Game
    {
        private SpatialSpaceDisplay spatialDisplay;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(spatialDisplay = new SpatialSpaceDisplay { RelativeSizeAxes = Graphics.Axes.Both });
            spatialDisplay.Meshes.Add(Object3DSamples.GetNewCube());
        }
    }
}
