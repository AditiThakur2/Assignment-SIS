using StudentInformationSystem.Entities;
using StudentInformationSystem.Exceptions;
using StudentInformationSystem.Repository;
using StudentInformationSystem.Service;

ISISService sisServiceImpl = new SISServiceImpl();

while(true)
{
    Console.WriteLine("-------------------STUDENT----------------\n");
    Console.WriteLine("1. DisplayStudentInfo");
    Console.WriteLine("2. FindStudentById");
    Console.WriteLine("3. EnrollInCourse");
    Console.WriteLine("4. UpdateStudentInfo");
    Console.WriteLine("5. MakePayment");
    Console.WriteLine("6. GetEnrolledCourses");
    Console.WriteLine("7. GetPaymentHistory");
    Console.WriteLine("\n-------------------COURSE-----------------\n");
    Console.WriteLine("8. DisplayCourseInfo");
    Console.WriteLine("9. FindCourseById");
    Console.WriteLine("10. AssignTeacher");
    Console.WriteLine("11. UpdateCourseInfo");
    Console.WriteLine("12. GetEnrollments");
    Console.WriteLine("13. GetTeacher");
    Console.WriteLine("\n-----------------ENROLLMENT---------------\n");
    Console.WriteLine("14. DisplayEnrollmentInfo");
    Console.WriteLine("15. FindEnrollmentById");
    Console.WriteLine("16. GetStudent");
    Console.WriteLine("17. GetCourse");
    Console.WriteLine("\n-----------------TEACHER-------------------\n");
    Console.WriteLine("18. DisplayTeacherInfo");
    Console.WriteLine("19. FindTeacherById");
    Console.WriteLine("\n-----------------PAYMENT-------------------\n");
    Console.WriteLine("20. DisplayPaymentInfo");
    Console.WriteLine("21. FindPaymentById");
    Console.WriteLine("\nEnter your Choice:: \n");
    int choice = int.Parse(Console.ReadLine());

    switch(choice)
    {
        case 1:
            sisServiceImpl.DisplayStudentInfo();
            break;
        case 2:
            sisServiceImpl.FindStudentById(); 
            break;
        case 3:
            sisServiceImpl.EnrollInCourse(); 
            break;
        case 4:
            sisServiceImpl.UpdateStudentInfo();
            break;
        case 5:
            sisServiceImpl.MakePayment();
            break;
        case 6:
            sisServiceImpl.GetEnrolledCourses();
            break;
        case 7:
            sisServiceImpl.GetPaymentHistory();
            break;
        case 8:
            sisServiceImpl.DisplayCourseInfo(); 
            break;
        case 9:
            sisServiceImpl.FindCourseById();
            break;
        case 10:
            sisServiceImpl.AssignTeacher();
            break;
        case 11:
            sisServiceImpl.UpdateCourseInfo();
            break;
        case 12:
            sisServiceImpl.GetEnrollments();
            break;
        case 13:
            sisServiceImpl.GetTeacher();
            break;
        case 14:
            sisServiceImpl.DisplayEnrollmentInfo();
            break;
        case 15:
            sisServiceImpl.FindEnrollmentById();
            break;
        case 16:
            sisServiceImpl.GetStudent();
                break;
        case 17:
            sisServiceImpl.GetCourse();
            break;
        case 18:
            sisServiceImpl.DisplayTeacherInfo();
                break;
        case 19:
            sisServiceImpl.FindTeacherById();
            break;
        case 20:
            sisServiceImpl.DisplayPaymentInfo();
            break;
        case 21:
            sisServiceImpl.FindPaymentById();
            break;
    }
}






