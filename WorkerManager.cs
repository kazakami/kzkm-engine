using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace KzkmEngine
{
    class WorkerManager
    {
        //Workerを保持する
        private List<Worker> workers = new List<Worker>();

        private Camera camera = new Camera();

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
            //視点の初期化
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            Matrix4 modelView = Matrix4.LookAt(camera.eye, camera.target, camera.up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelView);
 
            // Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)800.0 / (float)600.0, 1.0f, 64.0f);
            // GL.MatrixMode(MatrixMode.Projection);
            // GL.LoadMatrix(ref projection);
            Utility.RenderSphere(10, 10);

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