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
    public partial class FormLogin: Form
    {
        private DbClient db;
        public FormLogin()
        {
            InitializeComponent();
            db = new DbClient();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (email == "" || password == "")
            {
                MessageBox.Show("Email dan password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT * FROM pemilik_warung WHERE email = @email AND password = @password";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)
            };

            DataTable result = db.ExecuteSelect(query, parameters);

            if (result.Rows.Count > 0)
            {
                MessageBox.Show("Login berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormDashboard dashboard = new FormDashboard();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email atau password salah!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
