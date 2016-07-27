using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class UserTest : IDisposable
  {
    DateTime testDate = new DateTime(2016, 7, 22);
    public UserTest()
    {
      // DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
      DBConfiguration.ConnectionString = "Data Source=DESKTOP-7OLC9FT\\SQLEXPRESS;Initial Catalog=message_board_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      // User.DeleteAll();
      // OriginalPost.DeleteAll();
      // Comment.DeleteAll();
      User.DeleteAll();
    }

    [Fact]
    public void User_Equals_TrueIfUsersSame()
    {
      //Arrange, act
      User firstUser = new User("Homer J. Simpson", "230sfd");
      User secondUser = new User("Homer J. Simpson", "230sfd");
      //Assert
      Assert.Equal(firstUser, secondUser);
    }

    [Fact]
    public void User_DatabaseEmptyAtFirst()
    {
      //Arrange, act
      int result = User.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void User_Save_SavesUserToDatabase()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      //Act
      testUser.Save();
      User foundUser = User.GetAll()[0];
      //Assert
      Assert.Equal(testUser, foundUser);
    }

    [Fact]
    public void User_Find_FindsUserInDatabase()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      testUser.Save();
      //Act
      User foundUser = User.Find(testUser.GetId());
      //Assert
      Assert.Equal(testUser, foundUser);
    }
    [Fact]
    public void User_Find_FindsUserInDatabaseByName()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      testUser.Save();
      //Act
      User foundUser = User.Find(testUser.GetName());
      //Assert
      Assert.Equal(testUser, foundUser);
    }
    [Fact]
    public void User_Delete_DeletesUserFromDatabase()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      testUser.Save();
      //Act
      testUser.Delete();
      int result = User.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void User_GetComments_GetsAllOfUsersComments()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      testUser.Save();
      Comment newComment = new Comment(testUser.GetName(), "This stuff is really cool!", 0, 1, testDate, testUser.GetId());
      newComment.Save();
      //Act
      List<Comment> expectedResult = new List<Comment> {newComment};
      List<Comment> result = testUser.GetComments();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void User_GetOriginalPosts_GetsAllOfUsersPost()
    {
      //Arrange
      User testUser = new User("Homer J. Simpson", "230sfd");
      testUser.Save();
      OriginalPost newOriginalPost = new OriginalPost(testUser.GetName(), "Fishing", "I like to fish", 0, testDate, testUser.GetId());
      newOriginalPost.Save();
      //Act
      List<OriginalPost> expectedResult = new List<OriginalPost> {newOriginalPost};
      List<OriginalPost> result = testUser.GetOriginalPosts();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void User_ValidateUser_ValidatesUserAgainstDatabase()
    {
      //Arrange
      User testUser = new User("Homer", "230sfd");
      testUser.Save();
      //Act
      User result = User.ValidateUserLogin("Homer", "230sfd");
      //Assert
      Assert.Equal(testUser, result);
    }
  }
}
