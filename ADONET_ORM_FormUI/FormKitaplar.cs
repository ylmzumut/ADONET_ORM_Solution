using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADONET_ORM_BLL;
using ADONET_ORM_Entities.Entities;
using ADONET_ORM_Entities;
using ADONET_ORM_Entities.ViewModels;

namespace ADONET_ORM_FormUI
{
    public partial class FormKitaplar : Form
    {
        public FormKitaplar()
        {
            InitializeComponent();
        }
        //global alan
        YazarlarORM myYazarORM = new YazarlarORM();
        TurlerORM myTurORM = new TurlerORM();
        KitaplarORM myKitapORM = new KitaplarORM();

        private void FormKitaplar_Load(object sender, EventArgs e)
        {
            TumYazarlariComboyaGetir();
            TumTurleriCombayaGetir();
            TumKitaplariGrideViewModelleGetir();
            TumKitaplariSilComboyaGetir();
            TumKitaplariGuncelleComboyaGetir();

            //comboBox'ların içine yazı yazılmasın diye comboBox'ların style'larını düzenleyeceğiz.
            //1.yöntem: Tek tek comboların name'leriden ilgili property'yi düzenlemektedir.
            comboBoxKitapGuncelle.DropDownStyle = ComboBoxStyle.DropDownList;
            //2.yöntem: foreach döngüsüyle form controlleri taranarak combo bulursa bulduğu nesnenin ilgi propery'sini düzenlemektir.

            //Uzun yöntem
            //foreach (var item in this.Controls)
            //{
            //    if (item is TabControl)
            //    {
            //        foreach (var subitem in ((TabControl)item).Controls)
            //        {
            //            if (subitem is TabPage)
            //            {
            //                foreach (var subofsubitem in ((TabPage)subitem).Controls)
            //                {
            //                    //if (subofsubitem is ComboBox)
            //                    //{
            //                    //    ((ComboBox)subofsubitem).DropDownStyle = ComboBoxStyle.DropDownList;
            //                    //}
            //                    //Eğer yukarıdaki gibi "is" kullanmak istemezseniz GetType metodu ile gelen Type'ı kontrol edebilirsiniz.
            //                    if (subofsubitem.GetType()==typeof(ComboBox))
            //                    {
            //                        ((ComboBox)subofsubitem).DropDownStyle = ComboBoxStyle.DropDownList;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //Tabcontrol ---> TabPage
            for (int i = 0; i < this.Controls[0].Controls.Count; i++)
            {
                for (int k = 0; k < this.Controls[0].Controls[i].Controls.Count; k++)
                {
                    if (this.Controls[0].Controls[i].Controls[k] is ComboBox)
                    {
                        ((ComboBox)this.Controls[0].Controls[i].Controls[k]).DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
            }
        }

        private void TumKitaplariGuncelleComboyaGetir()
        {
            comboBoxKitapGuncelle.DisplayMember = "KitapAdi";
            comboBoxKitapGuncelle.ValueMember = "KitapID";
            comboBoxKitapGuncelle.DataSource = myKitapORM.Select();
        }

        private void TumKitaplariSilComboyaGetir()
        {
            cmbBox_Sil_Kitap.DisplayMember = "KitapAdi";
            cmbBox_Sil_Kitap.ValueMember = "KitapID";

            // cmbBox_Sil_Kitap.DataSource = myKitapORM.Select();

            //Yukarıdaki gibi yapmak istemezsek yani
            // KitaplarORM class'ından instance almak istemezsek 
            // class içine tanımladığımız static property aracılığıyla o instance'a ulaşmış oluruz
            // aslında burada kendimize arka planda instance oluşturuyoruz ve static nesne aracılığıyla o nesneyi kullanıyoruz.

            cmbBox_Sil_Kitap.DataSource = KitaplarORM.Current.Select();
        }

        private void TumKitaplariGrideViewModelleGetir()
        {
            dataGridViewKitaplar.DataSource = myKitapORM.KitaplariViewModelleGetir();

            dataGridViewKitaplar.Columns["SilindiMi"].Visible = false;
            dataGridViewKitaplar.Columns["TurID"].Visible = false;
            dataGridViewKitaplar.Columns["YazarID"].Visible = false;
            for (int i = 0; i < dataGridViewKitaplar.Columns.Count; i++)
            {
                dataGridViewKitaplar.Columns[i].Width = 120;
            }
        }

        private void TumTurleriCombayaGetir()
        {
            cmbBox_Ekle_Tur.DisplayMember = "TurAdi";
            cmbBox_Ekle_Tur.ValueMember = "TurID";
            cmbBox_Ekle_Tur.DataSource = myTurORM.TurleriGetir();
            cmbBox_Ekle_Tur.SelectedIndex = 0;

            cmbBox_Guncelle_Tur.DisplayMember = "TurAdi";
            cmbBox_Guncelle_Tur.ValueMember = "TurID";
            cmbBox_Guncelle_Tur.DataSource = myTurORM.TurleriGetir();
        }

        private void TumYazarlariComboyaGetir()
        {
            cmbBox_Ekle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Ekle_Yazar.ValueMember = "YazarID";
            cmbBox_Ekle_Yazar.DataSource = myYazarORM.Select();

            cmbBox_Guncelle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Guncelle_Yazar.ValueMember = "YazarID";
            cmbBox_Guncelle_Yazar.DataSource = myYazarORM.Select();
        }

        private void btnKitapEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDown_Ekle_Stok.Value <= 0)
                {
                    MessageBox.Show("HATA: Kitap stoğu sıfırdan büyük olmalıdır!");
                    return;
                }
                if (numericUpDown_Ekle_SayfaSayisi.Value <= 0)
                {
                    MessageBox.Show("HATA: Sayfa sayısı sıfırdan büyük olmalıdır!");
                    return;
                }
                if ((int)cmbBox_Ekle_Yazar.SelectedValue <= 0)
                {
                    MessageBox.Show("HATA: Kitabın bir yazarı olmalıdır! Yazar seçiniz!");
                    return;
                }
                Kitap yeniKitap = new Kitap()
                {
                    KayitTarihi = DateTime.Now,
                    KitapAdi = txtKitapEkle.Text.Trim(),
                    SayfaSayisi = (int)numericUpDown_Ekle_SayfaSayisi.Value,
                    Stok = (int)numericUpDown_Ekle_Stok.Value,
                    SilindiMi = false,
                    YazarID = (int)cmbBox_Ekle_Yazar.SelectedValue
                };

                //TurID null mı değil mi?
                if ((int)cmbBox_Ekle_Tur.SelectedValue == -1)//türü yok'u seçmiş
                {
                    yeniKitap.TurID = null;
                }
                else
                {
                    yeniKitap.TurID = (int)cmbBox_Ekle_Tur.SelectedValue;
                }
                if (myKitapORM.Insert(yeniKitap))
                {
                    MessageBox.Show($"{yeniKitap.KitapAdi} kitabı sisteme eklendi...");
                    TumKitaplariGrideViewModelleGetir();
                    //temizlik
                    EkleSayfasiKontrolleriTemizle();
                    //combox güncelle ve combox sil içine burdan yeni kitaplar da gelmelidir.

                    TumKitaplariGuncelleComboyaGetir();
                    TumKitaplariSilComboyaGetir();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: " + ex.Message);
            }
        }

        private void EkleSayfasiKontrolleriTemizle()
        {
            txtKitapEkle.Clear();
            cmbBox_Ekle_Yazar.SelectedIndex = -1;
            cmbBox_Ekle_Tur.SelectedIndex = -1;
            numericUpDown_Ekle_SayfaSayisi.Value = 0;
            numericUpDown_Ekle_Stok.Value = 0;
        }

        private void cmbBox_Ekle_Yazar_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  MessageBox.Show(cmbBox_Ekle_Yazar.SelectedValue.ToString());
        }

        private void btnKitapSil_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)cmbBox_Sil_Kitap.SelectedValue <= 0)
                {
                    MessageBox.Show("Lütfen kitap seçimi yapınız!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                Kitap kitabim = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);

                DialogResult cevap = MessageBox.Show(
                    $"Bu kitabı silmek yerine paşifleştirmek ister misiniz? \n" +
                    $"Paşifleştirmek için  ---> EVET'e tıklayınız. \n" +
                    $"Silmek için  \t---> HAYIR'a tıklayınız. \n" +
                    $"İptal için   \t---> İPTAL'e tıklayınız.", "SİLME ONAY", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes)
                {
                    //pasifleştirme işlemi update ile yapılmalıdır.
                    kitabim.SilindiMi = true;
                    switch (myKitapORM.Update(kitabim))
                    {
                        case true:
                            MessageBox.Show($"{kitabim.KitapAdi} sistemde pasifleştrildi!");
                            //temizlik
                            SilmeSayfasiKontrolleriTemizle();
                            TumKitaplariSilComboyaGetir();
                            break;

                        case false:
                            throw new Exception($"HATA: {kitabim.KitapAdi} pasifleştirme işleminde beklenmedik bir hata oldu!");
                    }



                }
                else if (cevap == DialogResult.No)
                {
                    //silmek
                    //linq yazdık
                    var oduncListe = OduncIslemlerORM.Current.Select().Where(x => x.KitapID == kitabim.KitapID).ToList();
                    if (oduncListe.Count > 0)
                    {
                        MessageBox.Show("DİKKAT: Bu kitap ödünç alınmıştır. Silemezsiniz... Silmek isterseniz ödünç işlemler formuna gidip oradan ödünç alma işlem kaydınızı silmeniz gerekmektedir...", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //Yukarıdaki if'e girmezse return olmaz.
                    //Return olmazsa kod aşağıya doğru okunmaya devam eder.

                    if (myKitapORM.Delete(kitabim))
                    {
                        MessageBox.Show($"{kitabim.KitapAdi} adlı kitap silinmiştir.");
                        SilmeSayfasiKontrolleriTemizle();
                        TumKitaplariSilComboyaGetir();
                        // kitap silindikten sonra diğer listelerde yani gridde yani combolarda yenilenmelidir.
                        TumKitaplariGuncelleComboyaGetir();
                        TumKitaplariGrideViewModelleGetir();
                    }
                    else
                    {
                        throw new Exception($"HATA: {kitabim.KitapAdi} silinememiştir!");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: Silme işleminde neklenmedik hata oldu!" + ex.ToString());
            }
        }

        private void SilmeSayfasiKontrolleriTemizle()
        {
            cmbBox_Sil_Kitap.SelectedIndex = -1;
            richTextBoxKitap.Clear();
        }

        private void comboBoxKitapGuncelle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GuncelleSayfasiTemizle();
                if (comboBoxKitapGuncelle.SelectedIndex >= 0)
                {
                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    txt_GuncelleKitapAdi.Text = secilenKitap.KitapAdi;
                    numericUpDown_Guncelle_SayfaSayisi.Value = secilenKitap.SayfaSayisi;
                    numericUpDown_Guncelle_Stok.Value = secilenKitap.Stok;
                    //eğer TurID null ise
                    if (secilenKitap.TurID == null)
                    {
                        //cmbBox_Guncelle_Tur.SelectedIndex = 0;
                        //cmbBox_Guncelle_Tur.SelectedValue = -1;

                        // programda belirli değerler varsa örneğin Tür yok -1 value'ya sahip bir Tur. Böyle durumlarda sabit olan o değerin başka yerde de kullanılması gerekebilir ya da daha sonra o değer değişebilir diye, değeri elle yazmak yerine static bir nesneyle taşıyorlar
                        // Örneğin, Tür Yok -1 değil artık -3 olması gerekliyse
                        // Biz sadece onu ProgramBilgileri class'ında güncelleriz. Böylece onu kullanan ne kadar yer varsa otomatik güncellenmiş olur.
                        cmbBox_Guncelle_Tur.SelectedIndex = Sabitler.TurYokSelectedValue;
                    }
                    else
                    {
                        cmbBox_Guncelle_Tur.SelectedValue = secilenKitap.TurID;

                    }
                    cmbBox_Guncelle_Yazar.SelectedValue = secilenKitap.YazarID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA:" + ex.Message);
            }
        }

        private void GuncelleSayfasiTemizle()
        {
            txt_GuncelleKitapAdi.Text = string.Empty;
            numericUpDown_Guncelle_SayfaSayisi.Value = 0;
            numericUpDown_Guncelle_Stok.Value = 0;
            cmbBox_Guncelle_Tur.SelectedIndex = -1;
            cmbBox_Guncelle_Yazar.SelectedIndex = -1;
        }

        private void btnKitapGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxKitapGuncelle.SelectedIndex>0)
                {
                    if (numericUpDown_Guncelle_SayfaSayisi.Value<=0)
                    {
                        throw new Exception("HATA: Sayfa sayısı sıfırdan büyük olmalıdır!");
                    }
                    if (numericUpDown_Guncelle_Stok.Value <= 0)
                    {
                        throw new Exception("HATA: Kitap stoğu sıfırdan büyük olmalıdır!");
                    }
                    if (cmbBox_Guncelle_Tur.SelectedIndex<0)
                    {
                        throw new Exception("HATA: Lütfen türle ilgili seçim yapınız!");

                    }
                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    if (secilenKitap==null)
                    {
                        //1.yol
                        throw new Exception("HATA: Kitap bulunamadı!");
                        //2.yol
                        //MessageBox.Show("HATA: Kitap bulunamadı!");
                        //return;
                    }
                    //null değilse zaten buraya düşecek
                    secilenKitap.KitapAdi = txt_GuncelleKitapAdi.Text.Trim();
                    secilenKitap.SayfaSayisi = (int)numericUpDown_Guncelle_SayfaSayisi.Value;
                    secilenKitap.Stok = (int)numericUpDown_Guncelle_Stok.Value;
                    secilenKitap.SilindiMi = false;
                    secilenKitap.YazarID = (int)cmbBox_Guncelle_Yazar.SelectedValue;
                    if ((int)cmbBox_Guncelle_Tur.SelectedValue==-1)//türü yok
                    {
                        secilenKitap.TurID = null;
                    }
                    else
                    {
                        secilenKitap.TurID = (int)cmbBox_Guncelle_Tur.SelectedValue;
                    }
                    switch (myKitapORM.Update(secilenKitap))
                    {
                        case true:
                            MessageBox.Show($"{secilenKitap.KitapAdi} başarıyla güncellendi!");
                            //temizlik
                            TumKitaplariGuncelleComboyaGetir();
                            TumKitaplariGrideViewModelleGetir();
                            TumKitaplariSilComboyaGetir();
                            break;
                        
                        case false:
                            throw new Exception($"HATA: {secilenKitap.KitapAdi} güncellenirken beklenmedik bir hata oluştu!");
                        
                      
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: "+ex.Message);
            }
        }

        private void cmbBox_Sil_Kitap_SelectedIndexChanged(object sender, EventArgs e)
        {
            //richtextbox dolsun
            //1. yöntem

            if (cmbBox_Sil_Kitap.SelectedIndex>=0)
            {
                Kitap secilenKitap = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);
                if (secilenKitap!=null)
                {
                    richTextBoxKitap.Clear();
                    //1. yol
                    richTextBoxKitap.Text = $"Kitap: {secilenKitap.KitapAdi}\n" +
                        $"Tür: {(secilenKitap.TurID == null ? "Türü yok" : myTurORM.Select().FirstOrDefault(x => x.TurID == secilenKitap.TurID).TurAdi)}" +
                        $"Yazar: {myYazarORM.Select().FirstOrDefault(x=>x.YazarID==secilenKitap.YazarID).YazarAdSoyad}\n" +
                        $"Sayfa Sayısı: {secilenKitap.SayfaSayisi}\n" +
                        $"Stok: {secilenKitap.Stok} adet stokta var.\n";

                    // Yukarıdaki turu değişkenini ve $ 'ı kullanmak istemezseniz aşağıdaki gibi yazarız
                }
            }

            // 2. yöntem
            if (cmbBox_Sil_Kitap.SelectedIndex >= 0)
            {
                KitapViewModel seciliKitap = myKitapORM.KitaplariViewModelleGetir().FirstOrDefault(x => x.KitapID == (int)cmbBox_Sil_Kitap.SelectedValue);
                if (seciliKitap != null)
                {
                    richTextBoxKitap.Clear();
                    richTextBoxKitap.Text = "Kitap: " + seciliKitap.KitapAdi
                        + "\nTür:" + seciliKitap.TurAdi
                        + "\nYazar: " + seciliKitap.YazarAdSoyad
                        + "\nSayfa Sayısı:" + seciliKitap.SayfaSayisi
                        + "\nStok: " + seciliKitap.Stok + " adet bulunmaktadır";
                }
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            //tablar değiştikçe temizlik yapılsın.
            EkleSayfasiKontrolleriTemizle();
            comboBoxKitapGuncelle.SelectedIndex = -1;
            GuncelleSayfasiTemizle();
            SilmeSayfasiKontrolleriTemizle();

           
        }
    }
}
