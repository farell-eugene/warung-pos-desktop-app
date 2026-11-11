using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warung
{
    public partial class FormTransaksi : Form
    {
        private DbClient db;
        private DataTable barangTable;
        private List<KeranjangItem> keranjang;
        private int idPemilik = 1; 

        public FormTransaksi()
        {
            InitializeComponent();
            db = new DbClient();
            keranjang = new List<KeranjangItem>();
            LoadBarang();
            LoadMetodePembayaran();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void LoadBarang()
        {
            barangTable = db.ExecuteSelect("SELECT id_barang, nama_barang, harga, stok FROM barang");
            comboBox1.DisplayMember = "nama_barang";
            comboBox1.ValueMember = "id_barang";
            comboBox1.DataSource = barangTable;
        }

        private void LoadMetodePembayaran()
        {
            comboBox2.Items.Add("Cash");
            comboBox2.Items.Add("QRIS");
            comboBox2.SelectedIndex = 0;
        }

        private void RefreshKeranjang()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nama");
            dt.Columns.Add("Harga");
            dt.Columns.Add("Jumlah");
            dt.Columns.Add("Total");

            decimal total = 0;
            foreach (var item in keranjang)
            {
                dt.Rows.Add(item.IdBarang, item.NamaBarang, item.Harga, item.Jumlah, item.TotalHarga);
                total += item.TotalHarga;
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["ID"].Visible = false;
            label8.Text = total.ToString("C");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                DataRowView row = (DataRowView)comboBox1.SelectedItem;
                label6.Text = row["harga"].ToString();
            }
        }

        private void label6_Click(object sender, EventArgs e) { }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) return;

            DataRowView selectedBarang = (DataRowView)comboBox1.SelectedItem;
            int id = (int)selectedBarang["id_barang"];
            string nama = selectedBarang["nama_barang"].ToString();
            decimal harga = Convert.ToDecimal(selectedBarang["harga"]);
            int stok = Convert.ToInt32(selectedBarang["stok"]);

            if (!int.TryParse(textBox1.Text.Trim(), out int jumlah) || jumlah <= 0 || jumlah > stok)
            {
                MessageBox.Show("Jumlah tidak valid atau melebihi stok.");
                return;
            }

            keranjang.Add(new KeranjangItem { IdBarang = id, NamaBarang = nama, Harga = harga, Jumlah = jumlah });
            RefreshKeranjang();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            keranjang.Clear();
            RefreshKeranjang();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang kosong.");
                return;
            }

            foreach (var item in keranjang)
            {
                string cekStok = "SELECT stok FROM barang WHERE id_barang = @id_barang";
                object stokObj = db.ExecuteScalar(cekStok, new SqlParameter[] { new SqlParameter("@id_barang", item.IdBarang) });
                int stokTersisa = Convert.ToInt32(stokObj);

                if (stokTersisa < item.Jumlah)
                {
                    MessageBox.Show($"Stok barang '{item.NamaBarang}' tidak cukup. Sisa: {stokTersisa}");
                    return;
                }
            }

            decimal totalHarga = keranjang.Sum(k => k.TotalHarga);
            DateTime tanggal = dateTimePicker1.Value;

            string insertTransaksi = "INSERT INTO transaksi (tanggal, total_harga, id_pemilik) VALUES (@tanggal, @total, @pemilik); SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters =
            {
                new SqlParameter("@tanggal", tanggal),
                new SqlParameter("@total", totalHarga),
                new SqlParameter("@pemilik", idPemilik)
            };

            object result = db.ExecuteScalar(insertTransaksi, parameters);
            int idTransaksi = Convert.ToInt32(result);

            foreach (var item in keranjang)
            {
                string insertDetail = "INSERT INTO detail_transaksi (id_transaksi, id_barang, jumlah, total_harga) VALUES (@id_transaksi, @id_barang, @jumlah, @total_harga)";
                SqlParameter[] detailParams =
                {
                    new SqlParameter("@id_transaksi", idTransaksi),
                    new SqlParameter("@id_barang", item.IdBarang),
                    new SqlParameter("@jumlah", item.Jumlah),
                    new SqlParameter("@total_harga", item.TotalHarga) // total harga item = harga satuan * jumlah
                };
                db.ExecuteNonQuery(insertDetail, detailParams);


                string updateStok = "UPDATE barang SET stok = stok - @jumlah WHERE id_barang = @id_barang";
                SqlParameter[] updateParams =
                {
                    new SqlParameter("@jumlah", item.Jumlah),
                    new SqlParameter("@id_barang", item.IdBarang)
                };
                db.ExecuteNonQuery(updateStok, updateParams);
            }

            MessageBox.Show("Transaksi berhasil disimpan.");
            keranjang.Clear();
            RefreshKeranjang();
            LoadBarang();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
