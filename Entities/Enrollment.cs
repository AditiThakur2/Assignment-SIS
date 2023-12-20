using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entities
{
    internal class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Enrollment() { }
        public Enrollment(int enrollmentID, int studentID, int courseID, DateTime enrollmentDate)
        {
            EnrollmentID = enrollmentID;
            StudentID = studentID;
            CourseID = courseID;
            EnrollmentDate = enrollmentDate;
        }
        public override string ToString()
        {
            return $"EnrollmentID:: {EnrollmentID}, StudentID:: {StudentID}, CourseID:: {CourseID}, EnrollmentDate:: {EnrollmentDate}";
        }
    }
}
