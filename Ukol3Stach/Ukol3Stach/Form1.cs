using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ukol3Stach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        List<int> poleznamek = new List<int>();

        public void Vek(string rodnecislo, out int vek,out string mesic)
        {
            int rok = Convert.ToInt32(rodnecislo.Substring(0, 2));
            if (rok >= 0)
            {
                rok += 2000;
            }
            else { rok += 1900; }
            int mesica = Convert.ToInt32(rodnecislo.Substring(2, 2));            
            int den = Convert.ToInt32(rodnecislo.Substring(4, 2));
            if (mesica > 12) { mesica = mesica - 50; }

            DateTime veka = new DateTime(rok,mesica, den);            
            TimeSpan vysledekveku = new TimeSpan();
            vysledekveku = DateTime.Now - veka;
            
            int mujvek = vysledekveku.Days / 365;
            vek = mujvek;
            
            
            
            mesic = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(mesica);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            double soucetznamek = 0;
            int pocet = 0;
            bool konec = false;
            

            StreamReader sr = new StreamReader("rodna_cis.txt");
            StreamWriter wr = new StreamWriter("rodna_cis_opraveno.txt");

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                listBox1.Items.Add(line);
                string[] radek = line.Split(';');
                string rodnecislo = radek[2];
                int znamka = Convert.ToInt32(radek[1]);
                soucetznamek += znamka;
                pocet++;
                int mujvek;
                string mesicslovo;
                Vek(rodnecislo, out mujvek, out mesicslovo);

                int rok = Convert.ToInt32(rodnecislo.Substring(0,2));

                if ((mesicslovo == "pro") && konec == false)
                {
                    label1.Text = "Prvni clovek v prosinci: " + radek[0];
                    konec = true;
                }
                else if (konec == false)
                {
                    label1.Text = "Nikdo se nenarodil v prosinci ";
                }

                
                

                wr.WriteLine(line + ";" + mujvek.ToString());

                

            }


            soucetznamek = soucetznamek / pocet;
            wr.WriteLine("Prumer znamek: " + soucetznamek);
            sr.Close();
            wr.Close();

            StreamReader srx = new StreamReader("rodna_cis_opraveno.txt");
            while(!srx.EndOfStream) {
            
                listBox2.Items.Add(srx.ReadLine());
            
            }
            srx.Close();

            

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Textové soubory|*.txt";
            sfd.Title = "Uložit soubor";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sww = new StreamWriter(sfd.FileName);
                
                foreach(string line in listBox1.Items)
                {
                    string[] casti = line.Split(';');
                    string jmeno = casti[0];
                    int znamka = Convert.ToInt32(casti[1]);
                    string rdncslo = casti[2];

                    int vek;
                    string mesic;
                    Vek(rdncslo,out vek,out mesic);
                    if(znamka < 3 && znamka >= 1)
                    {
                        sww.WriteLine(jmeno + ";" + vek + ";" + mesic);
                        listBox3.Items.Add(jmeno + ";" + vek + ";" + mesic);
                    }
                }
                sww.Close();
            }
            
        }

        
    }

    
}
