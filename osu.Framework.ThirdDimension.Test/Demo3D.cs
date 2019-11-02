using osu.Framework.Input.Events;
using osuTK;

namespace osu.Framework.ThirdDimension.Test
{
    public class Demo3D : Game
    {
        private SpatialSpaceDisplay spatialDisplay;

        private const float camera_speed = 3;
        private Vector3 cameraVelocity;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(spatialDisplay = new SpatialSpaceDisplay { RelativeSizeAxes = Graphics.Axes.Both });
            //var cube = Object3DSamples.GetNewCube();
            //spatialDisplay.Objects.Add(cube);
            //cube.Translation = new Vector3(0, 0, 0);
            spatialDisplay.GlobalLightDirection = new Vector3(0, -1, 0);
            var cone = Object3DSamples.GetNewCone();
            spatialDisplay.Objects.Add(cone);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case osuTK.Input.Key.Up:
                    cameraVelocity.Z = camera_speed;
                    break;
                case osuTK.Input.Key.Down:
                    cameraVelocity.Z = -camera_speed;
                    break;
                case osuTK.Input.Key.Left:
                    cameraVelocity.X = -camera_speed;
                    break;
                case osuTK.Input.Key.Right:
                    cameraVelocity.X = camera_speed;
                    break;
                case osuTK.Input.Key.LShift:
                    cameraVelocity.Y = -camera_speed;
                    break;
                case osuTK.Input.Key.Space:
                    cameraVelocity.Y = camera_speed;
                    break;
            }
            return base.OnKeyDown(e);
        }

        protected override bool OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case osuTK.Input.Key.Left:
                case osuTK.Input.Key.Right:
                    cameraVelocity.X = 0;
                    break;
                case osuTK.Input.Key.LShift:
                case osuTK.Input.Key.Space:
                    cameraVelocity.Y = 0;
                    break;
                case osuTK.Input.Key.Up:
                case osuTK.Input.Key.Down:
                    cameraVelocity.Z = 0;
                    break;
            }
            return base.OnKeyUp(e);
        }

        protected override void Update()
        {
            spatialDisplay.CameraPosition += cameraVelocity * (float)Time.Elapsed / 1000;
            base.Update();
        }
    }
}
