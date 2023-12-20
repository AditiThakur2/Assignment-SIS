-- Create Database
CREATE DATABASE SIS

-- Use Database
USE SIS

-- Create Tables
CREATE TABLE Students (
    student_id INT IDENTITY PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    date_of_birth DATE,
    email VARCHAR(100),
    phone_number VARCHAR(20))

CREATE TABLE Courses (
    course_id INT IDENTITY PRIMARY KEY,
    course_name VARCHAR(100),
    credits INT,
    teacher_id INT,
    FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id))

CREATE TABLE Enrollments (
    enrollment_id INT IDENTITY PRIMARY KEY,
    student_id INT,
    course_id INT,
    enrollment_date DATE,
    FOREIGN KEY (student_id) REFERENCES Students(student_id),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id))

CREATE TABLE Teacher (
    teacher_id INT IDENTITY PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100))

CREATE TABLE Payments (
    payment_id INT IDENTITY PRIMARY KEY,
    student_id INT,
    amount DECIMAL(10,2),
    payment_date DATE,
    FOREIGN KEY (student_id) REFERENCES Students(student_id))

-- Add Primary Key and Foreign Key Constraints
--Primary Key Constraints
ALTER TABLE Students ADD CONSTRAINT PK_Students PRIMARY KEY (student_id)
ALTER TABLE Courses ADD CONSTRAINT PK_Courses PRIMARY KEY (course_id)
ALTER TABLE Enrollments ADD CONSTRAINT PK_Enrollments PRIMARY KEY (enrollment_id)
ALTER TABLE Teacher ADD CONSTRAINT PK_Teacher PRIMARY KEY (teacher_id)
ALTER TABLE Payments ADD CONSTRAINT PK_Payments PRIMARY KEY (payment_id)

-- Add Foreign Key Constraints
ALTER TABLE Courses ADD CONSTRAINT FK_Teacher FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id)
ALTER TABLE Enrollments ADD CONSTRAINT FK_Student FOREIGN KEY (student_id) REFERENCES Students(student_id)
ALTER TABLE Enrollments ADD CONSTRAINT FK_Course FOREIGN KEY (course_id) REFERENCES Courses(course_id)
ALTER TABLE Payments ADD CONSTRAINT FK_Payment_Student FOREIGN KEY (student_id) REFERENCES Students(student_id)

--A)Insert the values 
INSERT INTO Students VALUES('Steve','Johnson','2001-03-23','Steve.johnson123@gmail.com',7652890032),
('Sophia','Davis','1999-04-09','sophia342@gmail.com',9232456780),
('Noah','Martinez','2000-10-01','noah.martinez@email.com',7768990045),
('Olivia','Taylor','2000-12-12','olivia.taylor@email.com',9763456280),
('Jackson','Brown','1999-05-23','jackson.brown@email.com',6843356789),
('Ava','Clark','2001-02-28','ava.clark@email.com',9998762345),
('Lucas','Adams','2002-12-19','lucas.adams@email.com',8834562311),
('Mia','Turner','2001-08-23','mia.turner@email.com',6344345678),
('Ethan','Baker','1998-10-10','ethan.baker@email.com',7676789005),
('Isabella','White','1999-01-01','isabella.white@email.com',9450036627)
SELECT * FROM Students

INSERT INTO Teacher (first_name, last_name, email)
VALUES
('John', 'Smith', 'john.smith@example.com'),
('Jane', 'Doe', 'jane.doe@example.com'),
('Michael', 'Johnson', 'michael.johnson@example.com'),
('Emily', 'Brown', 'emily.brown@example.com'),
('David', 'Wilson', 'david.wilson@example.com'),
('Amy', 'Jones', 'amy.jones@example.com'),
('Christopher', 'Miller', 'christopher.miller@example.com'),
('Sophia', 'Davis', 'sophia.davis@example.com'),
('William', 'Martinez', 'william.martinez@example.com'),
('Olivia', 'Taylor', 'olivia.taylor@example.com')
SELECT * FROM Teacher

INSERT INTO Courses
VALUES
('Math 101', 3, 1),
('History 101', 3, 2),
('English 101', 3, 3),
('Physics 101', 4, 4),
('Computer Science 101', 4, 5),
('Psychology 101', 3, 6),
('Chemistry 101', 4, 7),
('Art History 101', 3, 8),
('Political Science 101', 3, 9),
('Economics 101', 3, 10)
SELECT * FROM Courses

INSERT INTO Enrollments (student_id, course_id, enrollment_date)
VALUES
(30, 2, '2023-01-15'),
(31, 2, '2023-01-20'),
(32, 8, '2023-02-01'),
(33, 4, '2023-02-10'),
(34, 5, '2023-02-15'),
(35, 10, '2023-03-01'),
(36, 4, '2023-03-05'),
(37, 6, '2023-03-10'),
(38, 8, '2023-03-20'),
(39, 9, '2023-04-01')
SELECT * FROM Enrollments

INSERT INTO Payments (student_id, amount, payment_date)
VALUES
(30, 100.00, '2023-01-25'),
(31, 75.00, '2023-02-05'),
(32, 120.00, '2023-02-15'),
(33, 90.00, '2023-03-01'),
(34, 110.00, '2023-03-10'),
(35, 80.00, '2023-03-15'),
(36, 150.00, '2023-04-01'),
(37, 95.00, '2023-04-10'),
(38, 130.00, '2023-04-15'),
(39, 70.00, '2023-05-01')
SELECT * FROM Payments 

--B)QUERY
--1. Write an SQL query to insert a new student into the "Students" table with the following details:a. First Name: Johnb. Last Name: Doe c. Date of Birth: 1995-08-15 d. Email: john.doe@example.com e. Phone Number: 1234567890
INSERT INTO Students (first_name, last_name, date_of_birth, email, phone_number)
VALUES ('John', 'Doe', '1998-08-15', 'john.doe@example.com', '1234567890')


--2. Write an SQL query to enroll a student in a course. Choose an existing student and course and insert a record into the "Enrollments" table with the enrollment date.
INSERT INTO Enrollments (student_id, course_id, enrollment_date)
VALUES (40, 2, '2023-01-01')


--3. Update the email address of a specific teacher in the "Teacher" table. Choose any teacher and modify their email address.
UPDATE Teacher SET email = 'john.email@example.com' WHERE teacher_id = 1


--4. Write an SQL query to delete a specific enrollment record from the "Enrollments" table. Select an enrollment record based on student and course.
DELETE FROM Enrollments WHERE student_id = 36 AND course_id = 4
SELECT * FROM Enrollments
SELECT * FROM Students
SELECT * FROM Payments

--5. Update the "Courses" table to assign a specific teacher to a course. Choose any course and teacher from the respective tables.
UPDATE Courses SET teacher_id = 2 WHERE course_id = 2
SELECT * FROM Teacher
SELECT * FROM COURSES

--6. Delete a specific student from the "Students" table and remove all their enrollment records from the "Enrollments" table. Be sure to maintain referential integrity.
DELETE FROM Students WHERE student_id = 31
DELETE FROM Payments WHERE student_id = 31
DELETE FROM Enrollments WHERE student_id = 31


--7. Update the payment amount for a specific payment record in the "Payments" table. Choose any payment record and modify the payment amount.
UPDATE Payments SET amount=170.00 WHERE student_id=32

--JOINS
--1. Write an SQL query to calculate the total payments made by a specific student. You will need to join the "Payments" table with the "Students" table based on the student's ID.
SELECT S.student_id,SUM(P.amount) AS TOTALPAYMENT
FROM students S 
JOIN payments P
ON S.student_id=P.student_id
GROUP BY S.student_id



--2. Write an SQL query to retrieve a list of courses along with the count of students enrolled in each course. Use a JOIN operation between the "Courses" table and the "Enrollments" table.
SELECT * FROM Courses
SELECT * FROM Enrollments

SELECT C.course_id, C.course_name,COUNT(E.course_id) AS NOOFENROLLMENT
FROM Courses C 
JOIN Enrollments E 
ON C.course_id=E.course_id
GROUP BY C.course_id,C.course_name



--3. Write an SQL query to find the names of students who have not enrolled in any course. Use a LEFT JOIN between the "Students" table and the "Enrollments" table to identify students without enrollments.
INSERT INTO Students VALUES('Steve','Smith','2000-11-05','STEVE.SMITH@GMAIL.COM',9999777765)

SELECT * FROM Students

SELECT S.student_id,CONCAT(first_name,' ', last_name) AS Name
FROM Students S
LEFT JOIN Enrollments E
ON S.student_id=E.student_id
WHERE E.student_id IS NULL


--4. Write an SQL query to retrieve the first name, last name of students, and the names of the courses they are enrolled in. Use JOIN operations between the "Students" table and the "Enrollments" and "Courses" tables.
SELECT S.student_id,CONCAT(first_name,' ', last_name) AS Name,C.course_name
FROM Students S
JOIN Enrollments E ON S.student_id=E.student_id
JOIN Courses C ON E.course_id=C.course_id



--5. Create a query to list the names of teachers and the courses they are assigned to. Join the "Teacher" table with the "Courses" table.
SELECT * FROM Teacher
SELECT * FROM Courses

SELECT T.teacher_id,CONCAT(first_name,' ', last_name) AS Name,C.course_name
FROM Teacher T
JOIN Courses C 
ON T.teacher_id=C.teacher_id


--6. Retrieve a list of students and their enrollment dates for a specific course. You'll need to join the "Students" table with the "Enrollments" and "Courses" tables.
SELECT * FROM Students
SELECT * FROM Enrollments

SELECT S.student_id,CONCAT(first_name,' ', last_name) AS Name, E.enrollment_date
FROM Students S 
JOIN Enrollments E
ON S.student_id=E.student_id



--7. Find the names of students who have not made any payments. Use a LEFT JOIN between the "Students" table and the "Payments" table and filter for students with NULL payment records.
SELECT S.student_id,CONCAT(first_name,' ', last_name) AS Name
FROM Students S
LEFT JOIN Payments P 
ON S.student_id=P.student_id
WHERE P.payment_id IS NULL



--8. Write a query to identify courses that have no enrollments. You'll need to use a LEFT JOIN between the "Courses" table and the "Enrollments" table and filter for courses with NULL enrollment records.
SELECT C.course_id, C.course_name
FROM Courses C
LEFT JOIN Enrollments E
ON C.course_id=E.course_id
WHERE E.course_id IS NULL



--9. Identify students who are enrolled in more than one course. Use a self-join on the "Enrollments" table to find students with multiple enrollment records.
SELECT DISTINCT e1.student_id,s.first_name,s.last_name
FROM Enrollments e1
JOIN Enrollments e2 ON e1.student_id = e2.student_id AND e1.course_id <> e2.course_id
JOIN Students s ON e1.student_id = s.student_id



--10. Find teachers who are not assigned to any courses. Use a LEFT JOIN between the "Teacher" table and the "Courses" table and filter for teachers with NULL course assignments.
SELECT T.teacher_id,CONCAT(first_name,' ', last_name) AS TeacherName
FROM Teacher T
LEFT JOIN Courses C 
ON T.teacher_id=C.teacher_id
WHERE C.teacher_id IS NULL



--Aggregate Functions and Subqueries:

--1. Write an SQL query to calculate the average number of students enrolled in each course. Use aggregate functions and subqueries to achieve this.
SELECT course_id,course_name,
COALESCE((SELECT AVG(enrollment_count) FROM (SELECT COUNT(student_id) AS enrollment_count
            FROM Enrollments
            WHERE course_id = Courses.course_id
            GROUP BY student_id) AS CourseEnrollments), 0) AS average_students_enrolled
FROM Courses


--2. Identify the student(s) who made the highest payment. Use a subquery to find the maximum payment amount and then retrieve the student(s) associated with that amount.
SELECT student_id,first_name,last_name
FROM Students
WHERE student_id IN (SELECT student_id FROM Payments WHERE amount = (SELECT MAX(amount) FROM Payments))


--3. Retrieve a list of courses with the highest number of enrollments. Use subqueries to find the course(s) with the maximum enrollment count.
SELECT course_id,course_name,COALESCE((SELECT TOP 1 COUNT(*) FROM Enrollments E WHERE C.course_id = E.course_id GROUP BY student_id ORDER BY COUNT(*) DESC), 0) AS max_enrollment_count
FROM Courses C


--4. Calculate the total payments made to courses taught by each teacher. Use subqueries to sum payments for each teacher's courses.
SELECT teacher_id,first_name,last_name,
COALESCE((SELECT SUM(P.amount)
        FROM Payments P, Enrollments E, Courses C
        WHERE P.student_id = E.student_id
            AND E.course_id = C.course_id
            AND C.teacher_id = T.teacher_id), 0) AS total_payments
FROM Teacher T

--5. Identify students who are enrolled in all available courses. Use subqueries to compare a student's enrollments with the total number of courses.
SELECT S.student_id, S.first_name, S.last_name
FROM Students S
WHERE (SELECT COUNT(DISTINCT E.course_id) FROM Enrollments E WHERE E.student_id = S.student_id)
= (SELECT COUNT(DISTINCT course_id) 
FROM Courses)


--6. Retrieve the names of teachers who have not been assigned to any courses. Use subqueries to find teachers with no course assignments.
SELECT teacher_id,first_name,last_name
FROM Teacher T
WHERE teacher_id NOT IN (SELECT DISTINCT teacher_id FROM Courses WHERE teacher_id IS NOT NULL)



--7. Calculate the average age of all students. Use subqueries to calculate the age of each student based on their date of birth.
SELECT AVG(age) AS average_age
FROM ( SELECT DATEDIFF(YEAR,date_of_birth,GETDATE()) AS age FROM Students) AS student_age


--8. Identify courses with no enrollments. Use subqueries to find courses without enrollment records.
SELECT course_id,course_name
FROM Courses C
WHERE course_id NOT IN (SELECT DISTINCT course_id FROM Enrollments WHERE course_id IS NOT NULL)


--9. Calculate the total payments made by each student for each course they are enrlled in. Use subqueries and aggregate functions to sum payments.
SELECT student_id,course_id,COALESCE((SELECT SUM(amount) FROM Payments 
    WHERE student_id = Enrollments.student_id
    AND course_id = Enrollments.course_id), 0) AS total_payments
FROM Enrollments



--10. Identify students who have made more than one payment. Use subqueries and aggregate functions to count payments per student and filter for those with counts greater than one.
SELECT student_id,first_name,last_name
FROM Students
WHERE student_id IN (SELECT student_id FROM Payments
GROUP BY student_id
HAVING COUNT(payment_id) > 1)



--11. Write an SQL query to calculate the total payments made by each student. Join the "Students" table with the "Payments" table and use GROUP BY to calculate the sum of payments for each student.
SELECT student_id,first_name,last_name,(SELECT COALESCE(SUM(amount), 0) FROM Payments WHERE student_id = Students.student_id) AS total_payments
FROM Students


--12. Retrieve a list of course names along with the count of students enrolled in each course. Use JOIN operations between the "Courses" table and the "Enrollments" table and GROUP BY to count enrollments.
SELECT course_id, course_name,(SELECT COUNT(student_id) FROM Enrollments WHERE course_id = Courses.course_id) AS student_count
FROM Courses


--13. Calculate the average payment amount made by students. Use JOIN operations between the "Students" table and the "Payments" table and GROUP BY to calculate the average.
SELECT student_id,first_name,last_name,(SELECT COALESCE(AVG(amount), 0) FROM Payments WHERE student_id = Students.student_id) AS average_payment_amount
FROM Students

