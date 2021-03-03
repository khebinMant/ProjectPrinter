﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Utilidades;
using System.Threading;

namespace ProjectPrinter
{
    class LogicaHeptagono
    {
        const int CENTRADOX = 105;
        const int CENTRADOY = 50;
        private float mLado;
        private float nuevoLado;
        private float mAp;
        private float mLadoa;
        private float mLadob;
        private float mLadoc;
        private float mLadod;
        private float mLadoe;
        private float mLadof;
        private float mLadog;
        private float mLadoh;
        private float mArea;
        private float mPerimetro;
        private float Angulo1;
        private float Angulo2;
        private float Angulo3;
        private float Angulo4;
        private float Angulo5;

        private PointF mA, mB, mC, mD, mE, mF, mG;

        private Graphics mGraficosX;
        private Graphics mGraficosY;
        private Graphics mGraficosZ;

        private Pen mPen;
        private const float SF = 10;
        private int velocidad;

        public LogicaHeptagono()
        {
            mArea = 0.0f;
            mPerimetro = 0.0f;
            mLado = 0.0f;
        }
        public void InicializarDatos(TextBox lado, TextBox perimetro, TextBox area, PictureBox heptagono)
        {
            mArea = 0.0f;
            mPerimetro = 0.0f;
            mLado = 0.0f;
            lado.Text = "";
            perimetro.Text = "";
            area.Text = "";
            heptagono.Refresh();
        }
        public void Leerdatos(TextBox lado)
        {
            try
            {
                mLado = float.Parse(lado.Text);
                if (mLado >= 4)
                {
                    mLado = 3;
                }
                if(mLado == 2)
                {
                    mLado = 3;
                }
            }
            catch
            {
                MessageBox.Show("Error en el Ingreso de datos");
            }

        }
        public void LadoyAngulos()
        {
            //Primero los angulos
            Angulo1 = 51.42f * (float)Math.PI / 180.0f;
            Angulo2 = 38.54f * (float)Math.PI / 180.0f;
            Angulo3 = 64.28f * (float)Math.PI / 180.0f;
            Angulo4 = 38.57f * (float)Math.PI / 180.0f;
            Angulo5 = 65.28f * (float)Math.PI / 180.0f;

            //Calculo de la Apotema
            mAp = (mLado * (float)Math.Tan(Angulo3)) / 2;

            //Los lados usando los angulos
            mLadoa = mLado * (float)Math.Cos(Angulo1);
            mLadob = mLado * (float)Math.Sin(Angulo1);
            mLadof = (mLado / (2 * (float)Math.Cos(Angulo3)));
            mLadoc = ((mAp + mLadof) - mLadob);
            mLadod = mLadoa + (mLado / 2);
            mLadoe = mLadof * (float)Math.Cos(Angulo2);
            mLadog = mLadod - mLadoe;
            mLadoh = mLadof * (float)Math.Sin(Angulo4);

        }
        public void AreayPerimetro()
        {
            mPerimetro = mLado * 7.0f;
            mArea = (mPerimetro * mAp) / 2;
        }
        public void DeterminarPuntos()
        {
            mA.X = (mLadoa * SF)+ CENTRADOX;
            mA.Y = (0.0f * SF)+ CENTRADOY;

            mB.X = ((mLadoa + mLado) * SF)+ CENTRADOX;
            mB.Y = (0.0f * SF)+ CENTRADOY;

            mC.X = (((2 * mLadoa) + mLado) * SF)+ CENTRADOX;
            mC.Y = (mLadob * SF)+CENTRADOY;

            mD.X = ((mLadod + mLadoe) * SF)+ CENTRADOX;
            mD.Y = ((mLadoh + mAp) * SF)+ CENTRADOY;

            mE.X = ((mLadoa + (mLado / 2)) * SF)+ CENTRADOX;
            mE.Y = ((mAp + mLadof) * SF)+ CENTRADOY;

            mF.X = ((mLadog) * SF)+ CENTRADOX;
            mF.Y = ((mLadoh + mAp) * SF)+ CENTRADOY;

            mG.X = (0.0f * SF)+ CENTRADOX;
            mG.Y = (mLadob * SF)+ CENTRADOY;
        }
        public void limpiar(PictureBox heptagono)
        {
            heptagono.Refresh();
        }
        public void CreadoraRelleno(PictureBox[] pictureBoxes, ComboBox color, ListBox[] listas, ComboBox velocidad)
        {
            DeterminarPuntos();
            Thread.CurrentThread.IsBackground = true;
            this.velocidad = DeterminarVelocidad(velocidad);
            Thread graficoZR = new Thread(new ThreadStart(() => GraficadoraRellenoZ(pictureBoxes, color, listas)));
            graficoZR.Start();
            graficoZR.Join();
        }
        public int DeterminarVelocidad(ComboBox velocidad)
        {
            if (velocidad.SelectedItem == "Lento")
            {
                return 30;
            }
            if (velocidad.SelectedItem == "Normal")
            {
                return 20;
            }
            if (velocidad.SelectedItem == "Rápido")
            {
                return 10;
            }
            if (velocidad.SelectedItem == "Muy Rápido")
            {
                return 1;
            }
            return 0;
        }
        public void GraficadoraRellenoZ(PictureBox[] pictureBoxes, ComboBox color, ListBox[] listas)
        {

            int rango = ((int)mLado * (int)SF);
            int veri = 0;
            mGraficosZ = pictureBoxes[0].CreateGraphics();
            mGraficosX = pictureBoxes[1].CreateGraphics();
            mGraficosY = pictureBoxes[2].CreateGraphics();
            PointF[] puntosEntreLineas;
            DeterminarPuntos();
            //Perspectiva Z por la izquierda
            PointF[] puntosAG;
            PointF[] puntosGF;
            PointF[] puntosFE;
            PointF[] puntosIzquierda;
            puntosAG = Bresenham.ObtenerPuntos(mA, mG, rango);
            puntosGF = Bresenham.ObtenerPuntos(mG, mF, rango);
            Recorrer(puntosGF, (int)rango / 2);
            puntosFE = Bresenham.ObtenerPuntos(mF, mE, rango);
            RecorrerPos(puntosFE, (int)rango / 2);

            puntosIzquierda = (puntosAG.Concat(puntosGF).ToArray()).Concat(puntosFE).ToArray();

            //Perspectiva Z por la derecha
            PointF[] puntosBC;
            PointF[] puntosCD;
            PointF[] puntosDE;
            PointF[] puntosDerecha;
            puntosAG = Bresenham.ObtenerPuntos(mB, mC, rango);
            puntosCD = Bresenham.ObtenerPuntos(mC, mD, rango);
            Recorrer(puntosCD, (int)rango / 2);

            puntosDE = Bresenham.ObtenerPuntos(mD, mE, rango);
            RecorrerPos(puntosDE, (int)rango / 2);

            puntosDerecha = (puntosAG.Concat(puntosCD).ToArray()).Concat(puntosDE).ToArray();


            veri = puntosDerecha.Length - 1;

            do
            {
                mPen = SeleccionarColor(color);
                puntosEntreLineas = Bresenham.ObtenerPuntos(puntosIzquierda[veri], puntosDerecha[veri], puntosDerecha.Length);
                for (int k = puntosDerecha.Length - 1, j = 0; k > 0; k--, j++)
                {
                    Thread.Sleep(this.velocidad);
                    //Z
                    mGraficosZ.DrawLine(mPen, puntosIzquierda[k], puntosDerecha[k]);
                    listas[0].Items.Add(puntosIzquierda[k].X.ToString()); listas[1].Items.Add(puntosIzquierda[k].Y.ToString());
                    listas[2].Items.Add(puntosDerecha[k].X.ToString()); listas[3].Items.Add(puntosDerecha[k].Y.ToString());
                    //Y
                    Point pixel = new Point();
                    pixel.X = (int)puntosEntreLineas[k].X;
                    pixel.Y = (int)puntosEntreLineas[k].Y;
                    listas[4].Items.Add(puntosIzquierda[k].X.ToString()); listas[5].Items.Add(puntosIzquierda[k].Y.ToString());
                    listas[6].Items.Add(puntosDerecha[k].X.ToString()); listas[7].Items.Add(puntosDerecha[k].Y.ToString());
                    listas[12].Items.Add(pixel.X + "," + pixel.Y);
                    Rectangle rect = new Rectangle(pixel, new Size(1, 1));
                    mGraficosY.DrawRectangle(mPen, rect);
                }
                mGraficosX.DrawLine(mPen, puntosIzquierda[veri], puntosDerecha[veri]);
                listas[8].Items.Add(puntosIzquierda[veri].X.ToString()); listas[9].Items.Add(puntosDerecha[veri].Y.ToString());
                listas[10].Items.Add(puntosDerecha[veri].X.ToString()); listas[11].Items.Add(puntosDerecha[veri].Y.ToString());
                veri--;
            } while (veri != 0);

        }
        public Pen SeleccionarColor(ComboBox color)
        {
            if (color.SelectedItem == "Random")
            {
                var random = new Random();
                int aleatorio = random.Next(1, 5);

                if (aleatorio == 1)
                    return new Pen(Color.Blue, 3);
                if (aleatorio == 2)
                    return new Pen(Color.Red, 3);
                if (aleatorio == 3)
                    return new Pen(Color.Yellow, 3);
                if (aleatorio == 4)
                    return new Pen(Color.Green, 3);
                if (aleatorio == 5)
                    return new Pen(Color.Brown, 3);
                return new Pen(Color.Black, 3);
            }

            else
            {
                if (color.SelectedItem == "Azul")
                    return new Pen(Color.Blue, 3);
                if (color.SelectedItem == "Rojo")
                    return new Pen(Color.Red, 3);
                if (color.SelectedItem == "Amarillo")
                    return new Pen(Color.Yellow, 3);
                if (color.SelectedItem == "Verde")
                    return new Pen(Color.Green, 3);
                if (color.SelectedItem == "Café")
                    return new Pen(Color.Brown, 3);
                return new Pen(Color.Black, 3);
            }
        }
        public void Recorrer(PointF[] Puntos, int rango)
        {
            rango = rango - (int)nuevoLado - 8;
            for (int i = 0; i < Puntos.Length; i++)
            {
                Puntos[i].Y = Puntos[i].Y - rango;
            }
        }
        public void RecorrerPos(PointF[] Puntos, int rango)
        {
            rango = rango - (int)nuevoLado - 5;
            for (int i = 0; i < Puntos.Length; i++)
            {
                Puntos[i].Y = Puntos[i].Y + rango;
            }
        }
    }
}
