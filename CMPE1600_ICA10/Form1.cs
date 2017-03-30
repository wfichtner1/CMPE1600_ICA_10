using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using GDIDrawer;

namespace CMPE1600_ICA10
{
    public partial class Form1 : Form
    {
        public struct MousePoint
        {
            public Point _point;
            public int _drawAmount;

            public MousePoint(Point point, int pixels)
            {
                _point = point;
                _drawAmount = pixels;
            }
        }

        static CDrawer canvas = new CDrawer(800,600, false);
        static object placeHolder = new object();
        static object passer = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MousePoint userClick = new MousePoint();
            Point point;
            if(canvas.GetLastMouseLeftClickScaled(out point))
            {
                userClick = new MousePoint(point, trackBar1.Value);
                passer = (object)userClick;
                Thread threadOne = new Thread(Wanderer);
                threadOne.Start(passer);                
            }
        }

        static void Wanderer(object holder)
        {
            MousePoint start = (MousePoint)holder;
            Random rand = new Random();
            Color drawCol = RandColor.GetKnownColor();
            Point velocity = new Point(1, 1);
            Point temp = start._point;
            lock (placeHolder)
            {
                canvas.SetBBScaledPixel(temp.X += velocity.X, temp.Y += velocity.Y, drawCol);
                canvas.Render();
            }

            for (int i = 0; i < start._drawAmount; i++)
            {
                //drawCol = RandColor.GetKnownColor();
                temp.X += rand.Next(-1, 2);
                temp.Y += rand.Next(-1, 2);
                temp.X = (temp.X < 0) ? 0 : temp.X;
                temp.Y = (temp.Y < 0) ? 0 : temp.Y;
                temp.X = (temp.X > 799) ? 0 : temp.X;
                temp.Y = (temp.Y > 599) ? 0 : temp.Y;

                lock(placeHolder)
                {                    
                    canvas.SetBBScaledPixel(temp.X, temp.Y, drawCol);
                    canvas.Render();
                    Thread.Sleep(1);
                }
            }

            

            
        }
    }
}
