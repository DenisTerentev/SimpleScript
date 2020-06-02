using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SimpleSkript;


namespace SimpleSkript
{
    public partial class Form1 : Form
    {
        //Person person = new Person();
        List<string> textScript = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScriptLoad();
            ObjectionLoad();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ObjectionForm form = new ObjectionForm();
            if (form.ShowDialog() != DialogResult.OK) return;
            listBox1.Items.Clear();
            ObjectionLoad();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DeleteItem(listBox1.SelectedItem.ToString());
            listBox1.Items.Clear();
            ObjectionLoad();
        }

        private void ScriptLoad()
        {
            using (StreamReader reader = new StreamReader("TextScript.txt", Encoding.UTF8))
            {
                while (true)
                {
                    string str = reader.ReadLine();
                    if (str == null) break;
                    textScript.Add(str);
                }
            }
            foreach (string str in textScript)
            {
                richTextBox1.Text += str;
                richTextBox1.Text += '\n';
            }
        }
        private void ObjectionLoad()
        {
            List<string> objections = new List<string>();
            using (FileStream fs = new FileStream("Objection.xml", FileMode.Open))
            {
                using (XmlTextReader reader = new XmlTextReader(fs))
                {
                    reader.MoveToContent();
                    if (reader.Name != "objections") throw new ArgumentException("Incorrect file format");
                    do
                    {
                        if (!reader.Read()) break;
                        if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "objections") break;
                        if (reader.NodeType == XmlNodeType.EndElement) continue;
                        reader.Read();
                        if (reader.Name == "objectionMain")
                        {
                            objections.Add(reader.GetAttribute("objection"));
                        }
                    } while (!reader.EOF);
                }
            }
            foreach (string str in objections)
            {
                listBox1.Items.Add(str);
            }
            listBox1.Refresh();
        }
        private void DeleteItem(string item)
        {
            XmlDocument document = new XmlDocument();
            document.Load("Objection.xml");
            XmlNode deleteNode = null;
            foreach (XmlNode node in document.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element) //проверка ноды, что это элемент
                {
                    //XmlAttribute xmlAttribute = node.Attributes["objection"];
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        foreach (XmlAttribute xmlAttribute in childNode.Attributes)
                        {
                            if (xmlAttribute.Name == "objection" && xmlAttribute.Value == item)
                                deleteNode = childNode;
                        }
                    }
                }
            }
            if (deleteNode != null)
            {
                deleteNode.ParentNode.RemoveChild(deleteNode);
                document.Save("Objection.xml");
            }
        }
        private void ShowXmlAnswer(string objection)
        {
            XmlDocument document = new XmlDocument();
            document.Load("Objection.xml");
            string answer = null;
            foreach (XmlNode node in document.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        foreach (XmlAttribute xmlAttribute in childNode.Attributes)
                        {
                            if (xmlAttribute.Name == "objection")
                                if (String.Equals((string)xmlAttribute.Value, (string)objection))
                                    answer = childNode.Attributes[1].Value.ToString();
                        }

                    }
                }
            }
            if (!string.IsNullOrEmpty(answer) && !string.IsNullOrWhiteSpace(answer))
                MessageBox.Show(answer);
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            ShowXmlAnswer(listBox1.SelectedItem.ToString());
        }

        private void СохранитьИзмененныйСкриптToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textScript.Clear();
            textScript.Add(richTextBox1.Text);
            StreamWriter writer = File.CreateText("TextScript.txt");
            writer.WriteLine(textScript[0]);
            writer.Close();
        }
    }
}
