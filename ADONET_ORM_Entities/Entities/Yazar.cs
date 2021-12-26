using ADONET_ORM_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName = "Yazarlar", IdentityColumn = "YazarID", PrimaryColumn = "YazarID")]

    public class Yazar
    {
        public int YazarID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string YazarAdSoyad { get; set; }
        public bool SilindiMi { get; set; }
        
      
      
      
    }
}
