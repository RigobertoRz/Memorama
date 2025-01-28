using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Memorama
{
    class Carta
    {
        bool volteada;// Si la cartea volteada 
        Bitmap imagen;
        int pos;

        public Carta(int pos, Bitmap imagen, bool volteada)
        {
            Posicion = pos;
            Imagen = imagen;
            Volteada = volteada;
        }

        public bool Volteada { get => volteada; set => volteada = value; }
        public Bitmap Imagen { get => imagen; set => imagen = value; }
        public int Posicion { get => pos; set => pos = value; }
    }
}
