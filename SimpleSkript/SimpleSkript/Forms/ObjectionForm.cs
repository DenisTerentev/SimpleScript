using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SimpleSkript
{
    public partial class ObjectionForm : Form
    {
        #region свойства
        protected string Objection
        {
            get { return objectionRichTextBox.Text; }
            set { objectionRichTextBox.Text = value; }
        }
        protected string Answer
        {
            get { return answerRichTextBox.Text; }
            set { answerRichTextBox.Text = value; }
        }
        #endregion
        public ObjectionForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddNewXmlElement();
        }
        private void AddNewXmlElement()
        {
            if (!string.IsNullOrEmpty(Objection) && !string.IsNullOrWhiteSpace(Objection) &&
               !string.IsNullOrEmpty(Answer) && !string.IsNullOrWhiteSpace(Answer))
            {
                XmlDocument document = new XmlDocument();
                document.Load("Objection.xml");

                XmlElement newElement = document.CreateElement("objectionMain");
                newElement.SetAttribute("objection", Objection);
                newElement.SetAttribute("answer", Answer);

                XmlElement element = document.DocumentElement;
                element.AppendChild(newElement);
                document.Save("Objection.xml");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
