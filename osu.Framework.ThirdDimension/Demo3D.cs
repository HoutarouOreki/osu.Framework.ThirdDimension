namespace osu.Framework.ThirdDimension
{
    public class Demo3D : Game
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new SpatialSpaceDisplay { RelativeSizeAxes = Graphics.Axes.Both });
        }
    }
}
