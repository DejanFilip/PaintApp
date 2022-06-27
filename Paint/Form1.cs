using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;



namespace Paint
{
    public partial class Paint : Form
    {
        Point Start, End;
        public Paint()
        {
            InitializeComponent();

            DoubleBuffered = true;
            this.Width = 800;
            this.Height = 550;
            bm = new Bitmap(picBox.Width, picBox.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            picBox.Image = bm;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            p.Width = (float)PaintBrushSize.Value;
        }
        public event EventHandler Resize;

        Bitmap bm;
        Graphics g;
        bool painting = false;
        Point dx, dy;
        Pen p = new Pen(Color.Black, 1);
        Pen erase = new Pen(Color.White,10);
        int index;
        int x, y, sX, sY, cX, cY;

        ColorDialog cd = new ColorDialog();
        Color new_color;

        public Image OpenedFile { get; private set; }

        private void Paint_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            
            if (control.Size.Height != control.Size.Width)
            {
                control.Size = new Size(control.Size.Width, control.Size.Width);
            }

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
            Pen dashed = new Pen(new_color);
            dashed.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
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
                DrawTriangle();
            }
        }

        private void Paint_Load(object sender, EventArgs e)
        {
           
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
                Fill(bm, e.X, e.Y, new_color);

            }
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
            index = 0;
            Invalidate();
        }
        private void validate(Bitmap bm,Stack<Point>sp,int x,int y,Color old_color,Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }
        
        public void Fill(Bitmap bm,int x,int y,Color new_clr)
        {
            Color old_color = bm.GetPixel(x,y);
            Stack<Point>pixel = new Stack<Point>();
            pixel.Push(new Point(x,y));
            bm.SetPixel(x,y,new_clr);
            if (old_color == new_clr) return;

            while(pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y-1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);

                }
            }
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, picBox.Width, picBox.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
            }
        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            DialogResult dr = Op.ShowDialog();
            if (dr == DialogResult.OK)
            {
                OpenedFile = Image.FromFile(Op.FileName);
                picBox.Image = OpenedFile;
                Invalidate();
            }

        }
        private void PaintBrushSize_ValueChanged(object sender, EventArgs e)
        {
            p.Width = (float)PaintBrushSize.Value;
        }
        private void DrawTriangle()
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
