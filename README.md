# Семинарска работа по Визуелно програмирање - Paint App
## Изработено од:
Мартин Марјановиќ 202023
<br>
Стефан Николов 203248 
## Објаснување на апликацијата
<b>Screenshot</b><br>
![paintapp](https://user-images.githubusercontent.com/100425624/176664174-f432a70d-9441-4171-b8ba-0603f1bdc8b7.png)

Апликацијата е слична на Microsoft Paint editor-ot со неколку додатни функционалности.Формата е составена од <b>pictureBox</b> и <b>panel</b>,
каде што на истиот ги има сите битни копчиња(<b>buttons</b>) на кои се имплементирани функционалностите за да работи програмата.За бирање на боја има две копчиња:<br>
<ul><li>Првото е при бирање на бојата да се појави на целото копче таа боја,<br>
<li>Второто со натпис <b>Color</b> и слика од палета каде што само може да се бира боја.<br></ul>
Копчињата за цртање се состојаат од:
<ul><li>Молив(<b>Pencil</b>),Елипса(<b>Ellipse</b>),Правоаглоник(<b>Rectangle</b>),Обична линија(<b>Line</b>) и Триаголник(<b>Triangle</b>).
        <li>За бришење на платното има копче (<b>Clear</b>) и гума за бришење(<b>Eraser</b>).<li>Копчето <b>PenSize</b> е за менување на големината на сите од овие форми,
<b>Clear</b> за бришење на се што ќе се нацрта,<b>Save</b> за зачувување,додека <b>Open</b> за отварање на зачуваното од соодветниот фолдер на комјутерот.
<li>Последното копче е за покажување на координатите при движење на маусот на pictureBox-ot.
</ul>

<h2>Опис на решавање на проблемот</h2>
1.<b>MouseMoveDown</b>,<b>MouseMoveUp</b> и <b>MouseMove</b> методите

Прво го имаме event-от <b>MouseDown</b> кој регистрира дека сме почнале со цртање и ја зема локацијата на напиот курсор во дадениот момент и X и Y оските.
Потоа со event-от  <b>MouseUp</b>  со помош на нашиот flag кажуваме дека сме завршиле со цртанје односно нашиот маус не го држиме кликнат повеќе и во зависност од индексот се исцртува конкретната форма која може да биде елипса, правоаголник, права линија и триаголник.

![image](https://user-images.githubusercontent.com/100298572/176908953-679206c9-eedf-47d2-8a75-72b62b85d8ad.png)

И event-от <b>MouseMove</b> кој цело време ја подесува локацијата на нашиот курсор во зависнот од тоа како го движиме и во десниот долен агол ни ја покажува точната локација според X и Y оските.

![image](https://user-images.githubusercontent.com/100298572/176908785-7763c311-ea5d-4d71-ba55-227277d836aa.png)

Во овој метод соодветни исртувања се , пенкалото за слободно цртанје и гумата i <b>Refresh()</b> методата која е слична на <b>Invalidate()</b> ја повикуваме директно на платното кое цртаме

![image](https://user-images.githubusercontent.com/100298572/176908686-aa7fcbb2-1173-4102-8b15-72d5ec2d7e00.png)


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
На овие 3 копчиња ни се понудени различни функции:

![image](https://user-images.githubusercontent.com/100298572/176908364-9e337d8f-9c4f-409d-bfdb-f81d8356d2ed.png)
![image](https://user-images.githubusercontent.com/100298572/176908427-43209d90-1359-457e-9180-6012dac409cb.png)


-Save ни овозможува зачувување на нашиот цртеж во локалната мажина на која цртаме и ги зачувува во jpg формат.

-Open ни овозможува истата слика која сме ја зачувале да ја отвориме на платното и да можеме да продолжиме со цртање на истата.

-Clear е она кое ни ја чисти целата површина доколку имаме потреба одново да почнеме со цртање на чисто платно.

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

При исртување на формата според индексот, овој метод ни овозможува пред да го отпуштиме левиот клик на маусот да видиме колкава е големината на формата и откако ќе
отпуштиме тогаш се исцртува.
Тоа го извршуваме Graphics g варијаблата која ја повикуваме за исртување на секоја форма

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

FloodFill методата во неговите параметри ја прима битмапата на која цртаме,X и Y оската и бојата која ни е одбрана.
Така што после во листа итерираме низ цела битмапа  или низ формата и со проверката на точката дали се наога во рамките на одредена форма или пак целата површина, 
ја бои само тогаш целата внатрешност.Истата метода ја повикуваме со соодветниот индекс на копчето и евентот <b>picBox_MouseClick</b> .Оваа функција воедно беше и најтешката во самиот проект.

![image](https://user-images.githubusercontent.com/100298572/176909156-aab37b2b-93da-4b46-ab1c-c2a662a0ebd3.png)

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
        
          private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                FloodFill(bm, e.X, e.Y, new_color);
                Invalidate();

            }
        }
```

5.<b>Triangle</b> методот

Методата <b>DrawTriangle(Graphics g)</b> беше наша идеа и предизвик бидејќи таа не постои во библиотеката <b>System.Drawing</b> па така што успеавме на наш начин
со помош на готовата <b>DrawLine()</b> функција да исцртаме 3 прави линии кои прават триаголник.Исто така после се повикува со соодветен индекс на копчето за да може
да ја исцртаме.

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

<b>Init()</b> е обична функција каде ни се поставени потребните функционалности кои треба да ги имаме на самото стартување на програмата и ја повикуваме истата
во самиот конструктор на иницијализација на формата.
 
```
  public Paint()
        {
            InitializeComponent();
            Init();
        }
 
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

<b>PickColor()</b> е функција која ја повикуваме на 2 места.Тоа е на самото копче Color
 
![image](https://user-images.githubusercontent.com/100298572/176907173-df13afa3-43b9-4209-8279-47d5648f87f9.png)
 
а додадена е и бонус опција да менуваме боја и на местото каде што ја прикажуваме тековната боја која ни е одбрана , која се наоѓа одма до копчето Color

![image](https://user-images.githubusercontent.com/100298572/176907618-e2d94e77-4ccf-49a0-972d-43f1d8f48dd7.png)

И со оваа фукнција која може да ја активираме на овие 2 места, ја добиваме палетата за бирање на боја

![image](https://user-images.githubusercontent.com/100298572/176907807-840e9d7b-0e93-4945-a3ea-61ded454a619.png)



```
 private void pic_color_Click(object sender, EventArgs e)
        {
            PickColor();
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            PickColor();
        }
 
private void PickColor()
        {
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }
```        
        
8.Event-от <b>PaintBrushSize_ValueChanged</b>
Ни овозмува на промена на вредноста на <b>numericUpAndDown</b> да ја менуваме ширината на пенкалото и сите останати форми 

![image](https://user-images.githubusercontent.com/100298572/176909746-cc18b746-4e5d-4bfc-a95d-8c48b04fdaa4.png)
 
```
private void PaintBrushSize_ValueChanged(object sender, EventArgs e)
        {
            p.Width = (float)PaintBrushSize.Value;
        }
```
