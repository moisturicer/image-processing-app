using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_processing
{
    public partial class Form2 : Form
    {
        Form1 _f1;
        Bitmap imageB, imageA, processed;
        public Form2(Form1 f1)
        {
            InitializeComponent();
            _f1 = f1;
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select an Image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageB = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = imageB;
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _f1.Show();
            this.Close();
        }

        private void exportSubtractedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null)
            {
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string fileName = "processed_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ".png";
                string filePath = Path.Combine(downloadPath, fileName);
                pictureBox3.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                MessageBox.Show("Image saved to: " + filePath);
            }
            else
            {
                MessageBox.Show("No processed image to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select an Image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageA = new Bitmap(openFileDialog1.FileName);
                pictureBox2.Image = imageA;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && pictureBox2.Image != null)
            {
                Color mygreen = Color.FromArgb(0, 0, 255);
                int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                int threshold = 5;
                processed = new Bitmap(imageB.Width, imageB.Height);
                for (int x = 0; x < imageB.Width; x++)
                {
                    for (int y = 0; y < imageB.Height; y++)
                    {
                        Color pixelColor = imageB.GetPixel(x, y);
                        Color backPixelColor = imageA.GetPixel(x, y);
                        int grey = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                        int difference = Math.Abs(grey - greygreen);
                        if (difference > threshold)
                        {
                            processed.SetPixel(x, y, pixelColor);
                        }
                        else
                        {
                            processed.SetPixel(x, y, backPixelColor);
                        }
                    }
                }

                pictureBox3.Image = processed;
            }
            else
            {
                MessageBox.Show("No images to subtract.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
