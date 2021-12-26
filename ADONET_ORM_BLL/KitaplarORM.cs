using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Common;
using ADONET_ORM_Entities.Entities;
using ADONET_ORM_Entities.ViewModels;

namespace ADONET_ORM_BLL
{
    public class KitaplarORM : ORMBase<Kitap, KitaplarORM>
    {
        TurlerORM myTurORM = new TurlerORM();
        YazarlarORM myYazarORM = new YazarlarORM();

        public List<KitapViewModel> KitaplariViewModelleGetir()
        {
            List<KitapViewModel> returnList = new List<KitapViewModel>();
            try
            {


                List<Kitap> kitaplar = this.Select();
                var yazarlar = myYazarORM.Select();
                List<Tur> turler = myTurORM.Select();

                foreach (Kitap item in kitaplar)
                {
                    KitapViewModel kitap = new KitapViewModel()
                    {
                        KitapID = item.KitapID,
                        KitapAdi = item.KitapAdi,
                        KayitTarihi = item.KayitTarihi,
                        SayfaSayisi = item.SayfaSayisi,
                        SilindiMi = item.SilindiMi,
                        Stok = item.Stok,
                        YazarID = item.YazarID
                    };
                    kitap.YazarAdSoyad = yazarlar.Find(x => x.YazarID == item.YazarID)?.YazarAdSoyad;
                    kitap.TurID = turler.Find(x => x.TurID == item.TurID)?.TurID;
                    kitap.TurAdi = item.TurID == null ? "Türü Yok" : turler.Find(x => x.TurID == item.TurID).TurAdi;

                    returnList.Add(kitap);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return returnList;
        }


    }
}
