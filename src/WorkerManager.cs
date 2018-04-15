using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace KzkmEngine
{
    public class WorkerManager
    {
        //Workerを保持する
        private List<Worker> workers = new List<Worker>();

        private Camera camera = new Camera();

        //private Mesh mesh = new Mesh();

        public WorkerManager()
        {
            //mesh.LoadFromObj("resources/ente_progress_model/ente progress_export.obj");
        }

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
            //バッファ初期化
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         
            //視点の設定  
            Matrix4 modelView = Matrix4.LookAt(camera.eye, camera.target, camera.up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelView);
 
            //Utility.RenderSphere(10, 10);
            //mesh.Render();

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