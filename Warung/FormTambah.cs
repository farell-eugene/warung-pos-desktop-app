using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Warung
{
    public partial class FormTambah : Form
    {
        private DbClient db;
        private int currentBarangId = 0;

        public FormTambah()
        {
            InitializeComponent();
            db = new DbClient();
            SetDefaultState();
        }

        // Navigasi antar form
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

        // Tombol Tambah Data
        private void button6_Click(object sender, EventArgs e)
        {
            string nama = textBox1.Text.Trim();
            string kategori = textBox3.Text.Trim();
            string hargaText = textBox2.Text.Trim();
            string stokText = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(kategori) || string.IsNullOrEmpty(hargaText) || string.IsNullOrEmpty(stokText))
            {
                MessageBox.Show("Nama, Kategori, Harga, dan Stok harus diisi.");
                return;
            }

            if (!decimal.TryParse(hargaText, out decimal harga))
            {
                MessageBox.Show("Harga harus berupa angka yang valid.");
                return;
            }

            if (!int.TryParse(stokText, out int stok))
            {
                MessageBox.Show("Stok harus berupa angka bulat.");
                return;
            }

            string query = "INSERT INTO barang (nama_barang, kategori, harga, stok, id_pemilik) " +
                           "VALUES (@nama, @kategori, @harga, @stok, 1)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nama", nama),
                new SqlParameter("@kategori", kategori),
                new SqlParameter("@harga", harga),
                new SqlParameter("@stok", stok)
            };

            try
            {
                int affected = db.ExecuteNonQuery(query, parameters);
                if (affected > 0)
                {
                    MessageBox.Show("Data berhasil ditambahkan.");
                    SetDefaultState();
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menambahkan data: " + ex.Message);
            }
        }

        // Tombol Reset
        private void button10_Click(object sender, EventArgs e)
        {
            SetDefaultState();
        }

        private void SetDefaultState()
        {
            ClearInput();
            button6.Enabled = true;
            button10.Enabled = true;
        }

        private void ClearInput()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            currentBarangId = 0;
        }
    }
}
