using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace KzkmEngine
{
    public class Shader
    {
        int vertexShader = 0;
        int fragmentShader = 0;
        public int program { get; private set; } = 0;



        public bool BuildShaderProgram(string vertexFile, string fragmentFile)
        {
            Delete();
            ReadVertexFromFile(vertexFile);
            ReadFragmentFromFile(fragmentFile);
            if (!Compile())
            {
                Console.WriteLine("failed to build shader program");
                return false;
            }
            return true;
        }

        public void Delete()
        {
            if (vertexShader != 0)
                GL.DeleteShader(vertexShader);
            if (fragmentShader != 0)
                GL.DeleteShader(fragmentShader);
            if (program != 0)
                GL.DeleteProgram(program);
            vertexShader = 0;
            fragmentShader = 0;
            program = 0;
        }

        /// <summary>
        /// シェーダのコードを渡せばプログラムオブジェクトを生成する
        /// </summary>
        /// <param name="vertexCode">頂点シェーダのコード</param>
        /// <param name="fragmentCode">フラグメントシェーダのコード</param>
        /// <returns></returns>
        public bool Compile(string vertexCode, string fragmentCode)
        {
            ReadVertex(vertexCode);
            ReadFragment(fragmentCode);
            return Compile();
        }

        /// <summary>
        /// シェーダプログラムを生成する
        /// 先にシェーダを読み込んでおく必要がある
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            if (vertexShader == 0 || fragmentShader == 0)
            {
                Console.WriteLine("vertex or fragment shader is not set.");
                return false;
            }
            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            int status;
            //シェーダプログラムのリンク
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            //シェーダプログラムのリンクのチェック
            if (status == 0)
            {
                Console.WriteLine(GL.GetProgramInfoLog(program));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 頂点シェーダをファイルから読み込む
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ReadVertexFromFile(string filename)
        {
            using (var data = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                ReadVertex(data.ReadToEnd());
            }
            return true;
        }

        /// <summary>
        /// 頂点シェーダを直接文字列として読み込む
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public void ReadVertex(string data)
        {
            if (vertexShader != 0)
                GL.DeleteShader(vertexShader);
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, data);
            GL.CompileShader(vertexShader);
            int status;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status);
            //コンパイル結果をチェック
            if (status == 0)
            {
                throw new ApplicationException(GL.GetShaderInfoLog(vertexShader));
            }
        }

        /// <summary>
        /// フラグメントシェーダをファイルから読み込む
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ReadFragmentFromFile(string filename)
        {
            using (var data = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                ReadFragment(data.ReadToEnd());
            }
            return true;
        }

        /// <summary>
        /// フラグメントシェーダを直接文字列として読み込む
        /// </summary>
        /// <param name="data"></param>
        public void ReadFragment(string data)
        {
            if (fragmentShader != 0)
                GL.DeleteShader(fragmentShader);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, data);
            GL.CompileShader(fragmentShader);
            int status;
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status);
            //コンパイル結果をチェック
            if (status == 0)
            {
                throw new ApplicationException(GL.GetShaderInfoLog(fragmentShader));
            }
        }

    }
}
