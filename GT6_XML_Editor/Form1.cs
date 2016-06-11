using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GT6_XML_Editor
{
    public partial class Form1 : Form
    {
        public static UInt32 magic_unity = 78; // Actually it's the meta size but since there is no magic, I'll use this as a magic
        public static string ENCODE_TYPE = "utf-8";
        public static string ID_rail = "<Rail>";
        public static string ID_scenery = "<Scenery>";

        private Control[] Editors;

        public string save_path;

        public dialog active_dialog;
        public packed pack_status;

        public Form1()
        {
            InitializeComponent();
        }

        #region OpenFileDialogs
        private void openraildefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                save_path = null;
                active_dialog = dialog.rail_def_1;
                LoadRailXML rail = new LoadRailXML(openFileDialog1.FileName);
            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }

        private void opencoursemakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                save_path = null;
                active_dialog = dialog.coursemaker_1;
                LoadSceneryXML scenery = new LoadSceneryXML(openFileDialog2.FileName);
            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }

        private void openraildef2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog3.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                using (FileStream fs = new FileStream(openFileDialog3.FileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(magic_unity)];
                    fs.Seek(0L, SeekOrigin.Begin);
                    fs.Read(header, 0, 4);
                    if (BitConverter.ToUInt32(header.Reverse().ToArray(), 0) != magic_unity)
                    {
                        byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_rail);
                        byte[] array = new byte[bytes.Length];
                        fs.Seek(0L, SeekOrigin.Begin);
                        fs.Read(array, 0, bytes.Length);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != array[i])
                            {
                                throw new Exception("File ID is invalid. \"" + Encoding.UTF8.GetString(array, 0, array.Length) + "\"");
                            }
                        }
                        save_path = null;
                        pack_status = packed.unpacked;
                        active_dialog = dialog.rail_def_2;
                        populateTreeview(openFileDialog3.FileName);
                    }
                    else
                    {
                        save_path = null;
                        pack_status = packed.packed;
                        active_dialog = dialog.rail_def_2;
                        populateTreeview(Unity_Asset_Handler.Load_Asset(openFileDialog3.FileName));
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }

        private void opencoursemaker2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog4.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                using (FileStream fs = new FileStream(openFileDialog4.FileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(magic_unity)];
                    fs.Seek(0L, SeekOrigin.Begin);
                    fs.Read(header, 0, 4);
                    if (BitConverter.ToUInt32(header.Reverse().ToArray(), 0) != magic_unity)
                    {
                        byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_scenery);
                        byte[] array = new byte[bytes.Length];
                        fs.Seek(0L, SeekOrigin.Begin);
                        fs.Read(array, 0, bytes.Length);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != array[i])
                            {
                                throw new Exception("File ID is invalid. \"" + Encoding.UTF8.GetString(array, 0, array.Length) + "\"");
                            }
                        }
                        save_path = null;
                        pack_status = packed.unpacked;
                        active_dialog = dialog.coursemaker_2;
                        populateTreeview(openFileDialog4.FileName);
                    }
                    else
                    {
                        save_path = null;
                        pack_status = packed.packed;
                        active_dialog = dialog.coursemaker_2;
                        populateTreeview(Unity_Asset_Handler.Load_Asset(openFileDialog4.FileName));
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }
        #endregion

        #region Exit & About ToolStripMenuItems
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to my GT6 Track Path Editors XML Editor!" + Environment.NewLine + Environment.NewLine + "This tool is created by Razerman!");
        } 
        #endregion

        #region SaveFileDialogs
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (save_path != null)
                exportToXml(treeView1, save_path);
            else
            {
                // Show the dialog and get result.
                if (pack_status == packed.packed)
                    saveFileDialog1.FilterIndex = 2;
                else
                {
                    saveFileDialog1.FilterIndex = 1;
                }
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    save_path = saveFileDialog1.FileName;
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            exportToXml(treeView1, saveFileDialog1.FileName);
                            break;
                        case 2:
                            exportToXml(treeView1, saveFileDialog1.FileName);
                            Unity_Asset_Handler.Save_Asset(saveFileDialog1.FileName, active_dialog);
                            break;
                    }
                }
                System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            if (pack_status == packed.packed)
                saveFileDialog1.FilterIndex = 2;
            else
            {
                saveFileDialog1.FilterIndex = 1;
            }
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                save_path = saveFileDialog1.FileName;
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        exportToXml(treeView1, saveFileDialog1.FileName);
                        break;
                    case 2:
                        exportToXml(treeView1, saveFileDialog1.FileName);
                        Unity_Asset_Handler.Save_Asset(saveFileDialog1.FileName, active_dialog);
                        break;
                }
            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }
        #endregion

        #region TreeView
        #region LoadXML
        //Open the XML file, and start to populate the treeview
        private void populateTreeview(string FileName)
        {
            try
            {
                treeView1.BeginUpdate();
                //Just a good practice -- change the cursor to a 
                //wait cursor while the nodes populate
                this.Cursor = Cursors.WaitCursor;
                //First, we'll load the Xml document
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(FileName);
                //Now, clear out the treeview, 
                //and add the first (root) node
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new
                  TreeNode(xDoc.DocumentElement.Name));
                TreeNode tNode = new TreeNode();
                tNode = (TreeNode)treeView1.Nodes[0];
                //We make a call to addTreeNode, 
                //where we'll add all of our nodes
                addTreeNode(xDoc.DocumentElement, tNode);
                //Expand the treeview to show all nodes
                treeView1.ExpandAll();
                treeView1.EndUpdate();
            }
            catch (XmlException xExc)
            //Exception is thrown is there is an error in the Xml
            {
                MessageBox.Show(xExc.Message);
            }
            catch (Exception ex) //General exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default; //Change the cursor back
            }
        }
        private void populateTreeview(Stream stream)
        {
            try
            {
                treeView1.BeginUpdate();
                //Just a good practice -- change the cursor to a 
                //wait cursor while the nodes populate
                this.Cursor = Cursors.WaitCursor;
                //First, we'll load the Xml document
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(stream);
                //Now, clear out the treeview, 
                //and add the first (root) node
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new
                  TreeNode(xDoc.DocumentElement.Name));
                TreeNode tNode = new TreeNode();
                tNode = (TreeNode)treeView1.Nodes[0];
                //We make a call to addTreeNode, 
                //where we'll add all of our nodes
                addTreeNode(xDoc.DocumentElement, tNode);
                //Expand the treeview to show all nodes
                treeView1.ExpandAll();
                treeView1.EndUpdate();
            }
            catch (XmlException xExc)
            //Exception is thrown is there is an error in the Xml
            {
                MessageBox.Show(xExc.Message);
            }
            catch (Exception ex) //General exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default; //Change the cursor back
            }
        }
        //This function is called recursively until all nodes are loaded
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];
                    treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
            else //No children, so add the outer xml (trimming off whitespace)
                treeNode.Text = xmlNode.OuterXml.Trim();
        }
        #endregion

        #region SaveXML (XmlWriter)
        //We use this in the exportToXml and the saveNode 
        //functions, though it's only instantiated once.
        XmlWriter writer = null;

        public void exportToXml(TreeView tv, string filename)
        {
            // Create an XmlWriterSettings object with the correct options. 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("  ");
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.Encoding = Encoding.UTF8;

            // Create the XmlWriter object and write some content.
            writer = XmlWriter.Create(filename, settings);

            //Write our root node
            writer.WriteStartElement(treeView1.Nodes[0].Text);
            foreach (TreeNode node in tv.Nodes)
            {
                saveNode(node.Nodes);
            }
            //Close the root node
            writer.WriteEndElement();
            writer.Close();

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] bits = new byte[3];
            fs.Read(bits, 0, 3);

            // UTF8 byte order mark is: 0xEF,0xBB,0xBF
            if (bits[0] == 0xEF && bits[1] == 0xBB && bits[2] == 0xBF)
            {
                byte[] buffer = new byte[(fs.Length - 3)];
                fs.Read(buffer, 0, (int)(fs.Length - 3));
                fs.Close();
                File.WriteAllBytes(filename, buffer);
            }
            else
                fs.Close();
        }

        private void saveNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    writer.WriteStartElement(node.Text.Replace("&lt;", "<").Replace("&gt;", ">"));
                    saveNode(node.Nodes);
                    writer.WriteFullEndElement();
                }
                else if(!node.Text.Trim().StartsWith("<") && node.Nodes.Count <= 0) //No child nodes, so we just write the text
                {
                    if(String.IsNullOrEmpty(node.Text))
                    {
                        System.Diagnostics.Debug.WriteLine("IsNullOrEmpty");
                    }
                    else
                    {
                        writer.WriteString(node.Text);
                    }
                }
                else if (node.Text.Trim().StartsWith("<") && node.Nodes.Count <= 0)
                {
                   writer.WriteElementString(node.Text.Replace("<", String.Empty).Replace(" />", String.Empty), string.Empty);
                }
            }
        }
        #endregion

        public TreeNode mySelectedNode;
        /* Get the tree node under the mouse pointer and 
   save it in the mySelectedNode variable. */
        private void treeView1_MouseDown(object sender,
          System.Windows.Forms.MouseEventArgs e)
        {
            mySelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            if (mySelectedNode != null && mySelectedNode.Parent != null)
            {
                treeView1.SelectedNode = mySelectedNode;
                treeView1.LabelEdit = true;
                if (!mySelectedNode.IsEditing)
                {
                    mySelectedNode.BeginEdit();
                }
            }
            else
            {
                MessageBox.Show("No tree node selected or selected node is a root node.\n" +
                   "Editing of root nodes is not allowed.", "Invalid selection");
            }
        }

        private void treeView1_AfterLabelEdit(object sender,
                 System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', ',', '!' }) == -1)
                    {
                        // Stop editing without canceling the label change.
                        e.Node.EndEdit(false);
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                           "The invalid characters are: '@',',', '!'",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                       "Node Label Edit");
                    e.Node.BeginEdit();
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (mySelectedNode != null && mySelectedNode.Parent != null)
            {
                treeView1.SelectedNode = mySelectedNode;
                treeView1.LabelEdit = true;
                if (!mySelectedNode.IsEditing)
                {
                    mySelectedNode.BeginEdit();
                }
            }
            else
            {
                MessageBox.Show("No tree node selected or selected node is a root node.\n" +
                   "Editing of root nodes is not allowed.", "Invalid selection");
            }
        }

        #endregion

        private void signToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog5.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                String apk_path = openFileDialog5.FileName;

                string installPath = GetJavaInstallationPath();
                string filePath = @System.IO.Path.Combine(installPath, "bin\\Java.exe");
                String assembly_path = Path.GetDirectoryName(Application.ExecutablePath);
                if (System.IO.File.Exists(filePath))
                {
                    String signapk_path = Path.Combine(assembly_path, "signapk.jar");
                    String certificate_path = Path.Combine(assembly_path, "testkey.x509.pem");
                    String key_path = Path.Combine(assembly_path, "testkey.pk8");

                    if (!File.Exists(signapk_path)){
                        MessageBox.Show(String.Format("Couldn't find signapk.jar from: {0}", signapk_path), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!File.Exists(certificate_path)){
                        MessageBox.Show(String.Format("Couldn't find testkey.x509.pem from: {0}", certificate_path), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!File.Exists(key_path)){
                        MessageBox.Show(String.Format("Couldn't find testkey.pk8 from: {0}", key_path), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Use ProcessStartInfo class.
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardError = true;
                    startInfo.FileName = filePath;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //startInfo.Arguments = "-jar " + "\"" + signapk_path + "\" \"" + certificate_path + "\" \"" + key_path + "\" \"" + apk_path + "\" \"" + (Path.ChangeExtension(apk_path, String.Empty).TrimEnd('.') + "_signed.apk") + "\"";
                    startInfo.Arguments = String.Format("-jar \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", signapk_path, certificate_path, key_path, apk_path, Path.ChangeExtension(apk_path, String.Empty).TrimEnd('.') + "_signed.apk");

                    PerformProcess(startInfo);
                }
                else
                {
                    String key_path = Path.Combine(assembly_path, "testkey.p12");
                    if (!File.Exists(key_path))
                    {
                        MessageBox.Show(String.Format("Couldn't find testkey.p12 from: {0}", key_path), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!File.Exists(apk_path))
                    {
                        MessageBox.Show(String.Format("Couldn't find apk from: {0}", apk_path), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Enabled = false;
                    SplashForm.ShowSplashScreen(this);
                    try
                    {
                        FileStream inputStream = null;
                        FileStream outputStream = null;

                        try
                        {
                            var certificate = new X509Certificate2(key_path, "razerman");
                            inputStream = new FileStream(apk_path, FileMode.Open);
                            outputStream = new FileStream(Path.ChangeExtension(apk_path, String.Empty).TrimEnd('.') + "_signed.apk", FileMode.OpenOrCreate);
                            SignApk.SignPackage(inputStream, certificate, outputStream, true);
                        }
                        catch (Exception b)
                        {
                            MessageBox.Show(b.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(1);
                        }
                        finally
                        {
                            try
                            {
                                if (inputStream != null) inputStream.Close();
                                if (outputStream != null) outputStream.Close();
                            }
                            catch (IOException b)
                            {
                                MessageBox.Show(b.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                        }
                    }
                    catch (Exception b)
                    {
                        MessageBox.Show(b.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    SplashForm.CloseForm();
                    this.Enabled = true;
                }

    //            MessageBox.Show("Couldn't find Java.exe or SignApk.exe and can't sign!" + Environment.NewLine +
    //"Fixes: " + Environment.NewLine +
    //"1. Install Java and check JAVA_HOME Environment Variable is correct" + Environment.NewLine +
    //"2. Place SignApk.exe and ICSharpCode.SharpZipLib.dll in the same directory as this *.exe"
    //, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }

        private string GetJavaInstallationPath()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }

            string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
            {
                string currentVersion = rk.GetValue("CurrentVersion").ToString();
                using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                {
                    return key.GetValue("JavaHome").ToString();
                }
            }
        }

        private void PerformProcess(ProcessStartInfo startinfo)
        {
            this.Enabled = false;
            SplashForm.ShowSplashScreen(this);
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using-statement will close.
                using (Process exeProcess = Process.Start(startinfo))
                {
                    string errors = exeProcess.StandardError.ReadToEnd();
                    if (!String.IsNullOrEmpty(errors))
                        MessageBox.Show(errors, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
            SplashForm.CloseForm();
            this.Enabled = true;
        }
    }

    class SplashForm
    {
        //Delegate for cross thread call to close
        private delegate void CloseDelegate();

        //The type of form to be displayed as the splash screen.
        private static ProcessDialogForm splashForm;

        static public void ShowSplashScreen(Form parent)
        {
            // Make sure it is only launched once.
            if (splashForm != null)
                return;

            //Thread thread = new Thread(new ThreadStart(SplashForm.ShowForm));
            Thread thread = new Thread(() => ShowForm(parent));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        static private void ShowForm(Form parent)
        {
            splashForm = new ProcessDialogForm(parent);
            Application.Run(splashForm);
        }

        static public void CloseForm()
        {
            splashForm.Invoke(new CloseDelegate(SplashForm.CloseFormInternal));
        }

        static private void CloseFormInternal()
        {
            splashForm.Close();
            splashForm.Dispose();
            splashForm = null;
        }
    }
}
