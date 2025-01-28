using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Timers;
using AngleSharp;



namespace Memorama
{
    
    public partial class Tablero : Form
    {
        Carta[] tablero = new Carta[16];
        int cartas_volteadas = 0;
        Carta[] cartas = new Carta[2];
        string[] tags = new string[2];
        System.Timers.Timer temporizador;
        public Tablero()
        {
            InitializeComponent();
        }
        private Bitmap obtenerImagen(int id)
        {
            Bitmap imagen = null;
            switch(id)
            {
                case 0: imagen = new Bitmap(Memorama.Properties.Resources.rojo); break;
                case 1: imagen = new Bitmap(Memorama.Properties.Resources.rosa); break;
                case 2: imagen = new Bitmap(Memorama.Properties.Resources.morado); break;
                case 3: imagen = new Bitmap(Memorama.Properties.Resources.blanco); break;
                case 4: imagen = new Bitmap(Memorama.Properties.Resources.azul); break;
                case 5: imagen = new Bitmap(Memorama.Properties.Resources.amarillo); break;
                case 6: imagen = new Bitmap(Memorama.Properties.Resources.naranja); break;
                case 7: imagen = new Bitmap(Memorama.Properties.Resources.negro); break;
                case 8: imagen = new Bitmap(Memorama.Properties.Resources.cover); break;
            }
            return imagen;
        } 

        private void inicializarTablero()
        {
            int pos;
            int[] posiciones = new int[16];
            Random generador = new Random();
            for(int i = 0; i < 16; i++)
            {
                bool repetido = false;
                do
                {
                    repetido = false;
                    pos = generador.Next(16);
                    for(int j = 0; j < i; j++)
                    {
                        if(pos == posiciones[j])
                        {
                            repetido = true;
                            break;
                        }
                    }
                } while (repetido);
                posiciones[i] = pos;
                int idx_imagen = (pos > 7) ? pos - 8 : pos;
                Bitmap imagen = obtenerImagen(idx_imagen);
                tablero[i] = new Carta(idx_imagen, imagen, false);
                /*
                 * pos          idx_imagen
                 *  0               0
                 *  1               1
                 *  2               2
                 *  3               3
                 *  4               4
                 *  5               5
                 *  6               6
                 *  7               7
                 *  8               0
                 *  9               1
                 *  10              2
                 *  11              3
                 *  12              4
                 *  13              5
                 *  14              6
                 *  15              7          */
            }
        }
        private void mostrarCarta(object sender, EventArgs e)
        {
            try {
                PictureBox carta = (PictureBox)sender;
                int pos = Convert.ToInt32(carta.Tag);
                if (tablero[pos].Volteada == false && cartas_volteadas <= 2)
                {
                    tags[cartas_volteadas] = carta.Tag.ToString();
                    cartas[cartas_volteadas] = tablero[pos];
                    carta.Image = tablero[pos].Imagen;
                    tablero[pos].Volteada = true;
                    cartas_volteadas++;
                    if (cartas_volteadas == 2)
                    {
                        if (cartas[0].Posicion == cartas[1].Posicion)//Par
                        {
                            cartas_volteadas = 0;
                            verificarVictoria();
                        }
                        else
                        {
                            Iniciar_Timer();
                        }

                    }
                }
                carta.Image = tablero[pos].Imagen;
            }
            catch { }
            }
        private void ocultarCarta(Object source, ElapsedEventArgs e)
        {
            Bitmap imagen = obtenerImagen(8);
            PictureBox carta1 = this.Controls.OfType<PictureBox>().FirstOrDefault(pbx => pbx.Tag.ToString() == tags[0]);
            if(carta1 != null)
            {
                carta1.Image = imagen;
            }
            
            PictureBox carta2 = this.Controls.OfType<PictureBox>().FirstOrDefault(pbx => pbx.Tag.ToString() == tags[1]);
            if (carta2 != null)
            {
                carta2.Image = imagen;
            }
            int idx1 = Convert.ToInt32(tags[0]);
            tablero[idx1].Volteada = false;
            int idx2 = Convert.ToInt32(tags[1]);
            tablero[idx2].Volteada = false;
            cartas_volteadas = 0;
            Detener_Timer();


        }
        private void Iniciar_Timer()
        {
            temporizador = new System.Timers.Timer(1000);
            temporizador.Elapsed += ocultarCarta;
            temporizador.AutoReset = false;
            temporizador.Enabled = true;
        }
        private void Detener_Timer()
        {
            temporizador.Stop();
            temporizador.Dispose();
        }
        private void verificarVictoria()
        {
            bool todasVolteadas = tablero.All(carta => carta.Volteada);
            if (todasVolteadas)
            {
                // Mostrar pantalla de victoria
                MessageBox.Show("¡Felicidades! Has ganado el juego.", "Victoria", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reiniciarJuego();
            }
        }
        private void reiniciarJuego()
        {
            foreach (var carta in tablero)
            {
                carta.Volteada = false;
            }
            inicializarTablero();
            foreach (var pictureBox in this.Controls.OfType<PictureBox>())
            {
                pictureBox.Image = obtenerImagen(8); // Restablece la imagen de cubierta
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inicializarTablero();
        }
    }
    
}
