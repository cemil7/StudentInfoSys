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
        SqlConnection connection;
        SqlCommand cmd;
        SqlDataAdapter da;
        public void getInfos()
        {
            connection = new SqlConnection("Server=localhost=SQLEXPRESS;Database=master;Trusted_Connection=True;");
            connection.Open();
            da = new SqlDataAdapter("Select *From tblStudents", connection);
            DataTable table = new DataTable();
            da.Fill(table);
            connection.Close();
        }
    }
}
