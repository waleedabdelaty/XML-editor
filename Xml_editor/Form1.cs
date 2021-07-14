using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace Xml_editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //writeLineNumbers(textBox1.Lines.Length);
        }
        public bool check_error = false;
        private void button1_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            if (filePath != null) textBox1.Text = File.ReadAllText(filePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|json files (*.json)|*.json|All files (*.*)|*.*"; // if you want to save text files
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                {
                    // Insert your code to write the stream here.
                    writer.Write(textBox1.Text);
                    writer.Close();
                }
            }
        }
        string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                // Handle and throw if fatal exception here; don't just ignore them
                return xml;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < textBox1.Lines.Length; i++)
            {
                string line = textBox1.Lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    int loc = line.IndexOf('>', j + 1, line.Length - j - 1);
                    if (loc != -1) line = line.Insert(loc + 1, "/n/r");
                }
                textBox1.Lines[i] = line;
            }
            int counter = 0; // for lines
            Stack<string> s = new Stack<string>();
            int[] padding = new int[textBox1.Lines.Length + 1];
            padding[0] = 0;
            padding[textBox1.Lines.Length - 1] = 0;
            string[] lines = textBox1.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int j = 0; j < lines.Length - 1; j++)//reading every line in text 
            {
                string line = lines[j];
                for (int i = 0; i < line.Length; i++)
                {
                    string s1;
                    if (line[i] == '<')
                    {

                        int loc = line.IndexOf('>', i + 1, line.Length - i - 1);
                        if (loc != -1)
                        {
                            if (line[i + 1] == '/')
                            {
                                s1 = line.Substring(i + 2, loc - i - 2);

                            }
                            else
                            {
                                s1 = line.Substring(i + 1, loc - i - 1);
                            }
                            /////////////////////////////////////
                            if (counter == 0)
                            {
                                s.Push(s1 + counter.ToString());

                            }
                            else
                            {
                                string modified_data = s1 + counter.ToString();
                                //string temp = s1;
                                string top = s.Peek();
                                var original_top = Regex.Replace(top, @"[\d-]", string.Empty); //remove numbers from text

                                if (s1 != original_top && s.Count != 0)
                                    s.Push(modified_data);
                                else s.Pop();
                            }
                        }
                    }

                }
                padding[counter + 1] = s.Count;
                counter++;
            }

            for (int i = 0; i < lines.Length - 1; i++)
            {
                lines[i] = lines[i].PadLeft(padding[i] + lines[i].Length, ' '); // add space to left of lines
                lines[i] = lines[i].PadLeft(padding[i] + lines[i].Length, ' '); // add space to left of lines
                lines[i] = lines[i].PadLeft(padding[i] + lines[i].Length, ' '); // add space to left of lines

            }
            textBox1.Lines = lines;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            int counter = 0;
            Stack<string> s = new Stack<string>();
            using (StringReader reader = new StringReader(textBox1.Text))//reading every line in text 
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    counter++;
                    for (int i = 0; i < line.Length; i++)
                    {
                        string s1;
                        if (line[i] == '<')
                        {
                            int loc = line.IndexOf('>', i + 1, line.Length - i - 1);
                            if (loc != -1)
                            {
                                if (line[i + 1] == '/')
                                { s1 = line.Substring(i + 2, loc - i - 2); }
                                else
                                {
                                    s1 = line.Substring(i + 1, loc - i - 1);
                                }
                                /////////////////////////////////////
                                if (counter == 1) { s.Push(s1 + counter.ToString()); }
                                else
                                {
                                    string modified_data = s1 + counter.ToString();
                                    //string temp = s1;
                                    string top = s.Peek();
                                    var original_top = Regex.Replace(top, @"[\d-]", string.Empty); //remove numbers from text

                                    if (s.Count == 0 || s1 != original_top) s.Push(modified_data);
                                    else s.Pop();
                                }

                            }

                        }

                    }
                }
            }
            if (s.Count == 0) { MessageBox.Show("No Errors"); check_error = true; }
            else
            {
                // print message for error
                string message = "";
                string top;
                ////// remove duplicate elements from stack 
                Stack<string> temp = s;
                List<string> tags = new List<string>();
                while (temp.Count != 0)
                {
                    string str = s.Peek(); s.Pop();
                    tags.Add(str);
                }
                int length = tags.Count;
                for (int i = 0; i < length; i++)
                {
                    if (i < tags.Count)
                    {
                        string org_name = Regex.Replace(tags[i], @"[\d-]", string.Empty);
                        bool found = false;
                        for (int j = 0; j < tags.Count && j != i; j++)
                        {
                            if (org_name == Regex.Replace(tags[j], @"[\d-]", string.Empty)) { tags.RemoveAt(j); found = true; }
                        }
                        if (found && i < tags.Count) { tags.RemoveAt(i); }
                    }
                }
                for (int i = 0; i < tags.Count; i++)
                { temp.Push(tags[i]); }
                s = temp;
                ///////////// print stack contents to error message
                while (s.Count != 0)
                {
                    top = s.Peek();
                    s.Pop();
                    int i;
                    for (i = 0; i < top.Length; i++)//  get index of line num in string
                    {
                        if (top[i] >= '1' && top[i] <= '9') break;
                    }
                    //if (i == top.Length) i = 0;
                    string temp_message = "Error at " + top.Substring(0, i) + " tag" + " " + "at line " + top.Substring(i, top.Length - i);
                    message = message + temp_message + Environment.NewLine; // new line in message
                }
                MessageBox.Show(message);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
           

        }






        private void button6_Click(object sender, EventArgs e)
        {
            


        }

        private void button7_Click(object sender, EventArgs e)
        {
           





        }

        private void button8_Click(object sender, EventArgs e)
        {


          

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
               
    }
}
