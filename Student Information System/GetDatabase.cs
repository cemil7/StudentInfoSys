using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Student_Information_System
{
    public partial class GetDatabase
    {
        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;
        public void BilgileriGetir()
        {
            baglanti = new SqlConnection("Server=localhost=SQLEXPRESS;Database=master;Trusted_Connection=True;");
            baglanti.Open();
            da = new SqlDataAdapter("Select *From tblKisiler", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            baglanti.Close();
        }
    }
}
