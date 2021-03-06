﻿using System;
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
        public bool fill = false;                  //bool to check if fill is on or off
        public bool saved;                 //constant bool to check if saved
        public int index;                  //int to check what line of the textbox we are on for prining errors
        public string penColour = "white"; //set default colour as white on black background
        public Bitmap buffer;              //offscreen bitmap for drawing
        public PointF startPos = new PointF(0, 0);
        public Form1()
        {
            buffer = new Bitmap(300, 300);
            InitializeComponent();
        }

        private void btnExectute_Click(object sender, EventArgs e)
        {
            txtboxConsoleOut.Clear();
            index = 0;
            var lines = rtxtCommandLine.Lines;                           //Splits the lines in text window and adds them to an array 
            var line = txtCLI.Lines;

            // CommandHandling command = new CommandHandling();
            
            if(lines.Length <= 0)
            {
                Array.ForEach(line, s => commandHandler(s));                 //if there is a command in CLI window run that otherwise run program window
            }
            else
            {
                Array.ForEach(lines, s => commandHandler(s));                //For each line send the string to the command handler method
            }
 

        }

       private void commandHandler(string s)                            //Handles parsing and execution of commands
        {
            int i;
            Graphics g = Graphics.FromImage(buffer);
            index++;                                                     //Index for which line we are on
            char[] delimiterChars = { ' ', ',' };                        //Initiates a char array for the delimiter characters, this is where the command will be split.
            String[] command = s.Split(delimiterChars);                  //Splits the string, first position in the array will be the command.
            string x = command[0];
            switch (x.ToLower())
            {
                case "pencolour":                                         
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
                case "drawline":
                    
                    Pen dLinePen = new Pen(Color.FromName(penColour)); //makes new pen in runtime, this allows the user to select from a wide range of colours without the need for multiple pens
                    i = 0;
                    Single[] pointsLine = new Single[command.Length];             //single array for creating PointF object

                    foreach (var item in command.Skip(1))                 //For each item in command array convert to single and save into array
                    {
                        if (Single.TryParse(item, out Single point))
                            {
                                pointsLine[i] = point;
                                i++;
                            }
                    } 

                    PointF pt1 = new PointF(pointsLine[0], pointsLine[1]); //Grab coodinates from array
                    PointF pt2 = new PointF(pointsLine[2], pointsLine[3]);

                    g.DrawLine(dLinePen, pt1, pt2);                     //Draw line on bitmap
                    this.Refresh();                                  //Call paint event
                    dLinePen.Dispose();                                   
                    break; 
                case "drawto":                                              //Handles draw to starts at starting pos and draws to argument.
                    Pen dToPen = new Pen(Color.FromName(penColour));
                    i = 0;
                    Single[] points = new Single[command.Length];
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
                    startPos = ptTo;           //sets startpos to new target
                    break;
                case "moveto":
                    Single[] pointsTo = new Single[command.Length];
                    i = 0;
                    foreach (var item in command.Skip(1))
                    {
                        if(Single.TryParse(item, out Single point))
                        {
                            pointsTo[i] = point;
                            i++;
                        }
                    }

                    PointF ptMTo = new PointF(pointsTo[0], pointsTo[1]);
                    startPos = ptMTo;
                    break;
                case "rect":
                    int[] pointsRect = new int[command.Length];
                    Pen rectPen = new Pen(Color.FromName(penColour));
                    SolidBrush rectBrush = new SolidBrush(Color.FromName(penColour));  //solid brush allows for fill
                    i = 0;
                    foreach (var item in command.Skip(1))
                    {
                        if (int.TryParse(item, out int point))
                        {
                            pointsRect[i] = point;
                            i++;
                        }
                    }
                    int check = command.Length;
                    if(command.Length < 3) //checks argument 
                    {
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: Please Enter A Numerical Value x,y";
                    }
                    Size siRect = new Size(pointsRect[0], pointsRect[1]);
                    Point rectPoint = new Point((Size)Point.Round(startPos));
                    Rectangle rect = new Rectangle(rectPoint, siRect);

                    if (fill == true)                               //if statements for fill base
                    {
                        g.FillRectangle(rectBrush, rect);
                        this.Refresh();
                        rectPen.Dispose();
                        rectBrush.Dispose();

                    }
                    if (fill == false)
                    {
                        g.DrawRectangle(rectPen, rect);
                        this.Refresh();
                        rectPen.Dispose();
                        rectBrush.Dispose();
                    }
                    break;
                case "circle":
                    int pointsCirc = new int();
                    Pen circPen = new Pen(Color.FromName(penColour));
                    SolidBrush circBrush = new SolidBrush(Color.FromName(penColour));
                    try
                    {
                        if (int.TryParse(command[1], out int pointCirc))
                        {
                            pointsCirc = pointCirc;
                        }
                        else
                        {
                            txtboxConsoleOut.AppendText(Environment.NewLine);
                            txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                            txtboxConsoleOut.AppendText(Environment.NewLine);
                            txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: Please Enter Numerical Value";
                        }
                    }
                    catch(IndexOutOfRangeException)
                    {
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: Please Enter Numerical Value";
                    }
                    Size siCirc = new Size(pointsCirc, pointsCirc);
                    Point circPoint = new Point((Size)Point.Round(startPos));
                    Rectangle circle = new Rectangle(circPoint, siCirc);
                    if (fill == true)
                    {
                        g.FillEllipse(circBrush, circle);
                        this.Refresh();
                        circBrush.Dispose();
                        circPen.Dispose();
                    }
                    else if (fill == false)
                    {
                        g.DrawEllipse(circPen, circle);
                        this.Refresh();
                        circPen.Dispose();
                        circBrush.Dispose();
                    }


                    break;
                case "trig":
                    int[] pointsTrig = new int[command.Length];
                    Pen trigPen = new Pen(Color.FromName(penColour));
                    SolidBrush trigFill = new SolidBrush(Color.FromName(penColour));
                    i = 0;
                    foreach(var item in command.Skip(1))
                    {
                        if(int.TryParse(item, out int point))
                        {
                            pointsTrig[i] = point;
                            i++;
                        }    
                    }
                    Point trigStart = new Point((Size)Point.Round(startPos));
                    Point trigLength = new Point((int)startPos.X + pointsTrig[0], (int)startPos.Y);
                    Point trigHight = new Point((int)startPos.X + pointsTrig[0] / 2, (int)startPos.Y + pointsTrig[1]);
                    Point[] trigFull = { trigStart, trigLength, trigHight };
                    if(fill == true)
                    {
                        
                        g.FillPolygon(trigFill, trigFull);
                        this.Refresh();
                        trigFill.Dispose();
                        trigPen.Dispose();
                    }
                    else if(fill == false)
                    {
                        g.DrawPolygon(trigPen, trigFull);
                        this.Refresh();
                        trigPen.Dispose();
                        trigFill.Dispose();
                    }

                    break;
                case "fill":
                    if ((command[1] == "on") || (command[1] == "On"))
                    {
                        fill = true;
                    }
                    else if ((command[1] == "off") || (command[1] == "Off"))
                    {
                        fill = false;
                    }
                    else
                    {
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                        txtboxConsoleOut.AppendText(Environment.NewLine);
                        txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Argument: This Command Only Accpets: On or Off";
                    }
                    break;
                case "reset":
                    startPos = new PointF(0, 0);
                    break;
                case "clear":
                    g.Clear(Color.Black);
                    this.Refresh();
                    break;
                default:
                    txtboxConsoleOut.AppendText(Environment.NewLine);
                    txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Error on Line: " + index;
                    txtboxConsoleOut.AppendText(Environment.NewLine);
                    txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Invalid Command";
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

        private void pboxDrawingPanel_Paint(object sender, PaintEventArgs e) //Paint event for photobox
        {
            Graphics windowG = e.Graphics;
            windowG.DrawImageUnscaled(buffer, 0, 0);
        }

        private void btnClear_Click(object sender, EventArgs e) //Click event for clear button
        {
            Graphics g = Graphics.FromImage(buffer);
            g.Clear(Color.Black);
            this.Refresh();
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e) //Allows the user to load txt file
        {
            using(OpenFileDialog of = new OpenFileDialog())
            {
                try
                {
                    of.Filter = "Text Files (*.txt)|*.txt";
                    if(of.ShowDialog() == DialogResult.OK)
                    {
                        rtxtCommandLine.LoadFile(of.FileName, RichTextBoxStreamType.PlainText);
                    }
                }
                catch(Exception)
                {
                    txtboxConsoleOut.Text = txtboxConsoleOut.Text + "Please Select Valid .txt File";
                }
            }
        }

    }
}