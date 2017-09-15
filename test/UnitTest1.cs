using System;
using Xunit;
using KzkmEngine;

namespace test
{
    public class UnitTest1
    {
        [Fact(DisplayName = "mqoファイルの読み込みの関数に存在しないファイルを渡すと例外が投げられる")]
        public void MeshTest1()
        {
            Assert.Throws<System.IO.FileNotFoundException>(() => 
            {
                var m = new Mesh();
                //実行時のカレントディレクトリは bin/Debug/netcoreapp1.1
                m.LoadFromMQO("../../../resources/noExistFile.mqo");
            });

        }
        [Fact(DisplayName = "mqoファイルの読み込みのテスト")]
        public void MeshTest2()
        {
            var m = new Mesh();
            //実行時のカレントディレクトリは bin/Debug/netcoreapp1.1
            m.LoadFromMQO("../../../resources/box.mqo");
            //box.mqoのオブジェクト数は1
            Assert.Equal(1, m.objs.Count);
            //box.mqoのマテリアル数は1
            Assert.Equal(1, m.materials.Count);
            //box.mqoの頂点数は8
            Assert.Equal(8, m.objs[0].vertices.Count);
            //box.mqoの面数は6
            Assert.Equal(6, m.objs[0].faces.Count);
        }
    }
}
