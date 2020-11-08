/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    class Drawing : Form1
    {
        Graphics g = Graphics.FromImage(buffer);
        public void drawTo(string[] s)
        {
            Pen dToPen = new Pen(Color.FromName(penColour));
            int i = 0;
            Single[] points = new Single[2];
            {
                foreach (var item in s.Skip(1)) //skips first element in array as it is the command 
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
            return;
        }
    }
}
*/