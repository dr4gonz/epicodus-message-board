using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class PostTest : IDisposable
  {
    public PostTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Post.DeleteAll();
    }

    [Fact]
    public void Post_Equals_TrueIfPostsSame()
    {
      //Arrange, act
      Post firstPost = new Post("Bob", "Fishing", "I like to fish");
      Post secondPost = new Post("Bob", "Fishing", "I like to fish");
      //Assert
      Assert.Equal(firstPost, secondPost);
    }

    [Fact]
    public void Post_DatabaseEmptyAtFirst()
    {
      //Arrange, act
      int result = Post.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Post_Save_SavesPostToDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      //Act
      testPost.Save();
      Post foundPost = Post.GetAll()[0];
      //Assert
      Assert.Equal(testPost, foundPost);
    }

    [Fact]
    public void Post_Find_FindsPostInDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      Post foundPost = Post.Find(testPost.GetId());
      //Assert
      Assert.Equal(testPost, foundPost);
    }

    [Fact]
    public void Post_DeleteById_DeletesPostFromDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      Post.DeleteById(testPost.GetId());
      int result = Post.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Post_Delete_DeletesPostFromDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      testPost.Delete();
      int result = Post.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Post_Update_UpdatesPostInDatabaseById()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      Post.UpdateById("Bob", "Fishing at the lake", "I like to fish at the lake", testPost.GetId());
      string expectedResult = "Fishing at the lake";
      string result = Post.Find(testPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_Update_UpdatesPostInDatabaseByReference()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      testPost.Update("Fishing at the lake", "I like to fish at the lake");
      string expectedResult = "Fishing at the lake";
      string result = Post.Find(testPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_Remove_RedactsInDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      testPost.Remove();
      string expectedResult = "[removed]";
      string result = Post.Find(testPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_GetAllChildComments_ReturnsComment()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      Comment testComment = new Comment("Matt", "This stuff is really cool!", 0, testPost.GetId());
      testComment.Save();
      List<Comment> expectedResult = new List<Comment>{testComment};
      //Act
      List<Comment> result = testPost.GetAllChildComments();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_GetAllChildCommentsSortedByRating_ReturnsOrderedComments()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0, testPost.GetId());
      firstComment.Save();
      Comment secondComment = new Comment("Henry", "This stuff is just okay", 5, testPost.GetId());
      secondComment.Save();
      List<Comment> expectedResult = new List<Comment>{secondComment, firstComment};
      //Act
      List<Comment> result = testPost.GetAllChildComments("rating");
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }




}
