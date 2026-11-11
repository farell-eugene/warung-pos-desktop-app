using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warung
{
    public partial class FormBarang: Form
    {
        private DbClient db;

        public FormBarang()
        {
            InitializeComponent();
            db = new DbClient();
        }

        private void FormBarang_Load(object sender, EventArgs e)
        {
            this.barangTableAdapter.Fill(this.warungDataSet.barang);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormDashboard formDashboard = new FormDashboard();
            formDashboard.Show();
            this.Hide();
        }

private void button1_Click_1(object sender, EventArgs e)
        {
            FormBarang formBarang = new FormBarang();
            formBarang.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormTransaksi formTransaksi = new FormTransaksi();
            formTransaksi.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormLaporan formLaporan = new FormLaporan();
            formLaporan.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormTambah formTambah = new FormTambah();
            formTambah.Show();
            this.Hide();
        }
    }
}
