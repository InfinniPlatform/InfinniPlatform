using System;
using System.Collections.Generic;

namespace InfinniPlatform.Index.ElasticSearch.Tests.Builders
{
    /// <summary>
    ///     Создание набора школ для проверки различных видов запросов
    /// </summary>
    public class SchoolsFactory
    {
        /// <summary>
        ///     Создает определенные школы, необходимые для
        ///     проведения юнит-тестов
        /// </summary>
        public static IEnumerable<School> CreateSchools()
        {
            var principal1 = new Principal
                {
                    Name = "Ivan",
                    LastName = "Belyaev",
                    BirthDate = new DateTime(1960, 5, 5),
                    Grade = 3,
                    KnowledgeRating = 3.5
                };

            var principal2 = new Principal
                {
                    Name = "Mike",
                    LastName = "Nikitin",
                    BirthDate = new DateTime(1970, 2, 9),
                    Grade = 6,
                    KnowledgeRating = 3.9
                };

            var principal3 = new Principal
                {
                    Name = "",
                    LastName = "Monakhov",
                    BirthDate = new DateTime(1980, 11, 20),
                    Grade = 9,
                    KnowledgeRating = 4.3
                };

            var student1 = new Student
                {
                    Name = "ivan",
                    LastName = "Matveev",
                    BirthDate = new DateTime(1985, 7, 28),
                    FavoriteSubject = "Literature",
                    KnowledgeRating = 3.5,
                    CountOfFriends = 5
                };

            var student2 = new Student
                {
                    Name = "Oleg",
                    LastName = "Ivanov",
                    BirthDate = new DateTime(1990, 2, 8),
                    FavoriteSubject = "English",
                    KnowledgeRating = 4.0,
                    CountOfFriends = 10
                };

            var student3 = new Student
                {
                    Name = "Roman",
                    LastName = "Fomin",
                    BirthDate = new DateTime(1995, 10, 1),
                    FavoriteSubject = "Biology",
                    KnowledgeRating = 4.5,
                    CountOfFriends = 15
                };

            var school1 = new School
                {
                    FoundationDate = new DateTime(1980, 9, 1),
                    Street = "Lenina-Avenue",
                    HouseNumber = 41,
                    Name = "Very good school",
                    Rating = 4.1,
                    Principal = principal1,
                    Students = {student1, student2}
                };

            var school2 = new School
                {
                    FoundationDate = new DateTime(1988, 10, 1),
                    Street = "Kirova",
                    HouseNumber = 31,
                    Name = "Nice school",
                    Rating = 3.1,
                    Principal = principal2,
                    Students = {student1, student3}
                };

            var school3 = new School
                {
                    FoundationDate = new DateTime(1999, 1, 1),
                    Street = "Far away",
                    HouseNumber = 21,
                    Name = "Bad school",
                    Rating = 2.1,
                    Principal = principal3,
                    Students = {student1, student2, student3}
                };

            return new[] {school1, school2, school3};
        }

        /// <summary>
        ///     Создает определенные школы, необходимые для
        ///     проведения юнит-тестов
        /// </summary>
        public static IEnumerable<School> CreateSchoolsForFacetsTesting()
        {
            var principal1 = new Principal
                {
                    Name = "Ivan",
                    LastName = "Belyaev",
                    BirthDate = new DateTime(1960, 5, 5),
                    Grade = 3,
                    KnowledgeRating = 3.5
                };

            var principal2 = new Principal
                {
                    Name = "Mike",
                    LastName = "Nikitin",
                    BirthDate = new DateTime(1970, 2, 9),
                    Grade = 6,
                    KnowledgeRating = 3.9
                };

            var principal3 = new Principal
                {
                    Name = "Maxim",
                    LastName = "Monakhov",
                    BirthDate = new DateTime(1980, 11, 20),
                    Grade = 9,
                    KnowledgeRating = 4.3
                };

            var student1 = new Student
                {
                    Name = "ivan",
                    LastName = "Matveev",
                    BirthDate = new DateTime(1985, 7, 28),
                    FavoriteSubject = "Literature",
                    KnowledgeRating = 3.5,
                    CountOfFriends = 5
                };

            var student2 = new Student
                {
                    Name = "Oleg",
                    LastName = "Ivanov",
                    BirthDate = new DateTime(1990, 2, 8),
                    FavoriteSubject = "English",
                    KnowledgeRating = 4.0,
                    CountOfFriends = 10
                };

            var student3 = new Student
                {
                    Name = "Roman",
                    LastName = "Fomin",
                    BirthDate = new DateTime(1995, 10, 1),
                    FavoriteSubject = "Biology",
                    KnowledgeRating = 4.5,
                    CountOfFriends = 15
                };

            var school1 = new School
                {
                    FoundationDate = new DateTime(1980, 9, 1),
                    Street = "LeninaAvenue",
                    HouseNumber = 41,
                    Name = "Verygoodschool",
                    Rating = 4.1,
                    Principal = principal1,
                    Students = {student1, student2}
                };

            var school2 = new School
                {
                    FoundationDate = new DateTime(1988, 10, 1),
                    Street = "Kirova",
                    HouseNumber = 31,
                    Name = "Niceschool",
                    Rating = 3.1,
                    Principal = principal2,
                    Students = {student1, student3}
                };

            var school3 = new School
                {
                    FoundationDate = new DateTime(1999, 1, 1),
                    Street = "Faraway",
                    HouseNumber = 21,
                    Name = "Badschool",
                    Rating = 2.1,
                    Principal = principal3,
                    Students = {student1, student2, student3}
                };

            var school4 = new School
                {
                    FoundationDate = new DateTime(1980, 9, 1),
                    Street = "LeninaAvenue",
                    HouseNumber = 41,
                    Name = "simpleschool",
                    Rating = 4.2,
                    Principal = principal1,
                    Students = {student1, student2}
                };

            var school5 = new School
                {
                    FoundationDate = new DateTime(1988, 10, 1),
                    Street = "Somestreet",
                    HouseNumber = 31,
                    Name = "Niceschool",
                    Rating = 3.1,
                    Principal = principal2,
                    Students = {student1, student3}
                };

            var school6 = new School
                {
                    FoundationDate = new DateTime(1999, 1, 1),
                    Street = "31",
                    HouseNumber = 21,
                    Name = "Badschool",
                    Rating = 2.1,
                    Principal = principal3,
                    Students = {student1, student2, student3}
                };

            return new[] {school1, school2, school3, school4, school5, school6};
        }

        /// <summary>
        ///     Создаёт 1000 школ со случайными значениями полей
        /// </summary>
        public static IEnumerable<School> Create1000RandomSchools()
        {
            // Будем генерировать 1000 школ
            // и соответственно 1000 директоров.
            // Учеников будет 1000 и в каждой 
            // школе будет 10 случайно выбранных учеников

            var r = new Random();

            var students = new List<Student>();
            for (int i = 0; i < 1000; i++)
            {
                var student = new Student
                    {
                        Name = "Name" + i,
                        LastName = "LastName" + i%7,
                        BirthDate = new DateTime(r.Next(1980, 2000), r.Next(1, 12), r.Next(1, 28)),
                        FavoriteSubject = "Subject" + i,
                        KnowledgeRating = r.NextDouble()*5,
                        CountOfFriends = r.Next(1, 20)
                    };

                students.Add(student);
            }

            var principals = new List<Principal>();
            var schools = new List<School>();
            for (int i = 0; i < 1000; i++)
            {
                var principal = new Principal
                    {
                        Name = "PrincipalName" + i,
                        LastName = "PrincipalLastName" + i%5,
                        BirthDate = new DateTime(r.Next(1950, 1980), r.Next(1, 12), r.Next(1, 28)),
                        KnowledgeRating = r.NextDouble()*5,
                        Grade = r.Next(1, 5)
                    };
                principals.Add(principal);


                var school = new School
                    {
                        FoundationDate = new DateTime(r.Next(1980, 2000), r.Next(1, 12), r.Next(1, 28)),
                        Street = "Street" + i,
                        HouseNumber = r.Next(1, 200),
                        Name = "SchoolName" + i%3,
                        Rating = r.NextDouble()*5,
                        Principal = principals[i],
                    };

                for (int j = 0; j < 10; j++)
                {
                    school.Students.Add(students[r.Next(1000)]);
                }
                schools.Add(school);
            }

            return schools;
        }

        /// <summary>
        ///     Создаёт большое количество школ со случайными значениями полей
        /// </summary>
        public static IEnumerable<School> CreateRandomSchools(int count = 10000)
        {
            // Будем генерировать 100 000 школ
            // и соответственно 100 000 директоров.
            // Учеников будет 10 000 и в каждой 
            // школе будет 10 случайно выбранных учеников

            var r = new Random();

            var students = new List<Student>();
            for (int i = 0; i < count; i++)
            {
                var student = new Student
                    {
                        Name = "Name" + i,
                        LastName = "LastName" + i,
                        BirthDate = new DateTime(r.Next(1980, 2000), r.Next(1, 12), r.Next(1, 28)),
                        FavoriteSubject = "Subject" + i,
                        KnowledgeRating = r.NextDouble()*5,
                        CountOfFriends = r.Next(1, 20)
                    };

                students.Add(student);
            }

            var principals = new List<Principal>();
            var schools = new List<School>();
            for (int i = 0; i < count; i++)
            {
                var principal = new Principal
                    {
                        Name = "PrincipalName" + i,
                        LastName = "PrincipalLastName" + i,
                        BirthDate = new DateTime(r.Next(1950, 1980), r.Next(1, 12), r.Next(1, 28)),
                        KnowledgeRating = r.NextDouble()*5,
                        Grade = r.Next(1, 5)
                    };
                principals.Add(principal);


                var school = new School
                    {
                        FoundationDate = new DateTime(r.Next(1980, 2000), r.Next(1, 12), r.Next(1, 28)),
                        Street = "Street" + i,
                        HouseNumber = r.Next(1, 200),
                        Name = "SchoolName" + i,
                        Rating = r.NextDouble()*5,
                        Principal = principals[i],
                    };

                for (int j = 0; j < 10; j++)
                {
                    school.Students.Add(students[r.Next(10000)]);
                }
                schools.Add(school);
            }

            return schools;
        }
    }

    public class Student
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string FavoriteSubject { get; set; }

        public DateTime BirthDate { get; set; }

        public double KnowledgeRating { get; set; }

        public int CountOfFriends { get; set; }
    }

    public class Principal
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int Grade { get; set; }

        public double KnowledgeRating { get; set; }
    }

    public class School
    {
        public School()
        {
            Students = new List<Student>();
        }

        public string Name { get; set; }

        public string Street { get; set; }

        public int HouseNumber { get; set; }

        public DateTime FoundationDate { get; set; }

        public double Rating { get; set; }

        public Principal Principal { get; set; }

        public IList<Student> Students { get; set; }
    }
}