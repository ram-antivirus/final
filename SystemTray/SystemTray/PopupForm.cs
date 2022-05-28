using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace SystemTray
{
    public partial class PopupForm : Form
    {
        string m;
       

        public PopupForm()
        {
           
            InitializeComponent();
            
            
        }
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            //const int margin = 10;
            //int x = Screen.PrimaryScreen.WorkingArea.Right -
            //    this.Width - margin;
            //int y = Screen.PrimaryScreen.WorkingArea.Bottom -
            //    this.Height - margin;
            //this.Location = new Point(x, y);
            try
            {
                string abc = AppDomain.CurrentDomain.BaseDirectory;
                string filepath = abc + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                string message = File.ReadAllText(filepath);
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                this.Left = screenWidth - this.Width;
                this.Top = screenHeight - this.Height;
                richTextBox1.ReadOnly = true;
                Color color = System.Drawing.ColorTranslator.FromHtml("#f0f0f0");
                richTextBox1.BackColor = Color.FromArgb(240, 240, 240);
                richTextBox1.Cursor = Cursors.Arrow;
                richTextBox1.TabStop = false;
                richTextBox1.Enter += RichTextBox1_Enter;
                richTextBox1.Text = message;
                //var popupNotifier = new PopupNotifier();
                //popupNotifier.TitleText = "RAM Antivirus";
                //popupNotifier.ContentText = "Found Virus-->" + message;
                //popupNotifier.IsRightToLeft = true;

                //popupNotifier.Popup();
                //popupNotifier.Close += PopupNotifier_Close;
            }
            catch (Exception w)
            { }




        }

        private void RichTextBox1_Enter(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void PopupNotifier_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopupForm_Move(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a = richTextBox1.Text;

            string strTargetProcessName = System.IO.Path.GetFileNameWithoutExtension(a);

            Process[] Processes = Process.GetProcessesByName(strTargetProcessName);
            foreach (var process in Process.GetProcessesByName(strTargetProcessName))
            {
                process.Kill();
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "whitelistedmd5.txt";
            File.AppendAllText(filepath, richTextBox1.Text + "\n");
            this.Close();
        }
    }
}
