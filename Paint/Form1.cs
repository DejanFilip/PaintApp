using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Paint
{
    
    public partial class Paint : Form
    {
        private Point Start, End;
        private int height = SystemInformation.PrimaryMonitorSize.Height;
        private int width = SystemInformation.PrimaryMonitorSize.Width;
        private Bitmap bm;
        private Graphics g;
        private bool painting = false;
        private Point dx, dy;
        private Pen p = new Pen(Color.Black, 1);
        private Pen erase = new Pen(Color.White, 30);
        private int index;
        private int x, y, sX, sY, cX, cY;
        private ColorDialog cd = new ColorDialog();
        private Color new_color;
        public Paint()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            bm = new Bitmap(width, height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            picBox.Image = bm;
            p.Width = (float)PaintBrushSize.Value;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round ,System.Drawing.Drawing2D.DashCap.Round);
        }
        private void Paint_Load(object sender, EventArgs e)
        {
            picBox.SizeMode = PictureBoxSizeMode.Normal;
        }
        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }
        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }
        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }
        private void btn_rectangle_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 5;
        }
        private void btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            index = 8;
        }
       

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (painting)
            {
                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);

                }
                if (index == 4)
                {
                    g.DrawRectangle(p, cX, cY, sX, sY);

                }
                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);

                }
                if (index == 8)
                {
                    DrawTriangle(g);
                }
            }
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            painting = true;
            dy = e.Location;
            cX = e.X;
            cY = e.Y;
            if(e.Button== MouseButtons.Left)
            {
                Start = End = e.Location;
                painting = true;
            }
        }
        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            painting = false;

            sX = x - cX;
            sY = y - cY;

            if (index == 3)
            {
                g.DrawEllipse(p, cX, cY, sX, sY);

            }
            if (index == 4)
            {
                g.DrawRectangle(p, cX, cY, sX, sY);

            }
            if (index == 5)
            {
                g.DrawLine(p, cX, cY, x, y);

            }
            if(index == 8)
            {
                DrawTriangle(g);
            }
        }

       

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            lbCordinates.Text = "Cordinates"+"\n"+"X:" + e.X + "\n" + "Y:" + e.Y;
            
            if (painting)
            {
                if(index == 1)
                {
                    dx = e.Location;
                    g.DrawLine(p, dx, dy);
                    dy = dx;
                    Invalidate();
                }
                if (index == 2)
                {
                    dx = e.Location;
                    g.DrawLine(erase, dx, dy);
                    dy = dx;
                    Invalidate();
                }
            }
            picBox.Refresh();

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

        private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                FloodFill(bm, e.X, e.Y, new_color);
                Invalidate();

            }
        }

        private void pic_color_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            picBox.Image = bm;
            picBox.SizeMode = PictureBoxSizeMode.Normal;
            index = 0;
            Invalidate();
        }

        void FloodFill(Bitmap bitmap, int x, int y, Color color)
        {
            BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);

            LinkedList<Point> check = new LinkedList<Point>();
            int floodTo = color.ToArgb();
            int floodFrom = bits[x + y * data.Stride / 4];
            bits[x + y * data.Stride / 4] = floodTo;

            if (floodFrom != floodTo)
            {
                check.AddLast(new Point(x, y));
                while (check.Count > 0)
                {
                    Point cur = check.First.Value;
                    check.RemoveFirst();

                    foreach (Point off in new Point[] {
                new Point(0, -1), new Point(0, 1),
                new Point(-1, 0), new Point(1, 0)})
                    {
                        Point next = new Point(cur.X + off.X, cur.Y + off.Y);
                        if (next.X >= 0 && next.Y >= 0 &&
                            next.X < data.Width &&
                            next.Y < data.Height)
                        {
                            if (bits[next.X + next.Y * data.Stride / 4] == floodFrom)
                            {
                                check.AddLast(next);
                                bits[next.X + next.Y * data.Stride / 4] = floodTo;
                            }
                        }
                    }
                }
            }

            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, width, height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
            }
        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog Op = new OpenFileDialog();
            DialogResult dr = Op.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image imageIn = Image.FromFile(Op.FileName);
                bm = new Bitmap(imageIn, width, height);
                g = Graphics.FromImage(bm);
                picBox.Image = bm;
               
            }
        }
        private void PaintBrushSize_ValueChanged(object sender, EventArgs e)
        {
            p.Width = (float)PaintBrushSize.Value;
        }
        private void DrawTriangle(Graphics g)
        {
            End = PointToClient(MousePosition);
            double xMid = (Start.X + End.X) / 2;
            Point first = new Point(Start.X, End.Y);
            Point mid = new Point((int)xMid, Start.Y);
            g.DrawLine(p, first, mid);
            g.DrawLine(p, first, End);
            g.DrawLine(p, End, mid);
        }

    }
}
