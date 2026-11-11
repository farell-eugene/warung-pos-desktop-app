using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Warung
{
    public partial class FormDashboard : Form
    {
        private DbClient db;

        public FormDashboard()
        {
            InitializeComponent();
            db = new DbClient();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            try
            {
                string queryStok = "SELECT SUM(stok) FROM barang";
                object stokObj = db.ExecuteScalar(queryStok);
                int totalStok = stokObj != DBNull.Value ? Convert.ToInt32(stokObj) : 0;
                label7.Text = $"{totalStok}";

                string queryTransaksi = "SELECT COUNT(*) FROM transaksi";
                object transaksiObj = db.ExecuteScalar(queryTransaksi);
                int totalTransaksi = transaksiObj != DBNull.Value ? Convert.ToInt32(transaksiObj) : 0;
                label6.Text = $"{totalTransaksi}";

                string queryPemasukan = "SELECT SUM(total_harga) FROM transaksi";
                object pemasukanObj = db.ExecuteScalar(queryPemasukan);
                decimal totalPemasukan = pemasukanObj != DBNull.Value ? Convert.ToDecimal(pemasukanObj) : 0;
                label8.Text = $"RP{totalPemasukan:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data dashboard: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            FormDashboard formDashboard = new FormDashboard();
            formDashboard.Show();
            this.Hide();
        }
    }
}
