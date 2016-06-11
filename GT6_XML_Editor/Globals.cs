using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GT6_XML_Editor
{
    public enum packed
    {
        unpacked,
        packed
    }
    public enum dialog
    {
        rail_def_1,
        coursemaker_1,
        rail_def_2,
        coursemaker_2
    }

    public class Globals
    {
        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <param name="ex"></param>
        public static void printException(Exception ex)
        {
            StreamWriter log;
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            // Write to the file:
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("HelpLink = {0}", ex.HelpLink);
            log.WriteLine("Message = {0}", ex.Message);
            log.WriteLine("Source = {0}", ex.Source);
            log.WriteLine("StackTrace = {0}", ex.StackTrace);
            log.WriteLine("TargetSite = {0}", ex.TargetSite);
            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            log.WriteLine("TargetSite = {0}", frame.GetFileLineNumber());

            // Close the stream:
            log.Close();
        }
    }
}
