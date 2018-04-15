using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;


namespace KzkmEngine
{

    public class Game:GameWindow
	{

		//名前とシーンを紐付けて持つ
		private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
		//現在有効なシーンの名前
		private Scene activeScene = null;

		/// <summary>
		/// コンストラクタ
		/// <param name="width">ウィンドウの幅</param>
		/// <param name="height">ウィンドウの高さ</param>
		/// <param name="name">ウィンドウタイトル</param>
		/// <param name="initSceneName">初期シーンに割り当てるキー</param>
		/// <param name="initScene">初期シーン</param>
		/// </summary>
		public Game(int width, int height, string name, string initSceneName, Scene initScene)
			:base(width, height, GraphicsMode.Default, name)
		{
			AddScene(initSceneName, initScene);
			ChangeActiveScene(initSceneName);
			VSync = VSyncMode.On;
		}

		//アクティブなシーンを切り替える
		public void ChangeActiveScene(string sceneName)
		{
			activeScene = scenes[sceneName];
		}

		//シーンを追加する
		public void AddScene(string sceneName, Scene scene)
		{
			scenes[sceneName] = scene;
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
			activeScene.Update();
		}

		//画面描画時に呼ばれる
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
			GL.LoadMatrix(ref modelview);
			
			activeScene.Draw();

			SwapBuffers();
		}
    }
}
