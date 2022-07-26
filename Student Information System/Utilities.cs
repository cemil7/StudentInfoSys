﻿using System;
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
        int Lessonprocess;
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

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from Student_Info_Table", connection);


                    //Using Data Table
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    //Using Data Table;
                    Console.WriteLine("Student List\n");
                    Console.WriteLine("Id||Name||Surname||Gender\n");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Console.WriteLine(dt.Rows[i]["Student_Id"].ToString() + " " + dt.Rows[i]["Student_Name"].ToString() + " " + dt.Rows[i]["Student_Surname"].ToString() + " " + dt.Rows[i]["Student_Gender"].ToString());
                    }
                    Console.WriteLine("-----------------------------");
                }

                Console.WriteLine();
                numberofStudents();

                getStudent();
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
                showLesson();
            }
            else if (process == 6)
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
        public int countLessons(int countLesson)
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT COUNT (*) AS [LESSON_ID] FROM LESSON_TABLE WHERE LESSON_ID=" + countLesson;
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
                    Console.WriteLine("Which Operation Do You Want To Do?.. \n 1-Student List 2-Add Student 3-Remove Student 4-Update Student 5-Lessons List 6-Exit \n\n");
                    break;
                case 9:
                    Console.WriteLine("Wrong format.Try again.");
                    break;
                case 10:
                    Console.WriteLine("Enter the code of the student's lesson:  ");
                    getLessonId();
                    break;
                case 11:
                    Console.WriteLine("Enter the Id of the Student You Want to Enroll in the Lesson: ");
                    break;
                case 12:
                    Console.WriteLine("Which Operation Do You Want To Do?.. \n 1-Adding the Student to the Lesson 2-Deleting a Student from a Lesson 3-Updating the Course Quota 4-Show Students Taking the Lesson 5-Main Menu 6-Exit");
                    break;
                case 13:
                    Console.WriteLine("Enter the Id of the Student to be Deleted from the Lesson:");
                    break;
                case 14:
                    Console.WriteLine("Enter the Lesson Id:");
                    break;
                case 15:
                    Console.WriteLine("Enter the new quota: ");
                    break;


            }
            rValue = Console.ReadLine();

            if (rValue.Replace(" ", "") == "")
            {

                rValue = readlineCem(round);
            }

            return rValue;
        }
        public void getStudent()
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
        public int quotaCount(int lessonId)
        {
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select count(*)from StudentLessons where Lesson_Id =" + lessonId;
            var countQ = cmd.ExecuteScalar();
            conn.Close();
            return Convert.ToInt32(countQ);

        }
        public int quotaControl(int lessonId)
        {
            int lessonQuota;
            int lessonOccupancy = quotaCount(lessonId);
            conn = new SqlConnection(connString);
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select Lesson_Quota from Lesson_Table where Lesson_Id=" + lessonId;

            lessonQuota = Convert.ToInt32(cmd.ExecuteScalar());
            if (lessonQuota > lessonOccupancy)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public void showLesson()
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from Lesson_Table", connection);



                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Console.WriteLine("Lessons List:\n");
                    Console.WriteLine("Lesson Id|Remaining Quota|Lesson Name");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int kota = Convert.ToInt32(dt.Rows[i]["Lesson_Quota"].ToString());
                        int dersAlanSayisi = quotaCount(Convert.ToInt32(dt.Rows[i]["Lesson_Id"].ToString()));
                        int sonuc = kota - dersAlanSayisi;

                        Console.WriteLine(dt.Rows[i]["Lesson_Id"].ToString() + "         " + sonuc + "              " + dt.Rows[i]["Lesson_Name"].ToString());

                    }
                    Console.WriteLine("-----------------------------");
                }


                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong.\n" + e);
            }

            lessonProcess();
        }
        public void addToLesson()
        {
            int studentId = Convert.ToInt32(readlineCem(11));
            int lessonId = Convert.ToInt32(readlineCem(10));
            if (quotaControl(lessonId) == 1)
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO StudentLessons( Student_Id, Lesson_Id) VALUES ('" + studentId + "','" + lessonId + "')";
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("Student Added..");
                lessonProcess();
            }
            else
            {
                Console.WriteLine("There is no quota left in the lesson.");
                Console.WriteLine();
                lessonProcess();
            }


        }
        public void lessonProcess()
        {
            string isProcessNull = readlineCem(12);

            if (isProcessNull != "")
            {
                bool tryParse = Int32.TryParse(isProcessNull, out Lessonprocess);
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
                Lessonprocess = 0;
            }

            if (Lessonprocess == 1)
            {
                addToLesson();
            }
            else if (Lessonprocess == 2)
            {
                removeToLesson();
            }
            else if (Lessonprocess == 3)
            {
                uptadeLessonQuota();
            }
            else if (Lessonprocess == 4)
            {
                showStudentLesson();
            }
            else if (Lessonprocess == 5)
            {
                processControl();
            }
            else if (Lessonprocess == 6)
            {
                exit();
            }

        }
        public void getLessonId()

        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select Lesson_Name,Lesson_Id from Lesson_Table order by Lesson_Id", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Console.WriteLine(dt.Rows[i]["Lesson_Name"].ToString() + " " + dt.Rows[i]["Lesson_Id"].ToString());
                }

            }


            Console.WriteLine();


        }
        public void removeToLesson()
        {
            int studentId = Convert.ToInt32(readlineCem(13));
            int lessonId = Convert.ToInt32(readlineCem(14));
            int removeId = -1;
            int controledInt = idControl(Convert.ToString(studentId));

            if (controledInt > -1)
            {
                removeId = controledInt;
            }
            int LessonIndex = countLessons(removeId);

            if (LessonIndex > 0)
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM StudentLessons WHERE STUDENT_ID=" + removeId + "AND Lesson_Id=" + lessonId;
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("Student Deleted..");
                lessonProcess();

            }
            else
            {
                Console.WriteLine("You Entered Wrong Student Number..\n");
                lessonProcess();
            }
            Console.ReadLine();


        }
        public void uptadeLessonQuota()
        {

            int lessonId = Convert.ToInt32(readlineCem(14));
            int newQuota = Convert.ToInt32(readlineCem(15));

            int removeId = -1;
            int controledInt = idControl(Convert.ToString(lessonId));
            if (controledInt > -1)
            {
                removeId = controledInt;
            }
            int LessonIndex = countLessons(removeId);

            if (LessonIndex > 0)
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Lesson_Table SET Lesson_Quota=" + newQuota + "WHERE Lesson_Id=" + lessonId;
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("Lesson Quota Updated....");
                lessonProcess();
            }
            else
            {
                Console.WriteLine("You Entered Wrong Lesson Number..\n");
                lessonProcess();
            }




        }
        public void showStudentLesson()
        {
            string lessonId = (readlineCem(14));
            int removeId = -1;
            if (lessonId != "")
            {
                bool tryParse = Int32.TryParse(lessonId, out Lessonprocess);
                if (tryParse)
                {

                }
                else
                {
                    Console.WriteLine("Wrong format. Try again");
                }
                int controledInt = idControl(Convert.ToString(lessonId));
                if (controledInt > -1)
                {
                    removeId = controledInt;
                }
                int LessonIndex = countLessons(removeId);

                if (LessonIndex > 0)
                {

                    try
                    {

                        using (SqlConnection connection = new SqlConnection(connString))
                        {
                            SqlDataAdapter da = new SqlDataAdapter("SELECT Student_Info_Table.Student_Id, Student_Info_Table.Student_Name,Student_Info_Table.Student_Surname,Student_Info_Table.Student_Gender FROM Student_Info_Table LEFT JOIN StudentLessons ON Student_Info_Table.Student_Id = StudentLessons.Student_Id WHERE Lesson_Id =" + lessonId, connection);


                            //Using Data Table
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            //Using Data Table;
                            Console.WriteLine("\n");
                            Console.WriteLine("Id||Name||Surname||Gender\n");

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Console.WriteLine(dt.Rows[i]["Student_Id"].ToString() + " " + dt.Rows[i]["Student_Name"].ToString() + " " + dt.Rows[i]["Student_Surname"].ToString() + " " + dt.Rows[i]["Student_Gender"].ToString());
                            }
                            Console.WriteLine("-----------------------------\n");
                        }

                        
                        lessonProcess();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs, something went wrong.\n" + e);
                    }
                    Console.ReadKey();

                }
                else
                {
                    Console.WriteLine("\nThis student is not enrolled in any lesson.\n");
                    lessonProcess();
                }
            }
        }
    }
}
