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
using System.Xml;

namespace ASEProject
{
    public partial class frmGraphics : Form
    {
        bool saved;
        int index;
        string penColour = "white";
        Bitmap buffer;
        public frmGraphics()
        {
           // string penColour = "white";
            buffer = new Bitmap(300, 300);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnExectute_Click(object sender, EventArgs e)
        {
            txtboxConsoleOut.Text = "";
            index = 0;
            var lines = rtxtCommandLine.Lines;                           //Splits the lines in text window and adds them to an array 
            Array.ForEach(lines, s => commandHandler(s));                //For each line send the string to the command handler method

        }

        private void commandHandler(string s)                            //Handles parsing and execution of commands
        {
            index++;                                                     //Index for which line we are on
            char[] delimiterChars = { ' ', ',' };                        //Initiates a char array for the delimiter characters, this is where the command will be split.
            String[] command = s.Split(delimiterChars);                  //Splits the string, first position in the array will be the command.
            switch (command[0])
            {
                case "penColour":
                    if (System.Text.RegularExpressions.Regex.IsMatch(command[1], "^[a-zA-Z]"))
                    {
                        penColour = command[1];
                    }
                    else
                    {
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: This Command Only Alphabetical Chars";
                    }   
                        break;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //Checks if the user has saved before exiting and warns them before doing so
        {
            if(saved == false)
            {
                DialogResult result = MessageBox.Show("You have not saved are you sure you want to exit?", "Exit Warning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
            else { Environment.Exit(0); }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e) //Allows the user to save program in CLI window to txt file.
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = "C:\\" ;
            sf.Title = "Save CLI Program";
            sf.DefaultExt = "txt";
            sf.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            sf.FilterIndex = 2;
            sf.RestoreDirectory = true;
            if(sf.ShowDialog() == DialogResult.OK)
            {
                rtxtCommandLine.SaveFile(sf.FileName , (RichTextBoxStreamType.PlainText));
                saved = true;
            }

        }
    }
}