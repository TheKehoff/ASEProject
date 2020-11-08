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
    public partial class Form1 : Form
    {

        bool saved;                 //constant bool to check if saved
        int index;                  //int to check what line of the textbox we are on for prining errors
        string penColour = "white"; //set default colour as white on black background
        Bitmap buffer;              //offscreen bitmap for drawing
        PointF startPos = new PointF(0, 0);
        public Form1()
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
            int i;
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
                    
                    Pen dLinePen = new Pen(Color.FromName(penColour)); //makes new pen in runtime, this allows the user to select from a wide range of colours without the need for multiple pens
                    i = 0;
                    Single[] points1 = new Single[4];             //single array for creating PointF object

                    foreach (var item in command)                 //For each item in command array convert to single and save into array
                    {
                        if (Single.TryParse(item, out Single point))
                            {
                                points1[i] = point;
                                i++;
                            }
                    } 

                    PointF pt1 = new PointF(points1[0], points1[1]); //Grab coodinates from array
                    PointF pt2 = new PointF(points1[2], points1[3]);

                    g.DrawLine(dLinePen, pt1, pt2);                     //Draw line on bitmap
                    this.Refresh();                                  //Call paint event
                    dLinePen.Dispose();                                   
                    break; 
                case "drawTo":                                              //Handles draw to starts at starting pos and draws to argument.
                    Pen dToPen = new Pen(Color.FromName(penColour));
                    i = 0;
                    Single[] points = new Single[2];
                     {
                        foreach (var item in command.Skip(1)) //skips first element in array as it is the command 
                        {
                            if (Single.TryParse(item, out Single point))
                            {
                                points[i] = point;
                                i++;
                            }
                            else
                            {
                                txtboxConsoleOut.AppendText(Environment.NewLine);
                                txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                                txtboxConsoleOut.AppendText(Environment.NewLine);
                                txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: This Command Only Numerical Chars 0-300";

                            }
                        }
                    }

                    PointF ptTo = new PointF(points[0], points[1]);
                    g.DrawLine(dToPen, startPos, ptTo);
                    this.Refresh();
                    dToPen.Dispose();
                    startPos = ptTo;
                    break;
                case "moveTo":
                    Single[] points2 = new Single[2];
                    i = 0;
                    foreach (var item in command)
                    {
                        if(Single.TryParse(item, out Single point))
                        {
                            points2[i] = point;
                            i++;
                        }
                    }

                    PointF ptMTo = new PointF(points2[0], points2[1]);
                    startPos = ptMTo;
                    break;
                case "rect":
                    int[] points3 = new int[2];
                    Pen rectPen = new Pen(Color.FromName(penColour));
                    i = 0;
                    foreach (var item in command)
                    {
                        if (int.TryParse(item, out int point))
                        {
                            points3[i] = point;
                            i++;
                        }
                    }
                    Size siRect = new Size(points3[0], points3[1]);
                    System.Drawing.Point.Round(startPos);
                    Point rectPoint = new Point((Size)Point.Round(startPos));
                    Rectangle rect = new Rectangle(rectPoint, siRect);
                    g.DrawRectangle(rectPen, rect);
                    this.Refresh();
                    rectPen.Dispose();
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
            Graphics g = Graphics.FromImage(buffer);
            g.Clear(Color.Black);
            this.Refresh();
        }
    }
}