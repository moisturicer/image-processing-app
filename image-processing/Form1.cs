using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_processing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(openFileDialog1.FileName);
            Bitmap processed = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    int grey = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    pixelColor = Color.FromArgb(grey, grey, grey);  
                    processed.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox2.Image = processed;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(openFileDialog1.FileName);
            Bitmap processed = new Bitmap(original.Width, original.Height);

            for(int y=0; y<original.Height; y++)
            {
                for(int x=0; x<original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    processed.SetPixel(x, y, pixelColor); 
                }
            }

            pictureBox2.Image = processed;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select an Image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*";
            
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = image;
            }
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(openFileDialog1.FileName);
            Bitmap processed = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    pixelColor = Color.FromArgb(255-pixelColor.R, 255-pixelColor.G, 255-pixelColor.B);
                    processed.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(openFileDialog1.FileName);
            Bitmap processed = new Bitmap(original.Width, original.Height);

            int[] histogram = new int[256];
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    int grey = (int)(pixelColor.R*0.299 + pixelColor.G*0.587 + pixelColor.B*0.114);
                    histogram[grey]++;
                }
            }

            using (Graphics graphics = Graphics.FromImage(processed))
            {
                graphics.Clear(Color.White);
                int max = histogram.Max();
                float binWidth = processed.Width / 256f;

                for(int i=0; i<256; i++)
                {
                    int barHeight = (int)((histogram[i]/(float)max)*processed.Height);
                    graphics.FillRectangle(Brushes.Gray, i*binWidth, processed.Height - barHeight, binWidth, barHeight);
                }
            }

            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(openFileDialog1.FileName);
            Bitmap processed = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    int r = Math.Min(255, (int)(0.393*pixelColor.R + 0.769*pixelColor.G + 0.189*pixelColor.B));
                    int g = Math.Min(255, (int)(0.349*pixelColor.R + 0.686*pixelColor.G + 0.168*pixelColor.B));
                    int b = Math.Min(255, (int)(0.272*pixelColor.R + 0.534*pixelColor.G + 0.131*pixelColor.B));
                    processed.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            pictureBox2.Image = processed;
        }
    }
}
