using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

#nullable disable

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            InsertCourses(migrationBuilder);
            InsertAchievements(migrationBuilder);
            InsertUser(migrationBuilder);
        }

        private static OperationBuilder<SqlOperation> InsertCourses(MigrationBuilder migrationBuilder)
        {
            return migrationBuilder.Sql(@"
                --Insert Swift course
                INSERT INTO course(Id, Name, CreatedAt, UpdatedAt) VALUES(1, 'Swift', datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(1, 'Swift - Chapter 1', 1, 1, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(1, 'Lesson 1', 1, 'Content for Swift - Chapter 1 - lesson 1', 1, datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(2, 'Swift - Chapter 2', 2, 1, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(2, 'Lesson 2', 2, 'Content for Swift - Chapter 2 - lesson 1', 2, datetime(), datetime());

                --Insert Javascript course
                INSERT INTO course(Id, Name, CreatedAt, UpdatedAt) VALUES(2, 'Javascript', datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(3, 'Javascript - Chapter 1', 1, 2, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(3, 'Lesson 1', 1, 'Content for Javascript - Chapter 1 - lesson 1', 3, datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(4, 'Javascript - Chapter 2', 2, 2, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(4, 'Lesson 1', 2, 'Content for Javascript - Chapter 2 - lesson 1', 4, datetime(), datetime());

                --Insert C# course
                INSERT INTO course(Id, Name, CreatedAt, UpdatedAt) VALUES(3, 'C#', datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(5, 'C# - Chapter 1', 1, 3, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(5, 'Lesson 1', 1, 'Content for C# - Chapter 1 - lesson 1', 5, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(6, 'Lesson 2', 2, 'Content for C# - Chapter 1 - lesson 2', 5, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(7, 'Lesson 3', 3, 'Content for C# - Chapter 1 - lesson 3', 5, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(8, 'Lesson 4', 4, 'Content for C# - Chapter 1 - lesson 4', 5, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(9, 'Lesson 5', 5, 'Content for C# - Chapter 1 - lesson 5', 5, datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(6, 'C# - Chapter 2', 2, 3, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(10, 'Lesson 1', 1, 'Content for C# - Chapter 2 - lesson 1', 6, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(11, 'Lesson 2', 2, 'Content for C# - Chapter 2 - lesson 2', 6, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(12, 'Lesson 3', 3, 'Content for C# - Chapter 2 - lesson 3', 6, datetime(), datetime());

                INSERT INTO chapter(Id, Name, DisplayOrder, CourseId, CreatedAt, UpdatedAt) VALUES(7, 'C# - Chapter 3', 3, 3, datetime(), datetime());
                INSERT INTO lesson(Id, Name, DisplayOrder, Content, ChapterId, CreatedAt, UpdatedAt) VALUES(13, 'Lesson 1', 2, 'Content for C# - Chapter 3 - lesson 1', 7, datetime(), datetime());

            ");
        }
        private static OperationBuilder<SqlOperation> InsertAchievements(MigrationBuilder migrationBuilder)
        {
            return migrationBuilder.Sql(@"
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CreatedAt, UpdatedAt) VALUES ('Complete 5 lessons', 5, 0, datetime(), datetime());
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CreatedAt, UpdatedAt) VALUES ('Complete 25 lessons', 25, 0, datetime(), datetime());
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CreatedAt, UpdatedAt) VALUES ('Complete 50 lessons', 50, 0, datetime(), datetime());

                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CreatedAt, UpdatedAt) VALUES ('Complete 1 chapter', 1, 1, datetime(), datetime());
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CreatedAt, UpdatedAt) VALUES ('Complete 5 chapters', 5, 1, datetime(), datetime());


                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CourseId, CreatedAt, UpdatedAt) VALUES ('Complete the Swift course', 1, 2, 1, datetime(), datetime());
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CourseId, CreatedAt, UpdatedAt) VALUES ('Complete the Javascript course', 1, 2, 2, datetime(), datetime());
                INSERT INTO Achievement(Name, ObjectiveGoal, ObjectiveTarget, CourseId, CreatedAt, UpdatedAt) VALUES ('Complete the C# course', 1, 2, 3, datetime(), datetime());
            ");
        }
        private static OperationBuilder<SqlOperation> InsertUser(MigrationBuilder migrationBuilder)
        {
            //TODO : Add achievements here, they must be created at signup
            return migrationBuilder.Sql(@"
                INSERT INTO User(FirstName, LastName, CreatedAt, UpdatedAt) VALUES ('User', 'One', datetime(), datetime());

                --Associated achievements but empty
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Course;
                DELETE FROM Achievement;
                DELETE FROM User;
            ");
        }
    }
}
