using StudentInformationSystem.Entities;
using StudentInformationSystem.Exceptions;
using StudentInformationSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StudentInformationSystem.Service
{
    internal class SISServiceImpl:ISISService
    {
        ISISRepository sisRepositoryImpl = new SISRepositoryImpl();


        #region STUDENT MANAGEMENT

        public void DisplayStudentInfo()
        {
            var students = sisRepositoryImpl.DisplayStudentInfo();
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }
        public void FindStudentById()
        {
            Console.Write("Enter the Student ID:: ");
            int studentID = int.Parse(Console.ReadLine());
            Student foundStudent = sisRepositoryImpl.FindStudentById(studentID);
            if (foundStudent != null)
            {
                Console.WriteLine($"Found Student: ID: {foundStudent.StudentID}, Name:: {foundStudent.FirstName} {foundStudent.LastName}, DOB:: {foundStudent.DateOfBirth}, Email:: {foundStudent.Email}, Phone:: {foundStudent.PhoneNumber}\n");
            }
            else
            {
                Console.WriteLine($"Student with ID {studentID} not Found !!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
        }

        public void EnrollInCourse()
        {
            Console.Write("Enter the StudentId of the Student to Enroll:: ");
            int studentID = int.Parse(Console.ReadLine());
            Console.Write("Enter the CouseId to Enroll in :: ");
            int courseID = int.Parse(Console.ReadLine());
            try
            {
                Student student = sisRepositoryImpl.FindStudentById(studentID);
                Course  course = sisRepositoryImpl.FindCourseById(courseID);
                int enrollStatus = sisRepositoryImpl.EnrollInCourse(student, course);
                if(enrollStatus>0)
                {
                    Console.WriteLine("Student Enrolled Successfully !!!!!!!!!!!!!!!!!!\n");
                }
                else
                {
                    Console.WriteLine("There was some error while Enrolling the Student !!!!!!!!!!!!!!!!!\n");
                }
            }
            catch(StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(DuplicateEnrollmentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(InvalidEnrollmentDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateStudentInfo()
        {
            Console.Write("Enter student ID: ");
            int studentId = int.Parse(Console.ReadLine());
            Console.Write("Enter new first name: ");
            string newFirstName = Console.ReadLine();
            Console.Write("Enter new last name: ");
            string newLastName = Console.ReadLine();
            Console.Write("Enter new date of birth (yyyy-MM-dd): ");
            DateTime newDateOfBirth = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new email: ");
            string newEmail = Console.ReadLine();
            Console.Write("Enter new phone number: ");
            string newPhoneNumber = Console.ReadLine();
            try
            {
                int rowsAffected = sisRepositoryImpl.UpdateStudentInfo(studentId, newFirstName, newLastName,newDateOfBirth,newEmail,newPhoneNumber);
                if(rowsAffected > 0) 
                {
                    Console.WriteLine("Student information updated successfully. !!!!!!!!!!!!!!11\n");
                }
                else
                {
                    Console.WriteLine("There was Some Error while Updating StudentInfo !!!!!!!!!!111\n");
                }
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidStudentDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void MakePayment()
        {
            Console.Write("Enter student ID: ");
            int studentId = int.Parse(Console.ReadLine());
            Console.Write("Enter payment amount: ");
            decimal paymentAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Enter payment date (yyyy-MM-dd): ");
            DateTime paymentDate = DateTime.Parse(Console.ReadLine());
            try
            {
                int rowsAffected = sisRepositoryImpl.MakePayment(studentId, paymentAmount, paymentDate);
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Payment recorded successfully.!!!!!!!!!!!!!!!!\n");
                }
                else
                {
                    Console.WriteLine("There was Error While Making Payment !!!!!!!!!!!!!!\n");
                }
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            catch (PaymentValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetEnrolledCourses()
        {
            Console.Write("Enter the StudentId:: ");
            int studentId = int.Parse(Console.ReadLine());
            try
            {
                List<Course> enrolledCourses = sisRepositoryImpl.GetEnrolledCourses(studentId);

                if (enrolledCourses.Count == 0)
                {
                    Console.WriteLine("No courses enrolled for the student.");
                }
                else
                {
                    Console.WriteLine("Enrolled Courses:");
                    foreach (var course in enrolledCourses)
                    {
                        Console.WriteLine($"{course.CourseName} (Course ID: {course.CourseID})");
                    }
                }
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GetPaymentHistory()
        {
            Console.Write("Enter the StudentId:: ");
            int studentId = int.Parse(Console.ReadLine());
            try
            {
                List<Payment> paymentHistory = sisRepositoryImpl.GetPaymentHistory(studentId);

                if (paymentHistory.Count == 0)
                {
                    Console.WriteLine("No payment history for the student. !!!!!!!!!!!!!!!!!!!");
                }
                else
                {
                    Console.WriteLine("Payment History:");
                    foreach (var payment in paymentHistory)
                    {
                        Console.WriteLine($"Payment ID: {payment.PaymentID}, Amount: {payment.Amount}, Date: {payment.PaymentDate}");
                    }
                }
            }
            catch(StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion






        #region COURSE MANAGEMENT
        public void DisplayCourseInfo()
        {
            var courses = sisRepositoryImpl.DisplayCourseInfo();
            foreach (var course in courses)
            {
                Console.WriteLine(course);
            }
        }
        public void FindCourseById()
        {
            Console.WriteLine("Enter the Course ID:: ");
            int courseID = int.Parse(Console.ReadLine());

            Course foundCourse = sisRepositoryImpl.FindCourseById(courseID);
            if (foundCourse != null)
            { 
                Console.WriteLine($"Found Course: ID: {foundCourse.CourseID}, CourseName:: {foundCourse.CourseName}, Credits:: {foundCourse.Credit}, TeacherId:: {foundCourse.TeacherID}\n");
            }
            else
            {
                Console.WriteLine($"Course with ID {courseID} not Found !!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
        }
        public void AssignTeacher()
        {
            Console.Write("Enter the CourseId in which Teacher needs to be assigned:: ");
            int courseId = int.Parse(Console.ReadLine());
            Console.Write("Enter the teacherid:: ");
            int teacherId = int.Parse(Console.ReadLine());
            try
            {
                Course course = sisRepositoryImpl.FindCourseById(courseId);
                Teacher teacher = sisRepositoryImpl.FindTeacherById(teacherId);
                int rowsAffected = sisRepositoryImpl.AssignTeacher(course, teacher);
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Teacher Assigned Successfully   !!!!!!!!!!!!!!!!!!!!!!!!!\n");
                }
                else
                {
                    Console.WriteLine("There was some error while Assigning Teacher !!!!!!!!!!!!!!!!!!\n");
                }
            } 
            catch(CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(TeacherNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        public void UpdateCourseInfo()
        {
            Console.Write("Enter the CourseId to be Updated:: ");
            int courseId = int.Parse(Console.ReadLine());
            Console.Write("Enter the Course Name:: ");
            string newCourseName = Console.ReadLine();
            Console.Write("Enter the Credits of Course::");
            int newCredit = int.Parse(Console.ReadLine());
            Console.Write("Enter the TeacherId:: ");
            int newTeacherId = int.Parse(Console.ReadLine());
            try
            {
                int rowsAffected = sisRepositoryImpl.UpdateCourseInfo(courseId, newCourseName, newCredit, newTeacherId);
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Course Updated Successfully !!!!!!!!!!!!!!!!!!!!!!!!\n");
                }
                else
                {
                    Console.WriteLine("There was some Error While Updating Course Info !!!!!!!!!!!!!!!1\n");
                }
            }
            catch(CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(TeacherNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidCourseDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GetEnrollments()
        {
            Console.Write("Enter the CourseId:: ");
            int courseId = int.Parse(Console.ReadLine());
            try
            {
                List<Enrollment> enrollments = sisRepositoryImpl.GetEnrollments(courseId);

                if (enrollments.Count == 0)
                {
                    Console.WriteLine("No enrollments for the course.");
                }
                else
                {
                    Console.WriteLine("Enrollments:");
                    foreach (var enrollment in enrollments)
                    {
                        Console.WriteLine($"Enrollment ID: {enrollment.EnrollmentID}, Student ID: {enrollment.StudentID}, Enrollment Date: {enrollment.EnrollmentDate}");
                    }
                }
            }
            catch (CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }
        public void GetTeacher()
        {
            Console.Write("Enter course ID: ");
            int courseId = int.Parse(Console.ReadLine());
            try
            {
                Teacher teacher = sisRepositoryImpl.GetTeacher(courseId);

                if (teacher == null)
                {
                    Console.WriteLine("No teacher assigned to the course.");
                }
                else
                {
                    Console.WriteLine($"Assigned Teacher: {teacher.FirstName} {teacher.LastName}, Email: {teacher.Email}");
                }
            }
            catch (CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }        
        }
        #endregion





        # region ENROLLMENT MANAGEMENT
        public void DisplayEnrollmentInfo()
        {
            var enrollments = sisRepositoryImpl.DisplayEnrollmentInfo();
            foreach (var enrollment in enrollments)
            {
                Console.WriteLine(enrollment);
            }
        }
        public void FindEnrollmentById()
        {
            Console.WriteLine("Enter the Enrollment ID:: ");
            int enrollmentID = int.Parse(Console.ReadLine());

            Enrollment foundEnrollment = sisRepositoryImpl.FindEnrollmentById(enrollmentID);
            if (foundEnrollment != null)
            {
                Console.WriteLine($"Found Enrollment: ID: {foundEnrollment.EnrollmentID}, StudentId:: {foundEnrollment.StudentID}, CourseId:: {foundEnrollment.CourseID}, EnrollmentDate:: {foundEnrollment.EnrollmentDate}\n");
            }
            else
            {
                Console.WriteLine($"Enrollment with ID {enrollmentID} not Found !!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
        }
        public void GetStudent()
        {
            Console.Write("Enter enrollment ID: ");
            int enrollmentId = int.Parse(Console.ReadLine());
            try
            {
                Student student = sisRepositoryImpl.GetStudent(enrollmentId);

                if (student == null)
                {
                    Console.WriteLine("No student associated with the enrollment.");
                }
                else
                {
                    Console.WriteLine($"Associated Student: {student.FirstName} {student.LastName}, Email: {student.Email}");
                }
            }
            catch (EnrollmentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GetCourse()
        {
            Console.Write("Enter enrollment ID: ");
            int enrollmentId = int.Parse(Console.ReadLine());
            try
            {
                Course course = sisRepositoryImpl.GetCourse(enrollmentId);

                if (course == null)
                {
                    Console.WriteLine("No course associated with the enrollment.");
                }
                else
                {
                    Console.WriteLine($"Associated Course: {course.CourseName}, Code: {course.Credit}, Instructor: {course.TeacherID}");
                }
            }
            catch (EnrollmentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion






        //TEACHER MANAGEMENT
        public void DisplayTeacherInfo()
        {
            var teachers = sisRepositoryImpl.DisplayTeacherInfo();
            foreach(var teacher in teachers)
            {
                Console.WriteLine(teacher);
            }
        }
        public void FindTeacherById()
        {
            Console.WriteLine("Enter the Teacher Id:: ");
            int teacherID = int.Parse(Console.ReadLine());
            Teacher foundTeacher = sisRepositoryImpl.FindTeacherById(teacherID);
            if (foundTeacher != null)
            {
                Console.WriteLine($"Found Teacher: ID: {foundTeacher.TeacherID}, Name:: {foundTeacher.FirstName} {foundTeacher.LastName}, Email:: {foundTeacher.Email}\n");
            }
            else
            {
                Console.WriteLine($"Teacher with ID {teacherID} not Found !!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
        }









        //PAYMENT HANDLING
        public void DisplayPaymentInfo()
        {
            var payments = sisRepositoryImpl.DisplayPaymentInfo();
            foreach (var payment in payments)
            {
                Console.WriteLine(payment);
            }
        }

        public void FindPaymentById()
        {
            Console.WriteLine("Enter the Payment ID:: ");
            int paymentID = int.Parse(Console.ReadLine());
            Payment foundPayment = sisRepositoryImpl.FindPaymentById(paymentID);
            if (foundPayment != null)
            {
                Console.WriteLine($"Found Payment: ID:: {foundPayment.PaymentID}, StudentId:: {foundPayment.StudentID}, Amount:: {foundPayment.Amount}, PaymentDate:: {foundPayment.PaymentDate}\n");
            }
            else
            {
                Console.WriteLine($"Payment with Id {paymentID} not Found !!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            }

        }
    }
}
