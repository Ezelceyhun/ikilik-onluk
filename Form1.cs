﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace json_dosya_okuma_ve_2li_sistemden_10luk_sayıya_cevirme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void LoadJson()
        {

        }
        public class Item
        {
            public int form_id;
            public string form_adi;
            public int yazma_yetkisi;
            public int okuma_yetkisi;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label7.Hide();
            //debug klasörüne istenilen dosya eklenir
            string path = Application.StartupPath.ToString() + "\\Kitap1.json";
            List<Item> items = new List<Item>();

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            foreach (var item in items)
            {
                dataGridView1.Rows.Add(item.form_id, item.form_adi, false, false);

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label12.Text = "";
            label13.Text = "";
            label6.Text = "";
            label2.Text = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells["readAccess"].Value)
                {
                    //seçili olanlara 1 ekle
                    label1.Text += " " + row.Index.ToString();
                    label12.Text += 1.ToString();
                }
                else if ((bool)row.Cells["writeAccess"].Value)
                {
                    label1.Text += " " + row.Index.ToString();
                    label13.Text += 1.ToString();
                }
                else if ((bool)row.Cells["writeAccess"].Value && (bool)row.Cells["readAccess"].Value)
                {
                    //her iki sütunda seçim yapıldıysa
                    label13.Text += 1.ToString();
                    label12.Text += 1.ToString();
                }
                else
                {
                    //seçili olmayanlara 0 ekle
                    label12.Text += 0.ToString();
                    label13.Text += 0.ToString();
                }
            }
            //sayıların ters yazımı
            string cevir = "";
            for (int a = 0; a < label12.Text.Length; a++)
            {
                cevir = label12.Text.Substring(a, 1) + cevir;
            }
            label12.Text = cevir;
            string cevir2 = "";
            for (int a = 0; a < label13.Text.Length; a++)
            {
                cevir2 = label13.Text.Substring(a, 1) + cevir2;
            }
            label13.Text = cevir2;
        //ikilikten onluk çevirme
        tekrar:
            string say = label12.Text;
            foreach (char basamak in say)
            {
                if ((basamak != '0') && (basamak != '1'))
                {
                    goto tekrar;
                }
            }
            string say2 = label13.Text;
            foreach (char basamak in say2)
            {
                if ((basamak != '0') && (basamak != '1'))
                {
                    goto tekrar;
                }
            }
            double onluk = 0;
            int sira = 0;
            for (int i = say.Length - 1; i >= 0; i--)
            {
                onluk += (Math.Pow(2, sira) * Convert.ToInt32(say.Substring(i, 1)));
                sira++;
            }
            double onluk2 = 0;
            int sira2 = 0;
            for (int i = say2.Length - 1; i >= 0; i--)
            {
                onluk2 += (Math.Pow(2, sira2) * Convert.ToInt32(say2.Substring(i, 1)));
                sira2++;
            }
            label6.Text = onluk.ToString();
            label2.Text = onluk2.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //temizle butonu
            label1.Text = "";
            label2.Text = "";
            label6.Text = "";
            //for ile satırları dolaşıp checked false çevirmesi

            
            for(int i =0; i<=28;i++)
            {
                DataGridViewCheckBoxCell never = dataGridView1.Rows[i].Cells[2] as DataGridViewCheckBoxCell;
                DataGridViewCheckBoxCell once = dataGridView1.Rows[i].Cells[3] as DataGridViewCheckBoxCell;
                bool isNeverChecked = (bool)never.EditedFormattedValue;
                bool isOnceChecked = (bool)once.EditedFormattedValue;
                if (isNeverChecked || isOnceChecked)
                {
                    once.Value = "false";
                    never.Value = "false";
                }
            }
            

            //tabloyu yenile
            dataGridView1.Refresh();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        int yer = -1;
        private void button3_Click(object sender, EventArgs e)
        {
            //sayı decimal şeklinde girilicek binary dönüştürülüp kontrol edilicek
            //eğer dönüştürülen sayı 1 ise eş değerdeki satırın checkbox ı check olucak
            List<int> basamak = new List<int>();
            int kalan = 0;
            string yazikalan = "";
            int sayi = Convert.ToInt32(textBox1.Text);
            while (sayi != 0)
            {
                kalan = sayi % 2;
                sayi = sayi / 2;
                yazikalan = kalan.ToString() + yazikalan;
                basamak.Add(kalan);
            }
            textBox2.Text = yazikalan.ToString();
            label9.Text = yazikalan.ToString();
            //sayı ters çevir
            string cevir = "";
            string cevir2 = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            for (int a = 0; a < label9.Text.Length; a++)
            {
                cevir2 = label9.Text.Substring(a, 1) + cevir2;
            }
            textBox2.Text = cevir.PadLeft(32, '0');
            label9.Text = cevir2.PadLeft(32, '0');
            cevir = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            string yeni_cevir = cevir;
            foreach (int isim in basamak)
            {             
                    yer = yeni_cevir.ToUpper().IndexOf("1".ToUpper(), yer + 1);
                    
                    if (yer < 0)
                    {
                    MessageBox.Show("Bitti.");
                    break;
                }
                    else
                    {
                        textBox2.SelectionStart = yer;
                        textBox2.SelectionLength = "1".Length;
                        DataGridViewCheckBoxCell never = dataGridView1.Rows[yer].Cells[2] as DataGridViewCheckBoxCell;
                        DataGridViewCheckBoxCell once = dataGridView1.Rows[yer].Cells[3] as DataGridViewCheckBoxCell;
                        bool isOnceChecked = (bool)once.EditedFormattedValue;
                        bool isNeverChecked = (bool)never.EditedFormattedValue;
                        once.Value = "true";
                        never.Value = "true";
                        MessageBox.Show((yer+1).ToString() + ". Kutu Seçildi !");                                   
                    }
                }                  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> basamak = new List<int>();
            int kalan = 0;
            string yazikalan = "";
            int sayi = Convert.ToInt32(textBox1.Text);
            while (sayi != 0)
            {
                kalan = sayi % 2;
                sayi = sayi / 2;
                yazikalan = kalan.ToString() + yazikalan;
                basamak.Add(kalan);
            }
            textBox2.Text = yazikalan.ToString();
            label9.Text = yazikalan.ToString();
            //sayı ters çevir
            string cevir = "";
            string cevir2 = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            for (int a = 0; a < label9.Text.Length; a++)
            {
                cevir2 = label9.Text.Substring(a, 1) + cevir2;
            }
            textBox2.Text = cevir.PadLeft(32, '0');
            label9.Text = cevir2.PadLeft(32, '0');
            cevir = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            string yeni_cevir = cevir;
            foreach (int isim in basamak)
            {
                yer = yeni_cevir.ToUpper().IndexOf("1".ToUpper(), yer + 1);

                if (yer < 0)
                {
                    MessageBox.Show("Bitti.");
                    break;
                }
                else
                {
                    textBox2.SelectionStart = yer;
                    textBox2.SelectionLength = "1".Length;
                    DataGridViewCheckBoxCell never = dataGridView1.Rows[yer].Cells[2] as DataGridViewCheckBoxCell;
                    bool isNeverChecked = (bool)never.EditedFormattedValue;
                    never.Value = "true";
                    MessageBox.Show((yer + 1).ToString() + ". Kutu Seçildi !");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> basamak = new List<int>();
            int kalan = 0;
            string yazikalan = "";
            int sayi = Convert.ToInt32(textBox1.Text);
            while (sayi != 0)
            {
                kalan = sayi % 2;
                sayi = sayi / 2;
                yazikalan = kalan.ToString() + yazikalan;
                basamak.Add(kalan);
            }
            textBox2.Text = yazikalan.ToString();
            label9.Text = yazikalan.ToString();
            //sayı ters çevir
            string cevir = "";
            string cevir2 = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            for (int a = 0; a < label9.Text.Length; a++)
            {
                cevir2 = label9.Text.Substring(a, 1) + cevir2;
            }
            textBox2.Text = cevir.PadLeft(32, '0');
            label9.Text = cevir2.PadLeft(32, '0');
            cevir = "";
            for (int a = 0; a < textBox2.Text.Length; a++)
            {
                cevir = textBox2.Text.Substring(a, 1) + cevir;
            }
            string yeni_cevir = cevir;
            foreach (int isim in basamak)
            {
                yer = yeni_cevir.ToUpper().IndexOf("1".ToUpper(), yer + 1);

                if (yer < 0)
                {
                    MessageBox.Show("Bitti.");
                    break;
                }
                else
                {
                    textBox2.SelectionStart = yer;
                    textBox2.SelectionLength = "1".Length;
                    DataGridViewCheckBoxCell once = dataGridView1.Rows[yer].Cells[3] as DataGridViewCheckBoxCell;
                    bool isOnceChecked = (bool)once.EditedFormattedValue;
                    once.Value = "true";
                    MessageBox.Show((yer + 1).ToString() + ". Kutu Seçildi !");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label12.Text = "";
            label13.Text = "";
            label6.Text = "";
            label2.Text = "";
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
