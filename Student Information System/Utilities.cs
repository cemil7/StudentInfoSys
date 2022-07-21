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
        int process;
        public void add()
        {
            Student student = new Student();
            
            student.name = readlineCem(1);
            student.surname = readlineCem(2);
            student.gender = readlineCem(4);
            dataAdd(student);
            processControl();
        }
        public void remove()
        {
            Student student = new Student();
            int removeId = -1;
            string IdString = readlineCem(5);
            int controledInt = idControl(IdString);

            if (controledInt > -1)
            {
                removeId = controledInt;
            }
            int studentIndex = countStudents(removeId);

            if (studentIndex > 0)
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM STUDENT_INFO_TABLE WHERE STUDENT_ID=" + removeId;
                cmd.ExecuteNonQuery();
                conn.Close();
                show();
            }
            else
            {
                Console.WriteLine("You Entered Wrong Student Number..\n");
            }
            processControl();

        }
        public int exit()
        {

            return -1;
        }
        public void show()
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select * FROM STUDENT_INFO_TABLE";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from Student_Info_Table", connection);

                    //Using Data Table
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Console.WriteLine("Using Data Table");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Console.WriteLine(dt.Rows[i]["Student_Id"].ToString() + " " + dt.Rows[i]["Student_Name"].ToString() + " " + dt.Rows[i]["Student_Surname"].ToString() + " " + dt.Rows[i]["Student_Gender"].ToString());
                    }
                    Console.WriteLine("-----------------------------");
                }

                conn.Close();
                Console.WriteLine();
                numberofStudents();

                addObject();
                processControl();

            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong.\n" + e);
            }
            Console.ReadKey();
        }
        public void dataAdd(Student student)
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO STUDENT_INFO_TABLE( Student_Name, Student_Surname, Student_Gender) VALUES ('" + student.name + "','" + student.surname + "','" + student.gender + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            show();
            processControl();

        }
        public void processControl()
        {

            string isProcessNull = readlineCem(8);

            if (isProcessNull != "")
            {
                bool tryParse = Int32.TryParse(isProcessNull, out process);
                if (tryParse)
                {

                }
                else
                {
                    Console.WriteLine("Wrong format. Try again");
                }

            }
            else
            {
                process = 0;
            }

            if (process == 1)
            {
                show();

            }
            else if (process == 2)
            {
                add();
            }
            else if (process == 3)
            {
                remove();
            }
            else if (process == 4)
            {
                updateStudents();
            }
            else if (process == 5)
            {
                exit();
            }

            else
            {
                Console.Write("You Entered Wrong Transaction Code...\n\n");
                processControl();
            }
        }
        public void numberofStudents()
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT COUNT (*) AS [STUDENT_ID] FROM STUDENT_INFO_TABLE";
            var value = cmd.ExecuteScalar();
            Console.WriteLine("Number of Students Registered in the System: " + value);
            Console.WriteLine();
            conn.Close();

        }
        public int countStudents(int countID)
        {

            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT COUNT (*) AS [STUDENT_ID] FROM STUDENT_INFO_TABLE WHERE STUDENT_ID=" + countID;
            var count = cmd.ExecuteScalar();
            conn.Close();
            return Convert.ToInt32(count);

        }
        public void updateStudents()
        {
            int removeId = -1;

            string IdString = readlineCem(6);
            int controledInt = idControl(IdString);
            if (controledInt > -1)
            {
                removeId = controledInt;

            }
            int studentIndex = countStudents(removeId);
            if (studentIndex > 0)
            {
                Student student = new Student();

                student.name = readlineCem(1);
                student.surname = readlineCem(2);
                student.gender = readlineCem(4);
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                
                cmd.CommandText = "UPDATE STUDENT_INFO_TABLE SET Student_Name = '" + student.name + "', " + " Student_Surname = '" + student.surname + "', " + " Student_Gender = '" + student.gender + "' WHERE Student_Id= " + removeId;
                cmd.ExecuteNonQuery();
                conn.Close();
                show();
            }
            else
            {
                Console.WriteLine("You Entered Wrong Student Number..\n");

            }

            processControl();
        }
        public int idControl(string isIdNull)
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

                    int controledInt = idControl(isIdNull);
                    if (controledInt > -1)
                    {
                        return controledInt;
                    }
                }
            }
            return _id;
        }
        public string readlineCem(int round)
        {
            string rValue = "";

            switch (round)
            {
                case 1:
                    Console.WriteLine("Enter the Name of the Student You Want to Add: ");
                    break;
                case 2:
                    Console.WriteLine("Enter the Surname of the Student You Want to Add: ");
                    break;
                case 4:
                    Console.WriteLine("Enter the Gender of the Student You Want to Add: ");
                    break;
                case 5:
                    Console.WriteLine("Enter the Id of the Student You Want to Delete: ");
                    break;
                case 6:
                    Console.WriteLine("Enter the Id of the Student You Want to Update: ");
                    break;
                case 7:
                    Console.WriteLine("There is a student at this number. Please try again");
                    break;
                case 8:
                    Console.WriteLine("Which Operation Do You Want To Do?.. \n 1-List 2-Add 3-Remove 4-Update 5-Exit \n\n");
                    break;
                case 9:
                    Console.WriteLine("Wrong format.Try again.");
                    break;


            }
            rValue = Console.ReadLine();

            if (rValue.Replace(" ", "") == "")
            {

                rValue = readlineCem(round);
            }

            return rValue;
        }

        public void addObject()
        {
            students.Clear();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("Select * from STUDENT_INFO_TABLE", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Student student = new Student();
                student.id = Convert.ToInt32(dr["Student_Id"].ToString());
                student.name = dr["Student_Name"].ToString();
                student.surname = dr["Student_Surname"].ToString();
                student.gender = dr["Student_Gender"].ToString();
                students.Add(student);
            }
            conn.Close();

        }

    }

}
