using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JuegoSnake
{
    public partial class Form1 : Form
    {

        List<PictureBox> Lista = new List<PictureBox>();
        int tamanoHead = 26, tiempo = 10;
        PictureBox Comida = new PictureBox();
        String Direccion = "right";


        public Form1()
        {
            InitializeComponent();
            IniciarJuego();
        }
        public void IniciarJuego()
        {
            PictureBox pb = new PictureBox();
            tiempo = 10;
            Direccion = "right";
            timer2.Interval = 200;
            lblPuntos.Text = "0";
            Lista = new List<PictureBox>();
            for (int i = 2; 0 <= i; i--)
            {
                CrearSnake(Lista, this, (i * tamanoHead) + 70, 80);
            }
            CrearComida();
        }

        public void CrearSnake(List<PictureBox> ListaPelota, Form formulario, int posicionx, int posiciony)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new Point(posicionx, posiciony);
            pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("cuerp");
            pb.BackColor = Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            ListaPelota.Add(pb);
            formulario.Controls.Add(pb);
        }

        private void CrearComida()
        {
            Random rnd = new Random();
            int enterox = rnd.Next(1, this.Width - tamanoHead - 10);
            int enteroy = rnd.Next(1, this.Height - tamanoHead - 40);

            PictureBox pb = new PictureBox();
            pb.Location = new Point(enterox, enteroy);
            pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("Manza");
            pb.BackColor = Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            Comida = pb;
            this.Controls.Add(pb);
        }


        private void MoverPieza(object sender, KeyEventArgs e)
        {
            Direccion = ((e.KeyCode & Keys.Up) == Keys.Up) ? "up" : Direccion;
            Direccion = ((e.KeyCode & Keys.Down) == Keys.Down) ? "down" : Direccion;
            Direccion = ((e.KeyCode & Keys.Left) == Keys.Left) ? "left" : Direccion;
            Direccion = ((e.KeyCode & Keys.Right) == Keys.Right) ? "right" : Direccion;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            int nx = Lista[0].Location.X;
            int ny = Lista[0].Location.Y;
            Lista[0].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("head" + Direccion);
            //Cambio de direccion cAbeza
            for (int i = Lista.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    if (Direccion == "right") nx = nx + tamanoHead;
                    else if (Direccion == "left") nx = nx - tamanoHead;
                    else if (Direccion == "up") ny = ny - tamanoHead;
                    else if (Direccion == "down") ny = ny + tamanoHead;
                    Lista[0].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("head" + Direccion);
                    Lista[0].Location = new Point(nx, ny);
                }
                else
                {
                    Lista[i].Location = new Point((Lista[i - 1].Location.X), (Lista[i].Location.Y));
                    Lista[i].Location = new Point(Lista[i].Location.X, Lista[i - 1].Location.Y);

                }
            }
            //Comer puntos
            for (int contarPiezas = 1; contarPiezas < Lista.Count; contarPiezas++)
            {
                if (Lista[contarPiezas].Bounds.IntersectsWith(Comida.Bounds))
                {
                    this.Controls.Remove(Comida);
                    tiempo = Convert.ToInt32(timer2.Interval);
                    if (tiempo > 10) { timer2.Interval = tiempo - 10; }
                    lblPuntos.Text = (Convert.ToInt32(lblPuntos.Text) + 1).ToString();
                    CrearComida();
                    CrearSnake(Lista, this, Lista[Lista.Count - 1].Location.X * tamanoHead, 0);

                }
            }
            //Paredes
            if ((Lista[0].Location.X >= this.Width - 15) || (Lista[0].Location.Y >= this.Height - 50) || (Lista[0].Location.Y < -10) || (Lista[0].Location.X < -30))
            {
                ReiniciarJuego();
            }
            //Chocar cuerpo
            for (int contarPiezas = 1; contarPiezas < Lista.Count; contarPiezas++)
            {
                if (Lista[0].Bounds.IntersectsWith(Lista[contarPiezas].Bounds))
                { ReiniciarJuego(); }
            }

        }

        public void ReiniciarJuego()
        {
            foreach (PictureBox Serpiente in Lista) { this.Controls.Remove(Serpiente); }
            this.Controls.Remove(Comida);
            IniciarJuego();
        }


    }
}

