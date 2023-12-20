using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudentInformationSystem.Entities;
using StudentInformationSystem.Exceptions;
using StudentInformationSystem.Repository;
using StudentInformationSystem.Service;
using StudentInformationSystem.Utility;


namespace StudentInformationSystem.Repository
{
    internal class SISRepositoryImpl:ISISRepository
    {
        public string connectionString;
        SqlCommand cmd = null;
        
        public SISRepositoryImpl()
        {
            connectionString = DBConnection.GetConnectionString();
            cmd = new SqlCommand();
        }

        //STUDENT MANAGEMENT
        #region DisplayStudentInfo
        public List<Student> DisplayStudentInfo()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM Students";
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Student student = new Student();
                    student.StudentID = (int)reader["student_id"];
                    student.FirstName= (string)reader["first_name"];
                    student.LastName = (string)reader["last_name"];
                    student.DateOfBirth = (DateTime)reader["date_of_birth"];
                    student.Email = (string)reader["email"];
                    student.PhoneNumber = (string)reader["phone_number"];
                    students.Add(student);
                }
                return students;
            }
        }
        #endregion
        #region FindStudentById
        public Student FindStudentById(int studentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Students WHERE student_id = @StudentID";
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            StudentID = (int)reader["student_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            DateOfBirth = (DateTime)reader["date_of_birth"],
                            Email = (string)reader["email"],
                            PhoneNumber = (string)reader["phone_number"]
                        };
                    }
                }
                return null;
            }
        }
        #endregion
        #region EnrollInCourse
        public int EnrollInCourse(Student student, Course course)
        {
            
            if(student == null)
            {
                throw new StudentNotFoundException($"Student object is missing or null !!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
            if(course == null)
            {
                throw new CourseNotFoundException($"Course object is missing or null !!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            }

            ValidateEnrollmentData(student, course);

            int studentID = student.StudentID;
            int courseID = course.CourseID;

            if (IsDuplicateEnrollment(studentID, courseID))
            {
                throw new DuplicateEnrollmentException($"Student with ID {studentID} is already enrolled in Course with ID {courseID}.\n");
            }
            

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Enrollments VALUES (@StudentID, @CourseID, @EnrollmentDate)";
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@CourseID", courseID);
                cmd.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int enrollStatus = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return enrollStatus;
            }
        }
        private bool IsDuplicateEnrollment(int studentId, int courseId)
        {
            // Logic to check if a student is already enrolled in a course
            
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Enrollments WHERE student_id = @studentID AND course_id = @courseID";
                cmd.Parameters.AddWithValue("@studentID", studentId);
                cmd.Parameters.AddWithValue("@courseID", courseId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private void ValidateEnrollmentData(Student student, Course course)
        {
            // Check if student or course references are missing
            if (student == null || course == null)
            {
                throw new InvalidEnrollmentDataException("Invalid enrollment data. Student or course references are missing.");
            }
        }
        #endregion
        #region UpdateStudentInfo
        public int UpdateStudentInfo(int studentId, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
            // Check if the student exists
            Student existingStudent = FindStudentById(studentId);
            if (existingStudent == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }

            // Validate the provided data before making any updates
            ValidateStudentData(firstName, lastName, dateOfBirth, email, phoneNumber);

            // Now update the student's information
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Students SET first_name = @FirstName, last_name = @LastName, date_of_birth = @DateOfBirth, email = @Email, phone_number = @PhoneNumber WHERE student_id = @S_ID";
                cmd.Parameters.AddWithValue("@S_ID", studentId);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }
        
        private void ValidateStudentData(string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
            // Example validation checks
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new InvalidStudentDataException("First name and last name are required.");
            }

            if (dateOfBirth >= DateTime.Now)
            {
                throw new InvalidStudentDataException("Invalid date of birth.");
            }
            if (!email.Contains("@"))
            {
                throw new InvalidStudentDataException("Invalid email format.");
            }
        }
        #endregion
        #region MakePayment
        public int MakePayment(int studentID, decimal amount, DateTime paymentDate)
        {
            Student existingStudent = FindStudentById(studentID);

            if (existingStudent == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentID} not found.");
            }

            // Validate the payment data before recording the payment
            ValidatePaymentData(amount, paymentDate);

            // Record the payment
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Payments VALUES (@SID, @Amount, @PaymentDate)";
                cmd.Parameters.AddWithValue("@SID", studentID);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }

        private void ValidatePaymentData(decimal amount, DateTime paymentDate)
        {
            if (amount <= 0)
            {
                throw new PaymentValidationException("Payment amount must be greater than zero.");
            }

            if (paymentDate > DateTime.Now)
            {
                throw new PaymentValidationException("Invalid payment date. Cannot record future payments.");
            }
        }
        #endregion
        #region GetEnrolledCourses
        public List<Course> GetEnrolledCourses(int studentID)
        {
            Student existingStudent = FindStudentById(studentID);
            if (existingStudent == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentID} not found.");
            }

            // Retrieve enrolled courses for the student
            List<Course> enrolledCourses = new List<Course>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Courses.course_id, Courses.course_name FROM Courses " +
                                  "JOIN Enrollments ON Courses.course_id = Enrollments.course_id " +
                                  "WHERE Enrollments.student_id = @sID";
                cmd.Parameters.AddWithValue("@sID", studentID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course
                    {
                        CourseID = (int)reader["course_id"],
                        CourseName = (string)reader["course_name"]
                    };
                    enrolledCourses.Add(course);
                }
            }

            return enrolledCourses;
        }
        #endregion
        #region GetPaymentHistory
        public List<Payment> GetPaymentHistory(int studentID)
        {
            Student existingStudent = FindStudentById(studentID);
            if (existingStudent == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentID} not found.");
            }

            
            List<Payment> paymentHistory = new List<Payment>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT payment_id, amount, payment_date FROM Payments " +
                                  "WHERE student_id = @StudentId " +
                                  "ORDER BY payment_date DESC";
                cmd.Parameters.AddWithValue("@StudentId", studentID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Payment payment = new Payment
                    {
                        PaymentID = (int)reader["payment_id"],
                        Amount = (decimal)reader["amount"],
                        PaymentDate = (DateTime)reader["payment_date"]
                    };
                    paymentHistory.Add(payment);
                }
            }

            return paymentHistory;
        }
        #endregion




        //COURSE MANAGEMENT
        #region DisplayCourseInfo
        public List<Course> DisplayCourseInfo()
        {
            List<Course> courses = new List<Course>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM Courses";
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();
                    course.CourseID = (int)reader["course_id"];
                    course.CourseName = (string)reader["course_name"];
                    course.Credit = (int)reader["credits"];
                    course.TeacherID = (int)reader["teacher_id"];
                    courses.Add(course);
                }
                return courses;
            }
        }
        #endregion
        #region FindCourseById
        public Course FindCourseById(int courseId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Courses WHERE course_id = @CourseID";
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Course
                        {
                            CourseID = (int)reader["course_id"],
                            CourseName = (string)reader["course_name"],
                            Credit = (int)reader["credits"],
                            TeacherID = (int)reader["teacher_id"]
                        };
                    }
                }
                return null;
            }
        }
        #endregion
        #region AssignTeacher

        public int AssignTeacher(Course course, Teacher teacher)
        {
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with ID {course.CourseID} not found.");
            }
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"Teacher with ID {teacher.TeacherID} not found.");
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Courses SET teacher_id = @TeacherID WHERE course_id = @CourseId";
                cmd.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                cmd.Parameters.AddWithValue("@CourseId", course.CourseID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }
        #endregion
        #region UpdateCourseInfo

        public int UpdateCourseInfo(int courseId, string newCourseName, int newCredits, int newTeacherId)
        {
            Course existingCourse = FindCourseById(courseId);
            Teacher teacher = FindTeacherById(newTeacherId);
            if (existingCourse == null)
            {
                throw new CourseNotFoundException($"Course with Id {courseId} not found.");
            }
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"Teacher with Id {newTeacherId}");
            }

            // Validate the updated course information
            ValidateCourseData(newCourseName, newTeacherId);

            // Update the course information
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Courses SET course_name = @NewCourseName,credits = @Credits, teacher_id = @NewTeacherId WHERE course_id = @Courseid";
                cmd.Parameters.AddWithValue("@Customerid", courseId);
                cmd.Parameters.AddWithValue("@NewCourseName", newCourseName);
                cmd.Parameters.AddWithValue("@Credits", newCredits);
                cmd.Parameters.AddWithValue("@NewTeacherId", newTeacherId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }
        private void ValidateCourseData(string newCourseName, int newTeacherId)
        {
            // Validate new course name
            if (string.IsNullOrEmpty(newCourseName))
            {
                throw new InvalidCourseDataException("New course name cannot be null or empty.");
            }

            // Validate new instructor name
            if (newTeacherId <= 0)
            {
                throw new InvalidCourseDataException("New instructor name cannot be null or empty.");
            }
        }

        #endregion
        #region GetEnrollments

        public List<Enrollment> GetEnrollments(int courseID)
        {
            Course existingCourse = FindCourseById(courseID);
            if (existingCourse == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseID} not found.");
            }

            List<Enrollment> enrollments = new List<Enrollment>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT enrollment_id, student_id, enrollment_date FROM Enrollments " +
                                  "WHERE course_id = @CID " +
                                  "ORDER BY enrollment_date DESC";
                cmd.Parameters.AddWithValue("@CID", courseID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Enrollment enrollment = new Enrollment
                    {
                        EnrollmentID = (int)reader["enrollment_id"],
                        StudentID = (int)reader["student_id"],
                        EnrollmentDate = (DateTime)reader["enrollment_date"]
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }
        #endregion
        #region GetTeacher

        public Teacher GetTeacher(int courseID)
        {
            Course existingCourse = FindCourseById(courseID);
            if (existingCourse == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseID} not found.");
            }

            // Retrieve the assigned teacher for the course
            Teacher teacher = null;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT teacher_id, first_name, last_name, email FROM Teacher " +
                                  "WHERE teacher_id = @TID";
                cmd.Parameters.AddWithValue("@TID", existingCourse.TeacherID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    teacher = new Teacher
                    {
                        TeacherID = (int)reader["teacher_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Email = (string)reader["email"]
                    };
                }
            }

            return teacher;
        }
        #endregion






        //ENROLLMENT MANAGEMENT
        #region DisplayEnrollmentInfo
        public List<Enrollment> DisplayEnrollmentInfo()
        {
            List<Enrollment> enrollments = new List<Enrollment>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM Enrollments";
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Enrollment enrollment = new Enrollment();
                    enrollment.EnrollmentID = (int)reader["enrollment_id"];
                    enrollment.StudentID = (int)reader["student_id"];
                    enrollment.CourseID = (int)reader["course_id"];
                    enrollment.EnrollmentDate = (DateTime)reader["enrollment_date"];
                    enrollments.Add(enrollment);
                }
                return enrollments;
            }
        }
        #endregion
        #region FindEnrollmentById
        public Enrollment FindEnrollmentById(int enrollmentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Enrollments WHERE enrollment_id = @EnrollmentID";
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Enrollment
                        {
                            EnrollmentID = (int)reader["enrollment_id"],
                            StudentID = (int)reader["student_id"],
                            CourseID = (int)reader["course_id"],
                            EnrollmentDate = (DateTime)reader["enrollment_date"]
                        };
                    }
                }
                return null;
            }
        }
        #endregion
        #region GetStudent
        public Student GetStudent(int enrollmentID)
        {
            Enrollment existingEnrollment = FindEnrollmentById(enrollmentID);
            if (existingEnrollment == null)
            {
                throw new EnrollmentNotFoundException($"Enrollment with ID {enrollmentID} not found.");
            }

            Student student = null;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT student_id, first_name, last_name, date_of_birth, email, phone_number FROM Students " +
                                  "WHERE student_id = @StudID";
                cmd.Parameters.AddWithValue("@StudtID", existingEnrollment.StudentID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    student = new Student
                    {
                        StudentID = (int)reader["student_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        DateOfBirth = (DateTime)reader["date_of_birth"],
                        Email = (string)reader["email"],
                        PhoneNumber = (string)reader["phone_number"]
                    };
                }
            }

            return student;
        }
        #endregion
        #region GetCourse
        public Course GetCourse(int enrollmentID)
        {
            Enrollment existingEnrollment = FindEnrollmentById(enrollmentID);
            if (existingEnrollment == null)
            {
                throw new EnrollmentNotFoundException($"Enrollment with ID {enrollmentID} not found.");
            }

            Course course = null;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT course_id, course_name, credits, teacher_id FROM Courses " +
                                  "WHERE course_id = @cID";
                cmd.Parameters.AddWithValue("@cID", existingEnrollment.CourseID);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    course = new Course
                    {
                        CourseID = (int)reader["course_id"],
                        CourseName = (string)reader["course_name"],
                        Credit = (int)reader["credits"],
                        TeacherID = (int)reader["teacher_id"]
                    };
                }
            }
            return course;
        }
        #endregion



        //TEACHER MANAGEMENT
        public List<Teacher> DisplayTeacherInfo()
        {
            List<Teacher> teachers = new List<Teacher>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM Teacher";
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Teacher teacher = new Teacher();
                    teacher.TeacherID = (int)reader["teacher_id"];
                    teacher.FirstName = (string)reader["first_name"];
                    teacher.LastName = (string)reader["last_name"];
                    teacher.Email = (string)reader["email"];
                    teachers.Add(teacher);
                }
                return teachers;
            }
        }
        public Teacher FindTeacherById(int teacherId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Teacher WHERE teacher_id = @TeacherID";
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Teacher
                        {
                            TeacherID = (int)reader["teacher_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            Email = (string)reader["email"]
                        };
                    }
                }
                return null;
            }
        }



        //PAYMENT HANDLING
        public List<Payment> DisplayPaymentInfo()
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM Payments";
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Payment payment = new Payment();
                    payment.PaymentID = (int)reader["payment_id"];
                    payment.StudentID = (int)reader["student_id"];
                    payment.Amount = (decimal)reader["amount"];
                    payment.PaymentDate = (DateTime)reader["payment_date"];
                    payments.Add(payment);
                }
                return payments;
            }
        }
        public Payment FindPaymentById(int paymentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Payments WHERE payment_id = @PaymentID";
                cmd.Parameters.AddWithValue("@PaymentID", paymentId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Payment
                        {
                            PaymentID = (int)reader["payment_id"],
                            StudentID = (int)reader["student_id"],
                            Amount = (decimal)reader["amount"],
                            PaymentDate = (DateTime)reader["payment_date"]
                        };
                    }
                }
                return null;
            }
        }

    }
}
