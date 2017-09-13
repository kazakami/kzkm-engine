using OpenTK;

namespace KzkmEngine
{
    public class Camera : Worker
    {
        public Vector3 eye { get; private set; } = new Vector3(10, 10, 10);
        public Vector3 target { get; private set; } = new Vector3(0, 0, 0);
        public Vector3 up {get; private set; } = new Vector3(0, 1, 0);
        public override void Update()
        {

        }

        public override void Draw()
        {

        }
    }
}