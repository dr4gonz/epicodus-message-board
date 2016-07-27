using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class OriginalPostTest : IDisposable
  {
    DateTime testDate = new DateTime(2016, 7, 22);

    public OriginalPostTest()
    {
      // DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
      DBConfiguration.ConnectionString = "Data Source=DESKTOP-7OLC9FT\\SQLEXPRESS;Initial Catalog=message_board_test;Integrated Security=SSPI;";
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
      OriginalPost firstOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      OriginalPost secondOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      //Act
      OriginalPost.UpdateById("Fishing at the lake", "I like to fish at the lake", testOriginalPost.GetId());
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetAuthor();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Update_UpdatesOriginalPostInDatabaseByReference()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      //Act
      testOriginalPost.Update("Fishing at the lake", "I like to fish at the lake");
      string expectedResult = "Fishing at the lake";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetAuthor();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_Remove_RedactsInDatabase()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      //Act
      testOriginalPost.Remove();
      string expectedResult = "[removed]";
      string result = OriginalPost.Find(testOriginalPost.GetId()).GetAuthor();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void OriginalPost_GetAllChildComments_ReturnsComment()
    {
      //Arrange
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      Comment testComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId(), testDate, 4);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId(), testDate, 4);
      firstComment.Save();
      Comment secondComment = new Comment("Henry", "This stuff is just okay", 5, testOriginalPost.GetId(), testDate, 4);
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
      OriginalPost testOriginalPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testOriginalPost.Save();
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0, testOriginalPost.GetId(), testDate, 4);
      firstComment.Save();
      Comment secondComment = new Comment("Henry", "This stuff is just okay", 5, (testOriginalPost.GetId()+1), testDate, 4);
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
      OriginalPost testPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      testPost.Save();
      Comment firstComment = new Comment("Matt", "First", 0, testPost.GetId(), testDate, 4);
      firstComment.Save();
      Comment secondComment = new Comment("Matt", "Second", 0, testPost.GetId()+1, testDate, 4);
      secondComment.Save();
      List<Comment> expectedResult = new List<Comment>{secondComment};
      //Act
      testPost.DeleteAllChildren();
      List<Comment> result = Comment.GetAll();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_SearchByKeyword_SearchesByTitleKeyword()
    {
      //Arrange
      OriginalPost firstPost = new OriginalPost("Bob", "Fishing", "I like to fish", 0, testDate, 1);
      firstPost.Save();
      OriginalPost secondPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 1);
      secondPost.Save();
      //Act
      List<OriginalPost> foundPosts = OriginalPost.SearchByKeyword("Fishing");
      List<OriginalPost> expectedResults = new List<OriginalPost>{firstPost};
      //Assert
      Assert.Equal(expectedResults, foundPosts);
    }


    [Fact]
    public void Post_SearchByKeyword_SearchesByTextKeyword()
    {
      //Arrange
      OriginalPost firstPost = new OriginalPost("Bob", "Fishing", "I like the outdoors", 0, testDate, 1);
      firstPost.Save();
      OriginalPost secondPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 2);
      secondPost.Save();
      //Act
      List<OriginalPost> foundPosts = OriginalPost.SearchByKeyword("outdoors");
      List<OriginalPost> expectedResults = new List<OriginalPost>{firstPost};
      //Assert
      Assert.Equal(expectedResults, foundPosts);
    }

    [Fact]
    public void Post_Upvote_Adds1ToPostRating()
    {
      OriginalPost newOriginalPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 1);
      newOriginalPost.Save();
      User firstUser = new User("Bob", "password");
      firstUser.Save();
      //Act
      int expectedResult = 1;
      newOriginalPost.Upvote(firstUser.GetId());
      int result = newOriginalPost.GetRating();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_Downvote_Subtracts1ToPostRating()
    {
      OriginalPost newOriginalPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 1);
      newOriginalPost.Save();
      User firstUser = new User("Bob", "password");
      firstUser.Save();
      //Act
      int expectedResult = -1;
      newOriginalPost.Downvote(firstUser.GetId());
      int result = newOriginalPost.GetRating();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_Vote_SavesVoteInDatabase()
    {
      //Arrange
      OriginalPost newOriginalPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 1);
      newOriginalPost.Save();
      User firstUser = new User("Bob", "password");
      firstUser.Save();
      User secondUser = new User("John", "password");
      secondUser.Save();
      int expectedResult = 2;
      //Act
      newOriginalPost.Vote(firstUser.GetId(), 1);
      newOriginalPost.Vote(secondUser.GetId(), 1);
      int result = newOriginalPost.GetRating();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Post_Vote_UserCanOnlyVoteOnce()
    {
      //Arrange
      OriginalPost newOriginalPost = new OriginalPost("Joe", "Swimming", "I like to swim", 0, testDate, 1);
      newOriginalPost.Save();
      User firstUser = new User("Bob", "password");
      firstUser.Save();
      User secondUser = new User("John", "password");
      secondUser.Save();
      int expectedResult = 2;
      //Act
      newOriginalPost.Vote(firstUser.GetId(), 1);
      newOriginalPost.Vote(secondUser.GetId(), 1);
      newOriginalPost.Vote(secondUser.GetId(), 1);
      int result = newOriginalPost.GetRating();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
