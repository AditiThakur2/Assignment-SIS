using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Service
{
    internal interface ISISService
    {
        //Student Management
        void DisplayStudentInfo();
        void FindStudentById();
        void EnrollInCourse();
        void UpdateStudentInfo();
        void MakePayment();
        void GetEnrolledCourses();
        void GetPaymentHistory();




        //Course Management
        void DisplayCourseInfo();
        void FindCourseById();
        void AssignTeacher();
        void UpdateCourseInfo();
        void GetEnrollments();
        void GetTeacher();




        //Enrollment Management
        void DisplayEnrollmentInfo();
        void FindEnrollmentById();
        void GetStudent();
        void GetCourse();


        //Teacher Management
        void DisplayTeacherInfo();
        void FindTeacherById();


        //Payment Handling
        void DisplayPaymentInfo();
        void FindPaymentById();
    }
}
