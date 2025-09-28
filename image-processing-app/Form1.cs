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
using WebCamLib;

namespace image_processing
{
    public partial class Form1 : Form
    {
        private Device myWebcam;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap original = new Bitmap(pictureBox1.Image);
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
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap original = new Bitmap(pictureBox1.Image);
                Bitmap processed = new Bitmap(original.Width, original.Height);

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixelColor = original.GetPixel(x, y);
                        processed.SetPixel(x, y, pixelColor);
                    }
                }

                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
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
            if (pictureBox1.Image != null)
            {
                Bitmap original = new Bitmap(pictureBox1.Image);
                Bitmap processed = new Bitmap(original.Width, original.Height);

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixelColor = original.GetPixel(x, y);
                        pixelColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                        processed.SetPixel(x, y, pixelColor);
                    }
                }

                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap original = new Bitmap(pictureBox1.Image  );
                Bitmap processed = new Bitmap(original.Width, original.Height);

                int[] histogram = new int[256];
                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixelColor = original.GetPixel(x, y);
                        int grey = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                        histogram[grey]++;
                    }
                }

                using (Graphics graphics = Graphics.FromImage(processed))
                {
                    graphics.Clear(Color.White);
                    int max = histogram.Max();
                    float binWidth = processed.Width / 256f;

                    for (int i = 0; i < 256; i++)
                    {
                        int barHeight = (int)((histogram[i] / (float)max) * processed.Height);
                        graphics.FillRectangle(Brushes.Gray, i * binWidth, processed.Height - barHeight, binWidth, barHeight);
                    }
                }

                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap original = new Bitmap(pictureBox1.Image);
                Bitmap processed = new Bitmap(original.Width, original.Height);

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixelColor = original.GetPixel(x, y);
                        int r = Math.Min(255, (int)(0.393 * pixelColor.R + 0.769 * pixelColor.G + 0.189 * pixelColor.B));
                        int g = Math.Min(255, (int)(0.349 * pixelColor.R + 0.686 * pixelColor.G + 0.168 * pixelColor.B));
                        int b = Math.Min(255, (int)(0.272 * pixelColor.R + 0.534 * pixelColor.G + 0.131 * pixelColor.B));

                        processed.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }

                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string fileName = "processed_" + Guid.NewGuid().ToString("N").Substring(0,8) + ".png";
                string filePath = Path.Combine(downloadPath, fileName); 
                pictureBox2.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                MessageBox.Show("Image saved to: " + filePath);
            }
            else
            {
                MessageBox.Show("No processed image to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(this);
            f2.Show();
            this.Hide();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                offToolStripMenuItem_Click(null, null);
            }

            Device[] devices = DeviceManager.GetAllDevices();
            if (devices.Length > 0)
            {
                myWebcam = devices[0];
                myWebcam.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("No webcams found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                myWebcam.Stop();
                pictureBox1.Image = null;
                myWebcam = null;
            }
        }

        private void captureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                myWebcam.Sendmessage();
                IDataObject data = Clipboard.GetDataObject();
                if (data != null)
                {
                    Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                    pictureBox1.Image = image;
                }
                else
                {
                    MessageBox.Show("Could not capture image from webcam.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Camera is turned off. Please turn it on first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap processed = new Bitmap(pictureBox1.Image);
                Convolution.ConvMatrix m = new Convolution.ConvMatrix();
                m.SetAll(1);
                m.Pixel = 1;
                m.Factor = 1+8;
                Convolution.ConvFilters.Conv3x3(processed, m);
                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void smoothenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap processed = new Bitmap(pictureBox1.Image);
                Convolution.ConvMatrix m = new Convolution.ConvMatrix();
                m.SetAll(1);
                m.Pixel = 1;
                m.Factor = 9;
                m.Offset = 0;

                Convolution.ConvFilters.Conv3x3(processed, m);
                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap processed = new Bitmap(pictureBox1.Image);
                Convolution.ConvMatrix m = new Convolution.ConvMatrix();
                m.TopLeft = 1; m.TopMid = 2; m.TopRight = 1;
                m.MidLeft = 2; m.Pixel = 4; m.MidRight = 2;
                m.BottomLeft = 1; m.BottomMid = 2; m.BottomRight = 1;
                m.Factor = 16;
                m.Offset = 0;

                Convolution.ConvFilters.Conv3x3(processed, m);
                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap processed = new Bitmap(pictureBox1.Image);
                Convolution.ConvMatrix m = new Convolution.ConvMatrix();
                m.TopLeft = 0; m.TopMid = -2; m.TopRight = 0;
                m.MidLeft = -2; m.Pixel = 11; m.MidRight = -2;
                m.BottomLeft = 0; m.BottomMid = -2; m.BottomRight = 0;
                m.Factor = 3;
                m.Offset = 0;

                Convolution.ConvFilters.Conv3x3(processed, m);
                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void embossingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap processed = new Bitmap(pictureBox1.Image);
                Convolution.ConvMatrix m = new Convolution.ConvMatrix();
                m.TopLeft = -1; m.TopMid = 0; m.TopRight = -1;
                m.MidLeft = 0; m.Pixel = 4; m.MidRight = 0;
                m.BottomLeft = -1; m.BottomMid = 0; m.BottomRight = -1;
                m.Factor = 1;
                m.Offset = 127; 

                Convolution.ConvFilters.Conv3x3(processed, m);
                pictureBox2.Image = processed;
            }
            else
            {
                MessageBox.Show("No imported image to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
