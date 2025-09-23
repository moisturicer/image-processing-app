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
    public partial class Form2 : Form
    {
        Form1 _f1;
        Bitmap imageB, imageA, processed;
        private Device myWebcam;

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
        private void turnOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                turnOffToolStripMenuItem_Click(null, null);
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

        private void captureAsMainImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                myWebcam.Sendmessage();
                IDataObject data = Clipboard.GetDataObject();
                if (data != null)
                {
                    Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                    if(image!=null)
                    {
                        imageB = new Bitmap(image);
                        pictureBox1.Image = image;
                    }
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

        private void turnOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myWebcam != null)
            {
                myWebcam.Stop();
                pictureBox1.Image = null;
                myWebcam = null;
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
                Bitmap backgroundResized = new Bitmap(imageB.Width, imageB.Height);
                using (Graphics g = Graphics.FromImage(backgroundResized))
                {
                    g.DrawImage(imageA, 0, 0, imageB.Width, imageB.Height);
                }

                Color mygreen = Color.FromArgb(0, 0, 255);
                int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                int threshold = 5;
                processed = new Bitmap(imageB.Width, imageB.Height);
                for (int x = 0; x < imageB.Width; x++)
                {
                    for (int y = 0; y < imageB.Height; y++)
                    {
                        Color pixelColor = imageB.GetPixel(x, y);
                        Color backPixelColor = backgroundResized.GetPixel(x, y);
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
