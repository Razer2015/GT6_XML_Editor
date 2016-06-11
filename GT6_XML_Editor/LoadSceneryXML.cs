using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using XmlScenery;

namespace GT6_XML_Editor
{
    public class LoadSceneryXML
    {
        public static Scenery m_scenery = (Scenery)null;
        public static double m_mapHeight = 4000.0;
        public static double m_mapWidth = 4000.0;
        public static Dictionary<int, int[]> m_hitEdge = (Dictionary<int, int[]>)null;
        public static List<int> m_noEntryIndex = (List<int>)null;
        public static bool deserialized = false;
        public static bool debug = true;

        public LoadSceneryXML(string end)
        {
            try
            {
                TextReader stringReader = new StringReader(end);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Scenery));
                m_scenery = (Scenery)xmlSerializer.Deserialize(stringReader);
                deserialized = true;
                if (debug)
                {
                    Console.WriteLine(new object[] { string.Concat(new string[] { "Scenery X:", m_scenery.Terrain.aabb_min[0].ToString("F1"), " ~ ", m_scenery.Terrain.aabb_max[0].ToString("F1"), "  Z:", m_scenery.Terrain.aabb_min[1].ToString("F1"), " ~ ", m_scenery.Terrain.aabb_max[1].ToString("F1") }) });
                }
                m_mapHeight = (double)(m_scenery.Terrain.aabb_max[1] - m_scenery.Terrain.aabb_min[1]);
                m_mapWidth = (double)(m_scenery.Terrain.aabb_max[0] - m_scenery.Terrain.aabb_min[0]);
                int count = m_scenery.TerrainAttr.SurfaceAttrTriangle.attributes.Count;
                m_hitEdge = new Dictionary<int, int[]>();
                m_noEntryIndex = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    if (m_scenery.TerrainAttr.SurfaceAttrTriangle.attributes[i] == 32768)
                    {
                        m_noEntryIndex.Add(i);
                        int item = m_scenery.TerrainAttr.SurfaceAttrTriangle.vertexIndices[i][0];
                        int item1 = m_scenery.TerrainAttr.SurfaceAttrTriangle.vertexIndices[i][1];
                        int num1 = m_scenery.TerrainAttr.SurfaceAttrTriangle.vertexIndices[i][2];
                        Console.WriteLine("InsertEdge: {0} - {1}", item, item1);
                        Console.WriteLine("InsertEdge: {0} - {1}", item1, num1);
                        Console.WriteLine("InsertEdge: {0} - {1}", num1, item);

                        //this.InsertEdge(item, item1);
                        //this.InsertEdge(item1, num1);
                        //this.InsertEdge(num1, item);
                    }
                }
                Console.WriteLine(string.Concat(new object[] { "#DegugLog StaticObj.m_scenery.TerrainAttr: \nKDTree.nodes.axis.Count:", m_scenery.TerrainAttr.KDTree.nodes.axis.Count, "\nSurfaceAttrTriangle.attributes.Count:", m_scenery.TerrainAttr.SurfaceAttrTriangle.attributes.Count, "\nSurfaceAttrTriangle.vertexIndices.Count:", m_scenery.TerrainAttr.SurfaceAttrTriangle.vertexIndices.Count, "\nVertex.data.Count:", m_scenery.TerrainAttr.Vertex.data.Count, "\n" }));
            }
            catch (Exception exception1)
            {
                Globals.printException(exception1);

                Exception exception = exception1;
                Console.WriteLine(string.Concat("Exception caught. ", exception.Message));
                if (debug)
                {
                    Console.WriteLine(new object[] { string.Concat("Exception caught. ", exception.Message) });
                }
            }
        }
    }
}
