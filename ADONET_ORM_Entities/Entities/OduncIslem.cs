using ADONET_ORM_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Entities.Entities;

namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName ="Islemler",PrimaryColumn ="IslemID",IdentityColumn ="IslemID")]
    public class OduncIslem
    {
        public int IslemID { get; set; }
        public int OgrID { get; set; }
        public int KitapID { get; set; }
        public DateTime OduncAldigiTarih { get; set; }
        public DateTime OduncBitisTarih { get; set; }
        public bool TeslimEdildiMi { get; set; }
        
        
    }
}
