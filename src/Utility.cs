using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace KzkmEngine
{
    public static class Utility
    {
        static System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        static long oldTime = 0;
        const int spansSize = 100;
        static List<long> spans = new List<long>();
        /// <summary>
        /// 毎フレームに一回呼び出すと、FPSを計算する
        /// </summary>
        /// <returns>過去一定フレームでの平均FPS</returns>
        public static double GetFPS()
        {
            var nowTime = sw.ElapsedMilliseconds;
            var span = nowTime - oldTime;
            oldTime = nowTime;
            spans.Add(span);
            if (spans.Count > spansSize)
                spans.RemoveAt(0);
            var spanSum = spans.Sum();
            return (double)1000 / spanSum * spansSize;
        }

        /// <summary>
        /// 原点を中心とした単位球を描画する
        /// </summary>
        /// <param name="slices"></param>
        /// <param name="stacks"></param>
        public static void RenderSphere(int slices, int stacks)
        {
            for (int i = 0; i < stacks; i++)
            {
                //輪切り上部
                double upper = Math.PI / stacks * i;
                double upperHeight = Math.Cos(upper);
                double upperWidth = Math.Sin(upper);
                //輪切り下部
                double lower = Math.PI / stacks * (i + 1);
                double lowerHeight = Math.Cos(lower);
                double lowerWidth = Math.Sin(lower);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    //輪切りの面を単位円としたときの座標
                    double rotor = 2 * Math.PI / slices * j;
                    double x = Math.Cos(rotor);
                    double y = Math.Sin(rotor);

                    GL.Normal3(x * lowerWidth, lowerHeight, y * lowerWidth);
                    GL.Vertex3(x * lowerWidth, lowerHeight, y * lowerWidth);
                    GL.Normal3(x * upperWidth, upperHeight, y * upperWidth);
                    GL.Vertex3(x * upperWidth, upperHeight, y * upperWidth);
                }
                GL.End();
            }
        }
    }
}
