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
      Comment.DeleteAll();
    }

    [Fact]
    public void OriginalPost_Equals_TrueIfOriginalPostsSame()
    {
      //Arrange, act
      OriginalPost firstOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      OriginalPost secondOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      //Act
      OriginalPost.UpdateById("Bob", "Fishing at the lake", "I like to fish at the lake", testOriginalPost.GetId(), DateTime.Now);
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Update_UpdatesOriginalPostInDatabaseByReference()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      //Act
      testOriginalPost.Update("Fishing at the lake", "I like to fish at the lake", DateTime.Now);
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Remove_RedactsInDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      //Act
      testOriginalPost.Remove();
      string expectedResult = "[removed]";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetTitle();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_GetAllChildComments_ReturnsComment()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      Comment testComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId());
      testComment.Save();
      List<Comment> expectedResult = new List<Comment>{testComment};
      //Act
      List<Comment> result = testOriginalPost.GetAllChildComments();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_GetAllChildCommentsSortedByRating_ReturnsOrderedComments()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId());
      firstComment.Save();
      Comment secondComment = new Comment("Henry", "This stuff is just okay", 5, testOriginalPost.GetId());
      secondComment.Save();
      List<Comment> expectedResult = new List<Comment>{secondComment, firstComment};
      //Act
      List<Comment> result = testOriginalPost.GetAllChildComments("rating");
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_GetAllDirectChildren_ReturnsComment()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testOriginalPost.Save();
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId(), DateTime.Now);
      firstComment.Save();
      Comment secondComment = new Comment("Henry", "This stuff is just okay", 5, (testOriginalPost.GetId()+1));
      secondComment.Save();
      List<Comment> expectedResult = new List<Comment>{firstComment};
      //Act
      List<Comment> result = testOriginalPost.GetAllChildComments();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void OriginalPost_DeleteChildrenDeletesFromDatabase()
    {
      OriginalPost testPost = new OriginalPost("Bob", "Fishing", "I like to fish", DateTime.Now);
      testPost.Save();
      Comment firstComment = new Comment("Matt", "First", 0, testPost.GetId());
      firstComment.Save();
      Comment secondComment = new Comment("Matt", "Second", 0, testPost.GetId()+1);
      secondComment.Save();
      List<Comment> expectedResult = new List<Comment>{secondComment};
      //Act
      testPost.DeleteAllChildren();
      List<Comment> result = Comment.GetAll();
      //Assert
      Assert.Equal(expectedResult, result);
    }
<<<<<<< HEAD

    [Fact]
    public void Post_SearchByKeyword_SearchesByTitleKeyword()
    {
      //Arrange
      Post firstPost = new Post("Bob", "Fishing", "I like to fish", DateTime.Now);
      firstPost.Save();
      Post secondPost = new Post("Joe", "Swimming", "I like to swim", DateTime.Now);
      secondPost.Save();
      //Act
      List<Post> foundPosts = Post.SearchByKeyword("Fishing");
      List<Post> expectedResults = new List<Post>{firstPost};
      //Assert
      Assert.Equal(expectedResults, foundPosts);
    }
=======
>>>>>>> master

    [Fact]
    public void Post_SearchByKeyword_SearchesByTextKeyword()
    {
      //Arrange
      Post firstPost = new Post("Bob", "Fishing", "I like the outdoors", DateTime.Now);
      firstPost.Save();
      Post secondPost = new Post("Joe", "Swimming", "I like to swim", DateTime.Now);
      secondPost.Save();
      //Act
      List<Post> foundPosts = Post.SearchByKeyword("outdoors");
      List<Post> expectedResults = new List<Post>{firstPost};
      //Assert
      Assert.Equal(expectedResults, foundPosts);
    }

  }
}
