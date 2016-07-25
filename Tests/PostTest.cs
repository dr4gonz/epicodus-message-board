using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class OriginalPostTest : IDisposable
  {
    public OriginalPostTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      OriginalPost.DeleteAll();
    }

    [Fact]
    public void OriginalPost_Equals_TrueIfOriginalPostsSame()
    {
      //Arrange, act
      OriginalPost firstOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      OriginalPost secondOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      //Assert
      Assert.Equal(firstOriginalPost, secondOriginalPost);
    }

    [Fact]
    public void OriginalPost_DatabaseEmptyAtFirst()
    {
      //Arrange, act
      int result = OriginalPost.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OriginalPost_Save_SavesOriginalPostToDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      //Act
      testOriginalPost.Save();
      OriginalPost foundOriginalPost = OriginalPost.GetAll()[0];
      //Assert
      Assert.Equal(testOriginalPost, foundOriginalPost);
    }

    [Fact]
    public void OriginalPost_Find_FindsOriginalPostInDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      OriginalPost foundOriginalPost = OriginalPost.Find(testOriginalPost.GetId());
      //Assert
      Assert.Equal(testOriginalPost, foundOriginalPost);
    }

    [Fact]
    public void OriginalPost_DeleteById_DeletesOriginalPostFromDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      OriginalPost.DeleteById(testOriginalPost.GetId());
      int result = OriginalPost.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OriginalPost_Delete_DeletesOriginalPostFromDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      testOriginalPost.Delete();
      int result = OriginalPost.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OriginalPost_Update_UpdatesOriginalPostInDatabaseById()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      OriginalPost.UpdateById("Bob", "Fishing at the lake", "I like to fish at the lake", testOriginalPost.GetId());
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Update_UpdatesOriginalPostInDatabaseByReference()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      testOriginalPost.Update("Fishing at the lake", "I like to fish at the lake");
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Remove_RedactsInDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      testOriginalPost.Save();
      //Act
      testOriginalPost.Remove();
      string expectedResult = "[removed]";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);    }
  }
}
