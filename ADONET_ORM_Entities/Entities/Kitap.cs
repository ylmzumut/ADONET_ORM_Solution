using ADONET_ORM_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName = "Kitaplar", PrimaryColumn = "KitapID", IdentityColumn = "KitapID")]

    public class Kitap
    {
        public int KitapID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string KitapAdi { get; set; }
        public int SayfaSayisi { get; set; }
        public bool SilindiMi { get; set; }
        public int? TurID { get; set; }
        public int YazarID { get; set; }
        public int Stok { get; set; }


    }
}
