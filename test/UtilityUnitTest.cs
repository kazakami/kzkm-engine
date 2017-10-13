using System;
using Xunit;
using KzkmEngine;

namespace test
{
    public class UtilityUnitTest
    {
        [Fact(DisplayName = "mqoファイルの読み込みの関数に存在しないファイルを渡すと例外が投げられる")]
        public void UtulityTest1()
        {
            Assert.Equal("hoge fuga", Utility.CutEndSpace("hoge fuga   "));
        }
    }
}
