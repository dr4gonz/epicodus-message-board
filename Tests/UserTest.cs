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
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
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
  }
}
