using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ASEProject
{
    public partial class frmGraphics : Form
    {
        Bitmap buffer;
        public frmGraphics()
        {
            buffer = new Bitmap(300, 300);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnExectute_Click(object sender, EventArgs e)
        {
            var lines = rtxtCommandLine.Lines;                           //Splits the lines in text window and adds them to an array 
            Array.ForEach(lines, s => commandHandler(s));                //For each line send the string to the command handler method

        }

        private void commandHandler(string s)
        {
            char[] delimiterChars = { ' ', ',' };
            String[] command = s.Split(delimiterChars);
            switch (command[0])
            {
                case "drawTo":

                    break;
                case "moveTo":
                    break;
                case "rect":
                    break;
                case "circle":
                    break;
                case "trig":
                    System.Console.WriteLine("thisworks");
                    break;
                case "fill":
                    break;
                case "reset":
                    break;
                case "clear":
                    break;

            }
        }

        private void rtxtCommandLine_TextChanged(object sender, EventArgs e)
        {

        }
    }
}