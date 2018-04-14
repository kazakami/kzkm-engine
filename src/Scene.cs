namespace KzkmEngine
{
    public class Scene
    {
		public WorkerManager workerManager = new WorkerManager();
        public Scene()
        {

        }

        public void Draw()
        {
			workerManager.Draw();
        }

        public void Update()
        {
			workerManager.Update();
        }

    }
}