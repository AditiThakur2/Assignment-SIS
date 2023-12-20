using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entities
{
    internal class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int Credit { get; set; }
        public int TeacherID { get; set; }

        public Course() { }

        public Course(int courseID, string courseName, int credit, int teacherID)
        {
            CourseID = courseID;
            CourseName = courseName;
            Credit = credit;
            TeacherID = teacherID;
        }
        public override string ToString()
        {
            return $"CourseID:: {CourseID}, CourseName:: {CourseName}, Credit:: {Credit}, TeacherID:: {TeacherID}";
        }
    }
}
