using System.Collections.Generic;

namespace KzkmEngine
{
    class WorkerManager
    {
        private List<Worker> workers = new List<Worker>();

        public void Update()
        {
            workers.Sort((a, b) => a.priority - b.priority);
            foreach (var worker in workers)
            {
                if (worker.isAlive)
                    worker.Update();
            }
        }
        public void Draw()
        {
            workers.Sort((a, b) => a.priority - b.priority);
            foreach (var worker in workers)
            {
                if (worker.isAlive)
                    worker.Draw();
            }
        }

        public void AddWorker(Worker worker)
        {
            workers.Add(worker);
        }

        ///isAliveがfalseになったworkerを取り除く
        public void DeleteWorker()
        {
            workers.RemoveAll(w => !w.isAlive);
        }
    }
}