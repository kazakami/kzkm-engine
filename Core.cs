using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;


namespace KzkmEngine
{

    class Game:GameWindow
	{
		public WorkerManager workerManager;

		public Game(int width, int height, string name)
			:base(width, height, GraphicsMode.Default, name)
		{
			VSync = VSyncMode.On;
		}

		//ウィンドウ生成時に呼ばれる
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GL.ClearColor(Color4.Black);
			GL.Enable(EnableCap.DepthTest);
		}

		//ウィンドウサイズ変更時に呼ばれる
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)Width / (float)Height, 1.0f, 64.0f);
			GL.LoadMatrix(ref projection);
		}

		//画面更新時に呼ばれる
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			//Escape押してで終了
			if (Keyboard[Key.Escape])
			{
				this.Exit();
			}
		}

		//画面描画時に呼ばれる
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
			GL.LoadMatrix(ref modelview);
			
			// GL.Begin(PrimitiveType.Quads);

			// GL.Color4(Color4.White);							//色名で指定
			// GL.Vertex3(-1.0f, 1.0f, 4.0f);
			// GL.Color4(new float[] { 1.0f, 0.0f, 0.0f, 1.0f });	//配列で指定
			// GL.Vertex3(-1.0f, -1.0f, 4.0f);
			// GL.Color4(0.0f, 1.0f, 0.0f, 1.0f);					//4つの引数にfloat型で指定
			// GL.Vertex3(1.0f, -1.0f, 4.0f);
			// GL.Color4((byte)0, (byte)0, (byte)255, (byte)255);	//byte型で指定
			// GL.Vertex3(1.0f, 1.0f, 4.0f);

			// GL.End();

			SwapBuffers();
		}
    }
}
