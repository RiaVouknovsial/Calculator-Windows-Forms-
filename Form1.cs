using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WinFormsApp05_Calc_Dynamic_Component_Creation
{
    public partial class Form1 : Form
    {
        private const int bw = 50, bh = 50; // размер кнопки
        private const int dx = 10, dy = 10; // расстояние между кнопками   

        // новый Label
        Label label1 = new Label();

        // массив кнопок
        private Button[] btn = new Button[15];

        // текст на кнопках
        char[] btnText = {'7','8','9','+',
                          '4','5','6','-',
                          '1','2','3','=',
                          '0',',','c'};

        // кнопку будем идентифицировать по значению свойства Tag
        int[] btnTag = {7,8,9,-3,
                        4,5,6,-4,
                        1,2,3,-2,
                        0,-1,-5};

        private double ac = 0;  // аккумулятор
        private int op = 0;     // код операции

        private Boolean fd;  // fd == true - ждем первую цифру числа  например,
                             // после нажатия кнопки
                             // fd == false - ждем следующую  цифру или
                             // нажатие кнопки операции;

        public Form1()
        {
            InitializeComponent();

            fd = true;

            // создать кнопки
            int x, y;    // координаты кнопки

            // Устанавливаем цвет рамки и отступы
            this.Padding = new Padding(10, 10, 10, 3);
            this.BackColor = Color.FromArgb(95, 158, 160);

            // установить размер клиентской области формы
            this.ClientSize = new Size(4 * bw + 5 * dx, 5 * bh + 7 * dy);

            // задать размер и положение индикатора
            this.Controls.Add(label1);
            label1.TextAlign = ContentAlignment.BottomRight;
            label1.AutoSize = false;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.BackColor = SystemColors.ButtonFace;
            label1.SetBounds(dx, dy, 4 * bw + 3 * dx, bh);
            label1.Text = "0";
            y = label1.Bottom + dy;

            // создать кнопки
            int k = 0; // номер кнопки
            for (int i = 0; i < 4; i++)
            {
                x = dx;
                for (int j = 0; j < 4; j++)
                {
                    if (!((i == 3) && (j == 0)))
                    {
                        // создать и настроить кнопку
                        btn[k] = new Button();
                        btn[k].SetBounds(x, y, bw, bh);
                        btn[k].Tag = btnTag[k];
                        btn[k].Text = btnText[k].ToString();

                        // Установите черный цвет текста кнопки
                        btn[k].ForeColor = Color.Black;

                        // Установите цвет фона между кнопками
                        btn[k].BackColor = Color.WhiteSmoke;

                        // задать функцию обработки
                        // события Click
                        this.btn[k].Click += new EventHandler(this.Button_Click);

                        if (btnTag[k] < 0)
                        {
                            // кнопка операции
                            btn[k].BackColor = SystemColors.ControlLight;
                        }

                        // поместить кнопку на форму
                        this.Controls.Add(this.btn[k]);

                        x = x + bw + dx;
                        k++;
                    }
                    else // кнопка ноль
                    {
                        // создать и настроить кнопку
                        btn[k] = new Button();
                        btn[k].SetBounds(x, y, bw * 2 + dx, bh);
                        btn[k].Tag = btnTag[k];
                        btn[k].Text = btnText[k].ToString();

                        // Установите черный цвет текста кнопки
                        btn[k].ForeColor = Color.Black;

                        // Установите цвет фона кнопки
                        btn[k].BackColor = Color.White;

                        // задать функцию обработки  события Click
                        this.btn[k].Click += new System.EventHandler(this.Button_Click);

                        // поместить кнопку на форму
                        this.Controls.Add(this.btn[k]);

                        x = x + 2 * bw + 2 * dx;
                        k++;
                        j++;

                        if (j == 0)
                        {
                            // создать и настроить кнопку
                            btn[k] = new Button();
                            btn[k].SetBounds(x, y, bw * 2 + dx, bh);
                            btn[k].Tag = -6; // Change the tag value to -6 for the "C" button
                            btn[k].Text = btnText[k].ToString();

                            // Установите черный цвет текста кнопки
                            btn[k].ForeColor = Color.Black;

                            // Установите цвет фона кнопки
                            btn[k].BackColor = Color.White;

                            // задать функцию обработки  события Click
                            this.btn[k].Click += new System.EventHandler(this.Button_Click);

                            // поместить кнопку на форму
                            this.Controls.Add(this.btn[k]);

                            x = x + 2 * bw + 2 * dx;
                            k++;
                            j++;
                        }
                    }
                }
                y = y + bh + dy;
                this.KeyPreview = true;
             
            }

        }

        // щелчок на кнопке
        private void Button_Click(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;

            if (Convert.ToInt32(btn.Tag) > 0)
            {
                if (fd)
                {
                    label1.Text = btn.Text;
                    fd = false;
                }
                else
                {
                    label1.Text += btn.Text;
                }
                return;
            }

            if (Convert.ToInt32(btn.Tag) == 0)
            {
                if (fd)
                {
                    label1.Text = btn.Text;
                    fd = false;
                }
                else if (label1.Text != "0") // Check if the indicator text is not "0"
                {
                    label1.Text += btn.Text;
                }
                return;
            }

            if (Convert.ToInt32(btn.Tag) == -1)
            {
                if (fd)
                {
                    label1.Text = "0,";
                    fd = false;
                }
                else if (label1.Text.IndexOf(",") == -1)
                {
                    label1.Text += btn.Text;
                }
                return;
            }

            if (Convert.ToInt32(btn.Tag) == -5)
            {
                ac = 0;
                op = 0;
                label1.Text = "0";

                fd = true;
                return;
            }

            if (Convert.ToInt32(btn.Tag) < -1)
            {
                double n;
                n = Convert.ToDouble(label1.Text);

                if (op != 0)
                {
                    switch (op)
                    {
                        case -3: ac += n; break;
                        case -4: ac -= n; break;
                        case -2: ac *= n; break;
                    }
                    label1.Text = ac.ToString("N");
                }
                else
                {
                    ac = n;
                }
                op = Convert.ToInt32(btn.Tag);
                fd = true;
            }
            else if (Convert.ToInt32(btn.Tag) == -7) // "C" button
            {
                ac = 0;
                op = 0;
                label1.Text = "0";
                fd = true;
            }
        }
    }
}

