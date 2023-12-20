using StudentInformationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Repository
{
    internal interface ISISRepository
    {
        //Student Management
        List<Student> DisplayStudentInfo();
        int EnrollInCourse(Student student, Course course);
        Student FindStudentById(int studentId);
        int UpdateStudentInfo(int studentId, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber);
        int MakePayment(int studentId, decimal amount, DateTime paymentDate);
        List<Course> GetEnrolledCourses(int studentID);
        List<Payment> GetPaymentHistory(int studentID);



        //Course Management
        List<Course> DisplayCourseInfo();
        Course FindCourseById(int courseId);
        int AssignTeacher(Course course, Teacher teacher);
        int UpdateCourseInfo(int courseId, string newCourseName, int newCredits, int newTeacherId);
        List<Enrollment> GetEnrollments(int courseID);
        Teacher GetTeacher(int courseID);




        //Enrollment Management
        List<Enrollment> DisplayEnrollmentInfo();
        Enrollment FindEnrollmentById(int enrollmentId);
        Student GetStudent(int enrollmentID);
        Course GetCourse(int enrollmentID);


        //Teacher Management
        List<Teacher> DisplayTeacherInfo();
        Teacher FindTeacherById(int teacherId);



        //Payment Handling
        List<Payment> DisplayPaymentInfo();
        Payment FindPaymentById(int paymentId);
    }
}
