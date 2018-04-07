using System;
using Xunit;
using KzkmEngine;

namespace test
{
    public class MeshUnitTest
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
        [Fact(DisplayName = "objが複数あるmqoファイルの読み込みのテスト")]
        public void MeshTest3()
        {
            var m = new Mesh();
            //実行時のカレントディレクトリは bin/Debug/netcoreapp1.1
            m.LoadFromMQO("../../../resources/boxs.mqo");
            //boxs.mqoのオブジェクト数は2
            Assert.Equal(2, m.objs.Count);
            //boxs.mqoのマテリアル数は2
            Assert.Equal(2, m.materials.Count);
            //boxs.mqoの頂点数は8
            Assert.Equal(8, m.objs[1].vertices.Count);
            //boxs.mqoの面数は6
            Assert.Equal(6, m.objs[1].faces.Count);
        }
        [Fact(DisplayName = "objファイルの読み込みテスト")]
        public void MeshTest4()
        {
            var m = new Mesh();
            //実行時のカレントディレクトリは bin/Debug/netcoreapp1.1
            m.LoadFromObj("../../../resouces/box.obj");
            //box.objのオブジェクト数は1
            Assert.Equal(1, m.objs.Count);
            //box.objのマテリアル数は0
            Assert.Equal(0, m.materials.Count);
            //box.objの頂点数は8
            Assert.Equal(8, m.objs[0].vertices.Count);
            //boxs.mqoの面数は6
            Assert.Equal(6, m.objs[0].faces.Count);
        }
    }
}
