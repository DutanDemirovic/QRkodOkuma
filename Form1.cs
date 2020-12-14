using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using MessagingToolkit.QRCode;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;



namespace KarekodOlusturma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Image KareKodUret(string giris, int kkDuzey)
        {
            var deger = giris;
            MessagingToolkit.QRCode.Codec.QRCodeEncoder qre = new QRCodeEncoder();

            if (radioButton1.Checked == true)
            {
                qre.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            if (radioButton2.Checked == true)
            {
                qre.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            if (radioButton3.Checked == true)
            {
                qre.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }

            qre.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            qre.QRCodeVersion = kkDuzey;
            System.Drawing.Bitmap bm = qre.Encode(deger);
            return bm;
        }
        private void btnKarekodUret_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked == true || radioButton2.Checked == true || radioButton3.Checked == true)
                {
                    pictureBox1.Image = KareKodUret(textBox1.Text, 4);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Karakter sınırını aştınız. Lütfen biraz kısaltın .!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void karekodCozumle()
        {
            try
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                textBox1.Text = decoder.decode(new QRCodeBitmapImage(pictureBox1.Image as Bitmap));
            }
            catch (MessagingToolkit.QRCode.ExceptionHandler.DecodingFailedException ex)
            {
                MessageBox.Show("Karekod çözümleniyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnKarekodCoz_Click(object sender, EventArgs e)
        {
            karekodCozumle();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

        }

        private void karekodGetirVeOkuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog()
            {
                Title = "Karekod Seçin",
                Filter = "JPG Dosyası |*.jpg| PNG Dosyası |*.png| GIF Dosyası |*.gif| Bitmap Dosyası |*.bmp",
                FilterIndex = 1,
                RestoreDirectory = true,
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };
            string DosyaYolu;
            if (file.ShowDialog()==DialogResult.OK)
            {
                DosyaYolu = file.FileName;
                pictureBox1.Image = Image.FromFile(DosyaYolu);
                karekodCozumle();
            }
        }

        private void karekodKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            try
            {
                fileName = "KK-" + textBox1.Lines[0] + ".jpg";
            }
            catch (IndexOutOfRangeException)
            {
                fileName = "KK-" + DateTime.Now.ToString() + ".jpg";                
            }
            if (pictureBox1.Image != null)
            {
                SaveFileDialog sf = new SaveFileDialog()
                {
                    Title = "Kaydet",
                    RestoreDirectory = true,
                    Filter = "JPG Dosyası |*.jpg",
                    FileName = fileName,
                };
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(sf.FileName, ImageFormat.Jpeg);
                    MessageBox.Show("Karekod Kaydedildi.","Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Oluşturulmuş bir Karekod Bulunamadı.!", "Hata.!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label3.Text = textBox1.Text.Length.ToString();
        }
    }
}
