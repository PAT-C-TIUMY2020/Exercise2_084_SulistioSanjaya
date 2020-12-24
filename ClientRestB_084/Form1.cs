using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientRestB_084
{
    public partial class Form1 : Form
    {
        ClassData classData = new ClassData();
        public Form1()
        {
            InitializeComponent();
            //TampilData();
        }
        
        public void SearchData()
        {
            var json = new WebClient().DownloadString("http://localhost:1908/Mahasiswa");
            var data = JsonConvert.DeserializeObject<List<Mahasiswa>>(json);
            string nim = textBoxNIM.Text;
            if (nim == null || nim == "")
            {
                dataGridView1.DataSource = data;
            }
            else
            {
                var item = data.Where(x => x.nim == textBoxNIM.Text).ToList();

                dataGridView1.DataSource = item;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNama.Text = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            textBoxNIM.Text = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            textBoxProdi.Text = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            textBoxAngkatan.Text = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

            buttonUpdate.Enabled = true;
        }

        private void buttonTotal_Click(object sender, EventArgs e)
        {
            var json = new WebClient().DownloadString("http://localhost:1908/Mahasiswa");
            var data = JsonConvert.DeserializeObject<List<Mahasiswa>>(json);
            int length = data.Count();
            txtJumlah.Text = Convert.ToString(length);
        }
        

        private void txtJumlah_Click(object sender, EventArgs e)
        {

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxNIM.Text != "" &&
                textBoxNama.Text != "" &&
                textBoxProdi.Text != "" &&
                textBoxAngkatan.Text != "")
            {
                if (textBoxNIM.Text.Length <= 12 &&
                textBoxAngkatan.Text.Length <= 4 &&
                textBoxProdi.Text.Length <= 30 &&
                textBoxNama.Text.Length <= 20)
                {
                    try
                    {
                        Mahasiswa mhs = new Mahasiswa();
                        mhs.nim = textBoxNIM.Text;
                        mhs.nama = textBoxNama.Text;
                        mhs.prodi = textBoxProdi.Text;
                        mhs.angkatan = textBoxAngkatan.Text;

                        ClassData classData = new ClassData();
                        classData.updateDatabase(mhs);
                        MessageBox.Show("Data successfuly updated");
                       
                        dataGridView1.DataSource = classData.getAllData();
                    }
                    catch
                    {
                        label5.Text = "Server Error";
                    }
                }
                else
                {
                    MessageBox.Show("Please check your data");
                }
            }
            else
            {
                MessageBox.Show("Please check your data");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    ClassData classData = new ClassData();
                    classData.deleteMahasiswa(textBoxNIM.Text);
                    dataGridView1.DataSource = classData.getAllData();
                    MessageBox.Show("Data successfuly deleted");
                }
                catch (Exception ex)
                {
                   label5.Text = "Server Error";
                }
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = classData.getAllData();
            }
            catch
            {
                label5.Text = "Server error";
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void clear()
        {
            textBoxNIM.Text = "";
            textBoxNama.Text = "";
            textBoxProdi.Text = "";
            textBoxAngkatan.Text = "";
        
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
