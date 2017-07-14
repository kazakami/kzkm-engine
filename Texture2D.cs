using System;
using FreeImageAPI;
using OpenTK.Graphics.OpenGL;

namespace KzkmEngine
{
    class Texture2D
    {
        float[,,] data = null;
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public int texture { get; private set; } = 0;

        public bool Load(string filename)
        {
            //ファイルからの画像データの読み込み
            try
            {
                using (FreeImageBitmap img = new FreeImageBitmap(filename))
                {
                    Width = img.Width;
                    Height = img.Height;
                    data = new float[Height, Width, 4];
                    for (int j = 0; j < Height; j++)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            var col = img.GetPixel(i, j);
                            data[j, i, 0] = (float)col.R / 256;
                            data[j, i, 1] = (float)col.G / 256;
                            data[j, i, 2] = (float)col.B / 256;
                            data[j, i, 3] = (float)col.A / 256;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on loading file : " + filename);
                Console.WriteLine(e.Data.ToString());
                return false;
            }

            //画像データをまだ読み込んでないなら失敗
            if (data == null)
                return false;
            //テクスチャをすでに確保しているなら解放
            if (texture != 0)
                GL.DeleteTexture(texture);
            //テクスチャの確保、束縛、設定
            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, data);

            return true;
        }

        public void Delete()
        {
            GL.DeleteTexture(texture);
            texture = 0;
        }
    }
}
