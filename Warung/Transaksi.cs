using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warung
{
    class Transaksi
    {
        public int IdTransaksi { get; set; }
        public DateTime Tanggal { get; set; }
        public decimal TotalHarga { get; set; }
        public int IdPemilik { get; set; }
        public List<DetailTransaksi> DetailTransaksiList { get; set; } = new List<DetailTransaksi>();
    }

    public class DetailTransaksi
    {
        public int IdDetail { get; set; }
        public int IdTransaksi { get; set; }
        public int IdBarang { get; set; }
        public int Jumlah { get; set; }
        public decimal TotalHarga { get; set; }
    }

}
