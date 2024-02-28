using System;
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
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells["readAccess"].Value)
                {
                    //seçili olanlara 1 ekle
                    label1.Text += " " + row.Index.ToString();
                    label2.Text += 1.ToString();
                }
                else if ((bool)row.Cells["writeAccess"].Value)
                {
                    label1.Text += " " + row.Index.ToString();
                    label2.Text += 1.ToString();
                }
                else if ((bool)row.Cells["writeAccess"].Value && (bool)row.Cells["readAccess"].Value)
                {
                    //her iki sütunda seçim yapıldıysa
                    label7.Show();
                }
                else
                {
                    //seçili olmayanlara 0 ekle
                    label2.Text += 0.ToString();
                }
            }
            //sayıların ters yazımı
            string cevir = "";
            for (int a = 0; a < label2.Text.Length; a++)
            {
                cevir = label2.Text.Substring(a, 1) + cevir;
            }
            label2.Text = cevir;
        //ikilikten onluk çevirme
        tekrar:
            string say = label2.Text;
            foreach (char basamak in say)
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
            label6.Text = onluk.ToString();
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
            label9.Text = yazikalan.ToString();
            textBox2.Text = yazikalan.ToString();
            //    basamak.Reverse();
            foreach (int isim in basamak)
            {
                int i;
                don:
                    yer = textBox2.Text.ToUpper().IndexOf("1".ToUpper(), yer + 1);
                    
                    if (yer < 0)
                    {
                        goto bitir;
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
                        goto don;                                   
                    }
                }
            
        bitir:
            MessageBox.Show("Bitti.");
            
        }        
    }
}
