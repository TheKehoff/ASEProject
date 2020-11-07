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
        bool saved;                 //constant bool to check if saved
        int index;                  //int to check what line of the textbox we are on for prining errors
        string penColour = "white"; //set default colour as white on black background
        Bitmap buffer;              //offscreen bitmap for drawing
        PointF startPos = new PointF(0, 0);
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
            txtboxConsoleOut.Text = "";
            index = 0;
            var lines = rtxtCommandLine.Lines;                           //Splits the lines in text window and adds them to an array 
            Array.ForEach(lines, s => commandHandler(s));                //For each line send the string to the command handler method

        }

        private void commandHandler(string s)                            //Handles parsing and execution of commands
        {
            Graphics g = Graphics.FromImage(buffer);
            index++;                                                     //Index for which line we are on
            char[] delimiterChars = { ' ', ',' };                        //Initiates a char array for the delimiter characters, this is where the command will be split.
            String[] command = s.Split(delimiterChars);                  //Splits the string, first position in the array will be the command.
            switch (command[0])
            {
                case "penColour":                                         
                    if (System.Text.RegularExpressions.Regex.IsMatch(command[1], "^[a-zA-Z]")) //Makes sure the user can only enter alphabetical chars
                    {
                        penColour = command[1]; //Sets pen colour according to NET 3.5 System.Drawing.Color
                    }
                    else //Prints error message incase of invalid argument
                    {
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: This Command Only Alphabetical Chars";
                    }   
                        break;
                case "drawLine":
                   
                    Pen pen = new Pen(Color.FromName(penColour)); //makes new pen in runtime, this allows the user to select from a wide range of colours without the need for multiple pens
                    int i = 0;
                    Single[] spoints = new Single[4];             //single array for creating PointF object

                    foreach (var item in command)                 //For each item in command array convert to single and save into array
                    {
                        if (Single.TryParse(item, out Single point))
                            {
                            spoints[i] = point;
                            i++;
                            }
                    } 

                    PointF pt1 = new PointF(spoints[0], spoints[1]); //Grab coodinates from array
                    PointF pt2 = new PointF(spoints[2], spoints[3]);

                    g.DrawLine(pen, pt1, pt2);                       //Draw line on bitmap
                    this.Refresh();                                  //Call paint event
                    pen.Dispose();                                   

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
                    startPos = new PointF(0, 0);
                    break;
                case "clear":
                    g.Clear(Color.Black);
                    this.Refresh();
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

        private void pboxDrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics windowG = e.Graphics;
            windowG.DrawImageUnscaled(buffer, 0, 0);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            this.Refresh();
        }
    }
}