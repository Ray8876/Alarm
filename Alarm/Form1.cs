using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using MetroFramework.Forms;
using System.Xml;

namespace Alarm
{
    public partial class Form1 : MetroForm
    {
        SoundPlayer player = new SoundPlayer();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = (1000) * (1);
            timer1.Enabled = true;
            timer1.Start();

            try
            {
                //读取配置
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("sound.xml");
                XmlNodeList xml = xmlDoc.SelectSingleNode(@"configuration/path").ChildNodes;
                foreach (XmlNode Node in xml)
                {
                    player.SoundLocation = Node.InnerText;
                }
                
            }
            catch (Exception)
            {

            }
            
            notifyIcon1.Visible = false;

            try
            {
                //读取配置
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("data.xml");
                XmlNodeList xml = xmlDoc.SelectNodes(@"configuration/time_data");
                foreach (XmlNode Node in xml)
                {
                    listBox1.Items.Add(Node.InnerText);
                }
            }
            catch(Exception)
            {

            }



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Label3.Text = DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2");

            for(int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString() == Label3.Text.ToString())
                {
                    player.Load();
                    player.PlayLooping();
                }

            }

        }


        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string add_time = "";
            string temp = metroTextBox1.Text;
            if (!panduan(temp, 1))
            {
                MessageBox.Show("输入有误");
                return;
            }
            add_time = add_time + string.Format("{0:D2}", temp);
            temp = metroTextBox2.Text;
            if (!panduan(temp, 2))
            {
                MessageBox.Show("输入有误");
                return;
            }
            add_time = add_time + ":" + string.Format("{0:D2}", temp);
            temp = metroTextBox3.Text;
            if (!panduan(temp, 3))
            {
                MessageBox.Show("输入有误");
                return;
            }
            add_time = add_time + ":" + string.Format("{0:D2}", temp);
            listBox1.Items.Add(add_time);

            update_xml();
        }

        private void update_xml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclar;
            xmlDeclar = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(xmlDeclar);

            XmlElement xmlElement = xmlDocument.CreateElement("", "configuration", "");
            xmlDocument.AppendChild(xmlElement);
            XmlNode root = xmlDocument.SelectSingleNode("configuration");


            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                XmlElement temp = xmlDocument.CreateElement("time_data");
                temp.InnerText = listBox1.Items[i].ToString();
                root.AppendChild(temp);
            }
            xmlDocument.Save("data.xml");
        }

        private bool panduan(string temp, int v)
        {
            if (temp == "" || temp.Length != 2)
                return false;
            for (int i = 0; i < temp.Length; i++)
            {
                if (!Char.IsNumber(temp, i))
                    return false;
            }
            if (v == 1)
            {
                if (Convert.ToInt32(temp)>=24 || Convert.ToInt32(temp) < 0)
                {
                    return false;
                }
            }
            else if (v == 2)
            {
                if (Convert.ToInt32(temp) >= 60 || Convert.ToInt32(temp) < 0)
                {
                    return false;
                }
            }
            else
            {
                if (Convert.ToInt32(temp) >= 60 || Convert.ToInt32(temp) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            update_xml();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Design By Ray8876!\nver 4.0");
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(dlg.FileName);
                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclar;
                xmlDeclar = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDocument.AppendChild(xmlDeclar);

                XmlElement xmlElement = xmlDocument.CreateElement("", "configuration", "");
                xmlDocument.AppendChild(xmlElement);
                XmlNode root = xmlDocument.SelectSingleNode("configuration");


                XmlElement temp = xmlDocument.CreateElement("path");
                temp.InnerText = dlg.FileName;
                root.AppendChild(temp);

                xmlDocument.Save("sound.xml");
                player.SoundLocation = dlg.FileName;
            }

        }
    }
}
