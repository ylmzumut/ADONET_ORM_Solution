using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Common;

namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName ="Turler",IdentityColumn ="TurID",PrimaryColumn ="TurID")]
    public class Tur
    {
        public int TurID { get; set; }
        public string TurAdi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

    }
}
