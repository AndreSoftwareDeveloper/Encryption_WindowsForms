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
using System.Security.Cryptography;
namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label3.Text = openFileDialog1.FileName;
                var fileStream = openFileDialog1.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    textBox1.Text = reader.ReadToEnd();
                }
            }
        }
        string IV = "MQWJyOu+waPnA3fqkIpgfQ==";        

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textBox2.Text);
                label4.Text = saveFileDialog1.FileName;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            int encryptingKey = 3;
            string text = textBox1.Text;
            textBox2.Text = EncryptCeasar(text, encryptingKey);
        }
        static string EncryptCeasar(string text, int encryptingKey)
        {
            string result = "";

            foreach (char c in text)
            {
                char encryptedChar = c;
                if (Char.IsLetter(c))
                {
                    encryptedChar = (char) (( (c - 'a' + encryptingKey) % 26) + 'a');
                }
                result += encryptedChar;
            }

            return result;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            int encryptingKey = 3;
            string text = textBox1.Text;
            textBox2.Text = DecryptCeasar(text, encryptingKey);
        }

        static string DecryptCeasar(string text, int key)
        {
            string result = "";
            foreach (char c in text)
            {
                char decryptedChar = c;
                if (Char.IsLetter(c))
                {
                    decryptedChar = (char)(((c - 'a' - key + 26) % 26) + 'a');
                }
                result += decryptedChar;
            }
            return result;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            byte[] plainTextBytes = Encoding.Unicode.GetBytes(textBox1.Text);
            byte[] ivBytes = Convert.FromBase64String(IV);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.IV = ivBytes;
                aes.GenerateKey();
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] cipherTextBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        cipherTextBytes = ms.ToArray();
                    }
                    textBox2.Text = Convert.ToBase64String(cipherTextBytes);
                    textBox4.Text = Convert.ToBase64String(aes.Key);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(textBox2.Text);
            byte[] keyBytes = Convert.FromBase64String(textBox5.Text);
            byte[] ivBytes = Convert.FromBase64String(IV);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] plainTextBytes;
                    using (MemoryStream ms = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cs.Read(plainTextBytes, 0, plainTextBytes.Length);
                            Array.Resize(ref plainTextBytes, decryptedByteCount);
                        }
                    }
                    textBox2.Text = Encoding.Unicode.GetString(plainTextBytes);
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textBox2.Text);
                label4.Text = saveFileDialog1.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}