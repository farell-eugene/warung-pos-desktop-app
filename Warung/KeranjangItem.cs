using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warung
{
    public class KeranjangItem
    {
        public int IdBarang { get; set; }
        public string NamaBarang { get; set; }
        public decimal Harga { get; set; }
        public int Jumlah { get; set; }

        public decimal TotalHarga
        {
            get { return Harga * Jumlah; }
        }
    }
}
