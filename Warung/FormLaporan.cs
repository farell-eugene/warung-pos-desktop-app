using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Warung
{
    public partial class FormLaporan : Form
    {
        private DbClient db;
        private ReportDocument report;

        public FormLaporan()
        {
            InitializeComponent();
            db = new DbClient();
            report = new ReportDocument();
            dateTimePicker1.Value = DateTime.Today.AddDays(-7);
            dateTimePicker2.Value = DateTime.Today;
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

        private void button6_Click(object sender, EventArgs e) // Tampilkan laporan
        {
            DateTime start = dateTimePicker1.Value.Date;
            DateTime end = dateTimePicker2.Value.Date;

            string query = @"
                SELECT t.id_transaksi, t.tanggal, b.nama_barang, dt.jumlah, dt.total_harga
                FROM transaksi t
                JOIN detail_transaksi dt ON t.id_transaksi = dt.id_transaksi
                JOIN barang b ON b.id_barang = dt.id_barang
                WHERE t.tanggal BETWEEN @start AND @end";

            DataTable dt = db.ExecuteSelect(query, new SqlParameter[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@end", end)
            });

            if (dt.Rows.Count > 0)
            {
                try
                {
                    string reportPath = System.IO.Path.Combine(Application.StartupPath, "LaporanTransaksi.rpt");
                    report.Load(reportPath);
                    report.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = report;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menampilkan laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tidak ada data transaksi dalam rentang tanggal tersebut.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                crystalReportViewer1.ReportSource = null;
                crystalReportViewer1.Refresh();
            }
        }

        private void button7_Click(object sender, EventArgs e) // Export ke Excel
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xls",
                FileName = "LaporanTransaksi.xls"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    report.ExportToDisk(ExportFormatType.Excel, sfd.FileName);
                    MessageBox.Show("Laporan berhasil diekspor ke Excel.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengekspor laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e) // Export ke PDF
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                FileName = "LaporanTransaksi.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    report.ExportToDisk(ExportFormatType.PortableDocFormat, sfd.FileName);
                    MessageBox.Show("Laporan berhasil diekspor ke PDF.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengekspor laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e) { }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void FormLaporan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }
    }
}
