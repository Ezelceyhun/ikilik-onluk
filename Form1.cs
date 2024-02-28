using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

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
                dataGridView1.Rows.Add(item.form_id, item.form_adi, false,false) ;

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if((bool)row.Cells["readAccess"].Value)
                {
                    //MessageBox.Show(row.Index.ToString());
                    //seçili olanlara 1 ekle
                    label1.Text += " "+ row.Index.ToString();
                    label2.Text += 1.ToString();
                }
                else
                {
                    //seçili olmayanlara 0 ekle
                    label2.Text += 0.ToString();
                }      
            }
        //sayıların ters yazımı
        string cevir="";
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
        }
    }
}
