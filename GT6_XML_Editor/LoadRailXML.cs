using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using XmlRail;

namespace GT6_XML_Editor
{
    public class LoadRailXML
    {
        public Rail m_rail;
        public double defaultWidth;
        public bool loaded;
        public static double m_homeStraightLength;
        public static double m_oneWayStartLength;
        public static double m_oneWayGoalLength;
        public static double m_oneWayDummyLength;
        public static double m_oneWayGoalPos;
        public static double m_oneWayGoalOffset;
        public static double m_oneWayStartOffset;
        public static double m_loopStartOffset;
        public static double m_startPos;
        public bool debug = true;

        public LoadRailXML(string end)
        {
            try
            {
                TextReader stringReader = new StreamReader(end);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Rail));
                m_rail = (Rail)xmlSerializer.Deserialize(stringReader);
                defaultWidth = (double)m_rail.railDetail.defaultWidth;
                if ((int)m_rail.railGroups.Length > 0)
                {
                    this.loaded = true;
                    m_homeStraightLength = 0;
                    m_oneWayStartLength = 0;
                    m_oneWayGoalLength = 0;
                    m_oneWayGoalOffset = 0;
                    m_oneWayStartOffset = 0;
                    m_loopStartOffset = 0;
                    RailGroup[] mRail = m_rail.railGroups;
                    for (int i = 0; i < (int)mRail.Length; i++)
                    {
                        RailGroup railGroup = mRail[i];
                        if (this.debug)
                        {
                            System.Diagnostics.Debug.WriteLine(railGroup.name);
                        }
                        if ((int)railGroup.itemTransition.Length > 0 || (int)railGroup.itemLoop.Length > 0 || (int)railGroup.itemRandom.Length > 0)
                        {
                            if (railGroup.attr == 8)
                            {
                                for (int j = 0; j < (int)railGroup.itemTransition.Length; j++)
                                {
                                    RailGroupItem railGroupItem = railGroup.itemTransition[j];
                                    int num1 = Array.IndexOf<ulong>(m_rail.railDictionary.uuid, railGroupItem.uuid);
                                    if (num1 >= 0)
                                    {
                                        int mRail1 = m_rail.railDictionary.slot[num1];
                                        RailUnit railUnit = m_rail.railUnits[mRail1];
                                        if (railUnit.checkpoint > 0f)
                                        {
                                            m_loopStartOffset = m_homeStraightLength + (double)railUnit.checkpoint;
                                        }
                                        m_homeStraightLength = m_homeStraightLength + (double)railUnit.unitLength;
                                    }
                                }
                                if (this.debug)
                                {
                                    System.Diagnostics.Debug.WriteLine(string.Concat("home straight:", m_homeStraightLength.ToString("F1"), "m"));
                                }
                            }
                            else if (railGroup.attr == 15)
                            {
                                for (int k = 0; k < (int)railGroup.itemTransition.Length; k++)
                                {
                                    RailGroupItem railGroupItem1 = railGroup.itemTransition[k];
                                    int num2 = Array.IndexOf<ulong>(m_rail.railDictionary.uuid, railGroupItem1.uuid);
                                    if (num2 >= 0)
                                    {
                                        int mRail2 = m_rail.railDictionary.slot[num2];
                                        RailUnit railUnit1 = m_rail.railUnits[mRail2];
                                        if (railUnit1.checkpoint > 0f)
                                        {
                                            m_oneWayStartOffset = m_oneWayStartLength + (double)railUnit1.checkpoint;
                                        }
                                        m_oneWayStartLength = m_oneWayStartLength + (double)railUnit1.unitLength;
                                    }
                                }
                                if (this.debug)
                                {
                                    System.Diagnostics.Debug.WriteLine(string.Concat("start:", m_oneWayStartLength.ToString("F1"), "m"));
                                }
                            }
                            else if (railGroup.attr == 16)
                            {
                                for (int l = 0; l < (int)railGroup.itemTransition.Length; l++)
                                {
                                    RailGroupItem railGroupItem2 = railGroup.itemTransition[l];
                                    int num3 = Array.IndexOf<ulong>(m_rail.railDictionary.uuid, railGroupItem2.uuid);
                                    if (num3 >= 0)
                                    {
                                        int mRail3 = m_rail.railDictionary.slot[num3];
                                        RailUnit railUnit2 = m_rail.railUnits[mRail3];
                                        m_oneWayGoalLength = m_oneWayGoalLength + (double)railUnit2.unitLength;
                                        if (railUnit2.checkpoint > 0f)
                                        {
                                            m_oneWayGoalOffset = (double)railUnit2.checkpoint;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.debug)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Concat("goal:", m_oneWayGoalLength.ToString("F1"), "m"));
                        System.Diagnostics.Debug.WriteLine(string.Concat("oneway goal offset:", m_oneWayGoalOffset.ToString("F1"), "m"));
                        System.Diagnostics.Debug.WriteLine(string.Concat("oneway start offset:", m_oneWayStartOffset.ToString("F1"), "m"));
                        System.Diagnostics.Debug.WriteLine(string.Concat("loop start offset:", m_loopStartOffset.ToString("F1"), "m"));
                        System.Diagnostics.Debug.WriteLine(string.Concat("コーナー閾値:", m_rail.railDetail.limitCurvature));
                        System.Diagnostics.Debug.WriteLine(string.Concat("連続コーナー閾値:", m_rail.railDetail.limitContinuousTurn));
                        System.Diagnostics.Debug.WriteLine(string.Concat("曲率限界:", m_rail.railDetail.maxCurvature));
                    }
                }
                else if (this.debug)
                {
                    System.Diagnostics.Debug.WriteLine("レールグループが定義されていない！");
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                System.Diagnostics.Debug.WriteLine(string.Concat("Exception caught. ", exception.Message));
                if (this.debug)
                {
                    System.Diagnostics.Debug.WriteLine(string.Concat("Exception caught. ", exception.Message));
                }
            }
        }

        public void SerializeRail(string filename)
        {
            XmlSerializer s = new XmlSerializer(typeof(Rail));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    s.Serialize(writer, m_rail);
                }
            }
        }
    }
}
