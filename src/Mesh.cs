using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KzkmEngine
{
    public class Mesh
    {
        public class Vertex
        {
            public float x {get; private set;}
            public float y {get; private set;}
            public float z {get; private set;}
            public Vertex(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public class Face
        {
            public int verticesNum {get; private set;}
            public List<int> vertexIndices {get; private set;} = new List<int>();
            public List<Tuple<float, float>> uvCoords {get; private set;} = new List<Tuple<float, float>>();
            //2頂点
            public Face(int verticesNum, int vertexIndex1, int vertexIndex2, float uvCoord1x, float uvCoord1y, float uvCoord2x, float uvCoord2y)
            {
                this.verticesNum = verticesNum;
                vertexIndices.Add(vertexIndex1);
                vertexIndices.Add(vertexIndex2);
                uvCoords.Add(new Tuple<float, float>(uvCoord1x, uvCoord1y));
                uvCoords.Add(new Tuple<float, float>(uvCoord2x, uvCoord2y));
            }
            //3頂点
            public Face(int verticesNum, int vertexIndex1, int vertexIndex2, int vertexIndex3, float uvCoord1x, float uvCoord1y, float uvCoord2x, float uvCoord2y, float uvCoord3x, float uvCoord3y)
            {
                this.verticesNum = verticesNum;
                vertexIndices.Add(vertexIndex1);
                vertexIndices.Add(vertexIndex2);
                vertexIndices.Add(vertexIndex3);
                uvCoords.Add(new Tuple<float, float>(uvCoord1x, uvCoord1y));
                uvCoords.Add(new Tuple<float, float>(uvCoord2x, uvCoord2y));
                uvCoords.Add(new Tuple<float, float>(uvCoord3x, uvCoord3y));
            }
            //4頂点
            public Face(int verticesNum, int vertexIndex1, int vertexIndex2, int vertexIndex3, int vertexIndex4, float uvCoord1x, float uvCoord1y, float uvCoord2x, float uvCoord2y, float uvCoord3x, float uvCoord3y, float uvCoord4x, float uvCoord4y)
            {
                this.verticesNum = verticesNum;
                vertexIndices.Add(vertexIndex1);
                vertexIndices.Add(vertexIndex2);
                vertexIndices.Add(vertexIndex3);
                vertexIndices.Add(vertexIndex4);
                uvCoords.Add(new Tuple<float, float>(uvCoord1x, uvCoord1y));
                uvCoords.Add(new Tuple<float, float>(uvCoord2x, uvCoord2y));
                uvCoords.Add(new Tuple<float, float>(uvCoord3x, uvCoord3y));
                uvCoords.Add(new Tuple<float, float>(uvCoord4x, uvCoord4y));
            }

        }

        public class Material
        {
            public string name {get; private set;}
            public int shader {get; private set;}
            public float colR {get; private set;}
            public float colG {get; private set;}
            public float colB {get; private set;}
            public float colA {get; private set;}
            public float dif {get; private set;}
            public float amb {get; private set;}
            public float emi {get; private set;}
            public float spc {get; private set;}
            public float power {get; private set;}
            public string tex {get; private set;}

            public Material(string name, int shader, float colR, float colG, float colB, float colA, float dif, float amb, float emi, float spc, float power, string tex)
            {
                this.name = name;
                this.shader = shader;
                this.colR = colR;
                this.colG = colG;
                this.colB = colB;
                this.colA = colA;
                this.dif = dif;
                this.amb = amb;
                this.emi = emi;
                this.spc = spc;
                this.power = power;
                this.tex = tex;
            }
        }
        public class Obj
        {
            public List<Vertex> vertices {get; private set;}
            public List<Face> faces {get; private set;}
            public Obj(List<Vertex> vertices, List<Face> faces)
            {
                this.vertices = vertices;
                this.faces = faces;
            }

        }

        public List<Material> materials {get; private set;} = new List<Material>();
        public List<Obj> objs {get; private set;} = new List<Obj>();

        public void LoadFromMQO(string filename)
        {
            var source = new List<string>();
            
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var sr = new StreamReader(new FileStream(filename, FileMode.Open), Encoding.GetEncoding("Shift-JIS")))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        source.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            int sourceLinesNum = source.Count;
            int i = 0;
            while (i < sourceLinesNum - 1)
            {
                //マテリアルの読み込み開始判定
                if (Regex.IsMatch(source[i], @"\s*Material\s+(\d+)\s*{"))
                {
                    //マテリアル読み込みループ
                    while (i < sourceLinesNum - 1)
                    {
                        i++;
                        //マテリアルの読み込み終了判定
                        if (Regex.IsMatch(source[i], @"\s*}\s*"))
                        {
                            i++;
                            break;
                        }
                        var m = Regex.Match(source[i], @"""(.+)""\s*shader\(\s*(\d+)\s*\)\s*col\(\s*(\d+\.\d+)\s*(\d+\.\d+)\s*(\d+\.\d+)\s*(\d+\.\d+)\s*\)\s*dif\(\s*(\d+\.\d+)\s*\)\s*amb\(\s*(\d+\.\d+)\s*\)\s*emi\(\s*(\d+\.\d+)\s*\)\s*spc\(\s*(\d+\.\d+)\s*\)\s*power\(\s*(\d+\.\d+)\s*\)\s*(?:tex\(""([^""]*)""\)\s*)?");
                        while (m.Success)
                        {
                            string matName = m.Groups[1].ToString();
                            int shader = Convert.ToInt32(m.Groups[2].ToString());
                            float colR = Convert.ToSingle(m.Groups[3].ToString());
                            float colG = Convert.ToSingle(m.Groups[4].ToString());
                            float colB = Convert.ToSingle(m.Groups[5].ToString());
                            float colA = Convert.ToSingle(m.Groups[6].ToString());
                            float dif = Convert.ToSingle(m.Groups[7].ToString());
                            float amb = Convert.ToSingle(m.Groups[8].ToString());
                            float emi = Convert.ToSingle(m.Groups[9].ToString());
                            float spc = Convert.ToSingle(m.Groups[10].ToString());
                            float power = Convert.ToSingle(m.Groups[11].ToString());
                            string tex = m.Groups[12].ToString();
                            System.Console.WriteLine("mat: {0} added", matName);
                            materials.Add(new Material(matName, shader, colR, colG, colB, colA, dif, amb, emi, spc, power, tex));
                            m = m.NextMatch();
                        }
                    }
                }

                //オブジェクト読み込み開始判定
                if (Regex.IsMatch(source[i], @"\s*Object\s+""(.+)""\s*{"))
                {
                    //オブジェクト名の取得
                    var objNameMatch = Regex.Match(source[i], @"\s*Object\s+""(.+)""\s*{");
                    var vertices = new List<Vertex>();
                    var faces = new List<Face>();
                    string objName = objNameMatch.Groups[1].ToString();
                    // System.Console.WriteLine(objName);
                    //オブジェクトの読み込みループ
                    while (i < sourceLinesNum - 1)
                    {
                        // System.Console.WriteLine(source[i]);
                        //オブジェクトの読み込み終了判定
                        if (Regex.IsMatch(source[i], @"\s*}\s*"))
                        {
                            // System.Console.WriteLine("objend");
                            break;
                        }
                        i++;
                        //頂点の読み込み開始判定
                        if (Regex.IsMatch(source[i], @"\s*vertex\s+(\d+)\s*{"))
                        {
                            //頂点数の取得
                            var verticesNumMatch = Regex.Match(source[i], @"\s*vertex\s+(\d+)\s*{");
                            int verticesNum = Convert.ToInt32(verticesNumMatch.Groups[1].ToString());
                            while (i < sourceLinesNum - 1)
                            {
                                i++;
                                //頂点読み込み終了判定
                                if (Regex.IsMatch(source[i], @"\s*}\s*"))
                                {
                                    // System.Console.WriteLine("vert end");
                                    i++;
                                    break;
                                }
                                var vertexCoordMatch = Regex.Match(source[i], @"\s*(-?\d+(?:\.\d*)?)\s+(-?\d+(?:\.\d*)?)\s+(-?\d+(?:\.\d*)?)\s*");
                                float x = Convert.ToSingle(vertexCoordMatch.Groups[1].ToString());
                                float y = Convert.ToSingle(vertexCoordMatch.Groups[2].ToString());
                                float z = Convert.ToSingle(vertexCoordMatch.Groups[3].ToString());
                                vertices.Add(new Vertex(x, y, z));
                            }
                        }
                        //面の読み込み開始判定
                        if (Regex.IsMatch(source[i], @"\s*face\s+(\d+)\s*{"))
                        {
                            //面の数の取得
                            var facesNumMatch = Regex.Match(source[i], @"\s*face\s+(\d+)\s*{");
                            int faceNum = Convert.ToInt32(facesNumMatch.Groups[1].ToString());
                            while ( i < sourceLinesNum - 1)
                            {
                                i++;
                                //面の読み込み終了判定
                                if (Regex.IsMatch(source[i], @"\s*}\s*"))
                                {
                                    // System.Console.WriteLine("face end");
                                    i++;
                                    break;
                                }
                                //面の頂点数による場合分け
                                if (Regex.IsMatch(source[i], @"^\s*2.*"))
                                {
                                    var faceMatch = Regex.Match(source[i], @"\s*2\s*V\s*\(\s*(\d+)\s+(\d+)\s*\)\s*M\s*\(\s*(\d+)\s*\)\s*(?:UV\s*\(\s*(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s*\))?");
                                    int vertexIndex1 = Convert.ToInt32(faceMatch.Groups[1].ToString());
                                    int vertexIndex2 = Convert.ToInt32(faceMatch.Groups[2].ToString());
                                    int materialIndex = Convert.ToInt32(faceMatch.Groups[3].ToString());
                                    float uvCoord1x = (faceMatch.Groups[4].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[4].ToString())) : 0.0f;
                                    float uvCoord1y = (faceMatch.Groups[5].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[5].ToString())) : 0.0f;
                                    float uvCoord2x = (faceMatch.Groups[6].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[6].ToString())) : 0.0f;
                                    float uvCoord2y = (faceMatch.Groups[7].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[7].ToString())) : 0.0f;
                                    faces.Add(new Face(2, vertexIndex1, vertexIndex2, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y));
                                    // System.Console.WriteLine(source[i]);
                                    // System.Console.WriteLine("({0}, {1}) M({2}), UV({3}, {4}, {5}, {6})", vertexIndex1, vertexIndex2, materialIndex, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y);
                                }
                                else if (Regex.IsMatch(source[i], @"^\s*3.*"))
                                {
                                    var faceMatch = Regex.Match(source[i], @"\s*3\s*V\s*\(\s*(\d+)\s+(\d+)\s+(\d+)\s*\)\s*M\s*\(\s*(\d+)\s*\)\s*(?:UV\s*\(\s*(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s*\))?");
                                    int vertexIndex1 = Convert.ToInt32(faceMatch.Groups[1].ToString());
                                    int vertexIndex2 = Convert.ToInt32(faceMatch.Groups[2].ToString());
                                    int vertexIndex3 = Convert.ToInt32(faceMatch.Groups[3].ToString());
                                    int materialIndex = Convert.ToInt32(faceMatch.Groups[4].ToString());
                                    float uvCoord1x = (faceMatch.Groups[5].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[5].ToString())) : 0.0f;
                                    float uvCoord1y = (faceMatch.Groups[6].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[6].ToString())) : 0.0f;
                                    float uvCoord2x = (faceMatch.Groups[7].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[7].ToString())) : 0.0f;
                                    float uvCoord2y = (faceMatch.Groups[8].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[8].ToString())) : 0.0f;
                                    float uvCoord3x = (faceMatch.Groups[9].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[9].ToString())) : 0.0f;
                                    float uvCoord3y = (faceMatch.Groups[10].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[10].ToString())) : 0.0f;
                                    faces.Add(new Face(3, vertexIndex1, vertexIndex2, vertexIndex3, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y, uvCoord3x, uvCoord3y));
                                    // System.Console.WriteLine(source[i]);
                                    // System.Console.WriteLine("({0}, {1}, {2}) M({3}), UV({4}, {5}, {6}, {7}, {8}, {9})", vertexIndex1, vertexIndex2, vertexIndex3, materialIndex, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y, uvCoord3x, uvCoord3y);
                                }
                                else if (Regex.IsMatch(source[i], @"^\s*4.*"))
                                {
                                    var faceMatch = Regex.Match(source[i], @"\s*4\s*V\s*\(\s*(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s*\)\s*M\s*\(\s*(\d+)\s*\)\s*(?:UV\s*\(\s*(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s+(-?\d+(?:\.\d*))\s*\))?");
                                    int vertexIndex1 = Convert.ToInt32(faceMatch.Groups[1].ToString());
                                    int vertexIndex2 = Convert.ToInt32(faceMatch.Groups[2].ToString());
                                    int vertexIndex3 = Convert.ToInt32(faceMatch.Groups[3].ToString());
                                    int vertexIndex4 = Convert.ToInt32(faceMatch.Groups[4].ToString());
                                    int materialIndex = Convert.ToInt32(faceMatch.Groups[5].ToString());
                                    float uvCoord1x = (faceMatch.Groups[6].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[6].ToString())) : 0.0f;
                                    float uvCoord1y = (faceMatch.Groups[7].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[7].ToString())) : 0.0f;
                                    float uvCoord2x = (faceMatch.Groups[8].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[8].ToString())) : 0.0f;
                                    float uvCoord2y = (faceMatch.Groups[9].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[9].ToString())) : 0.0f;
                                    float uvCoord3x = (faceMatch.Groups[10].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[10].ToString())) : 0.0f;
                                    float uvCoord3y = (faceMatch.Groups[11].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[11].ToString())) : 0.0f;
                                    float uvCoord4x = (faceMatch.Groups[12].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[12].ToString())) : 0.0f;
                                    float uvCoord4y = (faceMatch.Groups[13].ToString() != "") ? (Convert.ToSingle(faceMatch.Groups[13].ToString())) : 0.0f;
                                    faces.Add(new Face(4, vertexIndex1, vertexIndex2, vertexIndex3, vertexIndex4, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y, uvCoord3x, uvCoord3y, uvCoord4x, uvCoord4y));
                                    // System.Console.WriteLine(source[i]);
                                    // System.Console.WriteLine("({0}, {1}, {2}, {3}) M({4}), UV({5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})", vertexIndex1, vertexIndex2, vertexIndex3, vertexIndex4, materialIndex, uvCoord1x, uvCoord1y, uvCoord2x, uvCoord2y, uvCoord3x, uvCoord3y, uvCoord4x, uvCoord4y);
                                }
                                else
                                {
                                    System.Console.WriteLine(source[i]);
                                    System.Console.WriteLine("format err");
                                    throw new Exception("Unexpected Face Format");
                                }
                            }
                        }
                    }
                    var obj = new Obj(vertices, faces);
                    objs.Add(obj);
                    System.Console.WriteLine("obj {0} Added, n verts={1}, n faces={2}", objName, vertices.Count, faces.Count);
                }
                i++;
            }
        }
    }
}