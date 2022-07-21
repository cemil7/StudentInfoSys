using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Student_Information_System
{
    internal class Utilities
    {
        string connString = "Server=localhost;Data Source=CEMIL\\SQLEXPRESS;Initial Catalog=StudentInformationSystem;Integrated Security=True;";
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public List<Student> students = new List<Student>();
        int islem;
        public void ekle()
        {

            Student student = new Student();

            student.name = readlineCem(1);
            student.surname = readlineCem(2);
            student.gender = readlineCem(4);
            veriEkle(student);
            islemKontrol();
        }
        public void sil()
        {
            Student student = new Student();
            int removeId=-1;
            string IdString = readlineCem(5);
            int controledInt = idKontrol(IdString);
            
           
            if (controledInt > -1)
            {
                removeId = controledInt;
            }
            int studentIndex = countOgrenci(removeId);
             
            if (studentIndex > 0)
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM STUDENT_INFO_TABLE WHERE STUDENT_ID=" + removeId;
                cmd.ExecuteNonQuery();
                conn.Close();
                listele();
            }
            else
            {
                Console.WriteLine("Hatalı Öğrenci Numarası Girdiniz..\n");
            }
            islemKontrol();

        }
        public int cikis()
        {

            return -1;
        }
        public void listele()
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * FROM STUDENT_INFO_TABLE";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Sisteme Kayıtlı Öğrenci Bilgileri..");
                while (reader.Read())
                {
                    
                    Console.WriteLine("---------------------------------------");
                   
                    Console.Write("Student Id: ");     Console.WriteLine(reader["Student_Id"].ToString());
                    Console.Write("Student Name: ");   Console.WriteLine(reader["Student_Name"].ToString());
                    Console.Write("Student Surname: ");Console.WriteLine(reader["Student_Surname"].ToString());
                    Console.Write("Student Gender: "); Console.WriteLine(reader["Student_Gender"].ToString());
                }
                
            }
            conn.Close();
            Console.WriteLine();
            ogrenciSayisi();
            islemKontrol();

        }   
       public void veriEkle(Student student)
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO STUDENT_INFO_TABLE( Student_Name, Student_Surname, Student_Gender) VALUES ('"+student.name+"','"+student.surname+"','"+student.gender+"')"; 
            cmd.ExecuteNonQuery();
            conn.Close();
            listele();
            islemKontrol();

        }
        public void islemKontrol()
        {
            

            string isIslemNull = readlineCem(8);



            if (isIslemNull != "")
            {
                bool tryParse = Int32.TryParse(isIslemNull, out islem);
                if (tryParse)
                {

                }
                else
                {
                    Console.WriteLine("Yanlış format. Tekrar Deneyin");
                }


            }
            else
            {
                islem = 0;
            }



            if (islem == 1)
            {
                listele();

            }
            else if (islem == 2)
            {
                ekle();
            }
            else if (islem == 3)
            {
                sil();
            }
            else if (islem == 4)
            {
                ogrenciGuncelleme();
            }
            else if (islem == 5)
            {
                cikis();
            }


            else
            {
                Console.Write("Hatalı İşlem Kodunu Girdiniz...\n\n");
                islemKontrol();
            }
        }
        public void ogrenciSayisi()
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT COUNT (*) AS [STUDENT_ID] FROM STUDENT_INFO_TABLE";
            var cikti=cmd.ExecuteScalar();
            Console.WriteLine("Sistemde Kayıtlı Ogrenci Sayısı: "+cikti);
            Console.WriteLine();
            conn.Close();

        }
        public int countOgrenci(int countID)
        {
            
            conn=new SqlConnection(connString);
            conn.Open();
            cmd=new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText="SELECT COUNT (*) AS [STUDENT_ID] FROM STUDENT_INFO_TABLE WHERE STUDENT_ID=" +countID;
            var count=cmd.ExecuteScalar();
            conn.Close();
            return Convert.ToInt32(count);
            

        }
        public void ogrenciGuncelleme()
        {
            int removeId=-1;

            string IdString = readlineCem(6);
            int controledInt = idKontrol(IdString);
            if (controledInt > -1)
            {
                removeId = controledInt;

            }
            int studentIndex = countOgrenci(removeId);
            if (studentIndex > 0)
            {
                Student student = new Student();

                student.name = readlineCem(1);
                student.surname = readlineCem(2);
                student.gender = readlineCem(4);
                conn= new SqlConnection(connString);
                conn.Open();
                cmd=new SqlCommand();
                cmd.Connection = conn;

               

                cmd.CommandText = "UPDATE STUDENT_INFO_TABLE SET Student_Name = '" + student.name +"', "+ " Student_Surname = '" + student.surname +"', "+ " Student_Gender = '" + student.gender + "' WHERE Student_Id= " +removeId;
                cmd.ExecuteNonQuery();
                conn.Close();
                listele();
            }
            else
            {
                Console.WriteLine("Hatalı Öğrenci Numarası Girdiniz..\n");

            }

            islemKontrol();
        }
        public int idKontrol(string isIdNull)
        {
            int _id = -1;
            if (isIdNull != "")
            {
                bool tryParse = Int32.TryParse(isIdNull, out _id);
                if (tryParse)
                {
                    return _id;
                }
                else
                {

                    isIdNull = readlineCem(9);

                    int controledInt = idKontrol(isIdNull);
                    if (controledInt > -1)
                    {
                        return controledInt;
                    }
                }
            }
            return _id;
        }
        public string readlineCem(int tur)
        {
            string rValue = "";

            switch (tur)
            {
                case 1:
                    Console.WriteLine("Eklemek İstediğiniz Öğrencinin Adını Girin: ");
                    break;
                case 2:
                    Console.WriteLine("Eklemek İstediğiniz Öğrencinin Soyadını Girin: ");
                    break;
                case 4:
                    Console.WriteLine("Eklemek İstediğiniz Öğrencinin Cinsiyetini Girin: ");
                    break;
                case 5:
                    Console.WriteLine("Silmek İstediğiniz Öğrencinin Numarasını Girin: ");
                    break;
                case 6:
                    Console.WriteLine("Güncellemek İstediğiniz Öğrencinin Numarasını Girin: ");
                    break;
                case 7:
                    Console.WriteLine("Bu numarada bir öğrenci var.Lütfen Tekrar deneyin");
                    break;
                case 8:
                    Console.WriteLine("Hangi İşlemi Yapmak İstiyorsun.. \n 1-Listele 2-Ekle 3-Sil 4-Güncelleme 5-Cikis \n\n");
                    break;
                case 9:
                    Console.WriteLine("Yanlış format.Tekrar Deneyin");
                    break;


            }
            rValue = Console.ReadLine();

            if (rValue.Replace(" ", "") == "")
            {

                rValue = readlineCem(tur);
            }



            return rValue;
        }

    }

}