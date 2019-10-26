namespace osu.Framework.ThirdDimension
{
    public class Program
    {
        private static void Main()
        {
            using (var host = Host.GetSuitableHost("3D"))
            {
                host.Run(new Demo3D());
            }
        }
    }
}
