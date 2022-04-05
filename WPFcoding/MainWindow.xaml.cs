using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.IO;

namespace WPFcoding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Code_Click(object sender, RoutedEventArgs e)
        {
            int length = 0;
            int rez;
            StringBuilder sb = new StringBuilder();
            rez = passwEncode.Text.Length;
            string word = passwEncode.Text;
            while (length < 32)
            {
                for(int i = 0; i < rez; i++)
                {
                    if (length != 32)
                    {
                        sb.Append(word[i]);
                        length++;
                    }
                }
            }
            string key = sb.ToString();
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(textEncode.Text);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            Encode.Text = Convert.ToBase64String(array);
            textDecode.Text = Encode.Text;
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            int length = 0;
            int rez;
            StringBuilder sb = new StringBuilder();
            rez = passwDecode.Text.Length;
            string word = passwDecode.Text;
            while (length < 32)
            {
                for (int i = 0; i < rez; i++)
                {
                    if (length != 32)
                    {
                        sb.Append(word[i]);
                        length++;
                    }
                }
            }
            string key = sb.ToString();
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(textDecode.Text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            Decode.Text = streamReader.ReadToEnd();

                        }
                    }
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            passwEncode.Text = "";
            passwDecode.Text = "";
            textEncode.Text = "";
            textDecode.Text = "";
            Encode.Text = "";
            Decode.Text = "";
        }
    }
}
