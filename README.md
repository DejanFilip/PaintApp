# Семинарска работа по Визуелно програмирање - Paint App
## Изработено од:
Мартин Марјановиќ 202023
<br>
Стефан Николов 203248 
## Објаснување на апликацијата
<b>Screenshot</b><br>
![paintapp](https://user-images.githubusercontent.com/100425624/176664174-f432a70d-9441-4171-b8ba-0603f1bdc8b7.png)

Апликацијата е слична на Microsoft Paint editor-ot со неколку додатни функционалности.Формата е составена од <b>pictureBox</b> и <b>panel</b>,
каде што на истиот ги има сите битни копчиња(<b>buttons</b>) на кој се имплементирани функциалностите за да работи програмата.За бирање на боја има две копчиња:<br>
<ul><li>Првото е при бирање на бојата да се појави на целото копче таа боја,<br>
<li>Второто со натпис <b>Color</b> и слика од палета каде што само може да се бира боја.<br></ul>
Копчињата за цртање се состојаат од:
<ul><li>Молив(<b>Pencil</b>),Елипса(<b>Ellipse</b>),Правоаглоник(<b>Rectangle</b>),Обична линија(<b>Line</b>) и Триаголник(<b>Triangle</b>).
<li>За бришење на сите овие форми има копче гума за бришење(<b>Eraser</b>).<li>Копчето <b>PenSize</b> е за менување на големината на сите од овие форми,
<b>Clear</b> за бришење на се што ќе се нацрта,<b>Save</b> за зачувување,додека <b>Open</b> за отварање на зачуваното од соодветниот фолдер на комјутерот.
<li>Последното копче е за покажување на координатите при движење на маусот на pictureBox-ot.
</ul>
<h2>Опис на решавање на проблемот</h2>
 1.<b>MouseMoveDown</b>,<b>MouseMoveUp</b> и <b>MouseMove</b> методите
Прво го имаме event-от **MouseDown** кој регистрира дека сме почнале со цртанје и ја зема локацијата на напиот курсор во дадениот момент и X и Y оските.
Потоа со event-от  **MouseUp**  со помош на нашиот flag кажуваме дека сме завршиле со цртанје односно нашиот маус не го држиме кликнат повеќе и во зависност од индексот се исцртува конкретната форма која може да биде елипса, правоаголник, права линија и триаголник.
И event-от **MouseMove** кој цело време ја подесува локацијата на нашиот курсор во зависнот од тоа како го движиме и во десниот долен агол ни ја покажува точната локација според X и Y оските.
Во овој метод соодветни исртувања се , пенкалото за слободно цртанје и гумата i **Refresh()** методата која е слична на **Invalidate()** ја повикуваме директно на платното
кое цртаме

```
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            painting = true;
            dy = e.Location;
            cX = e.X;
            cY = e.Y;
            if (e.Button == MouseButtons.Left)
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
            if (index == 8)
            {
                DrawTriangle(g);
            }
        }
        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            lbCordinates.Text = "Cordinates" + "\n" + "X:" + e.X + "\n" + "Y:" + e.Y;

            if (painting)
            {
                if (index == 1)
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
```

2.<b>Open</b>,<b>Save</b> и <b>Clear</b> методите
```
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
 private void btn_clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            picBox.Image = bm;
            picBox.SizeMode = PictureBoxSizeMode.Normal;
            index = 0;
            Invalidate();
        }
```
3.<b>Paint</b> методот
```
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
```        
        
4.<b>Fill</b> методот кој е и воедно најсложениот код во програмата
```
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
```
5.<b>Triangle</b> методот
```
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
```
6.<b>Init()</b> методот
```
private void Init()
        {
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            bm = new Bitmap(width, height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            picBox.Image = bm;
            p.Width = (float)PaintBrushSize.Value;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            erase.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }
```        
7.Методот за бирање боја <b>PickColor()</b> 
```
private void PickColor()
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }
```        
        
    
        














