using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class CommentTest : IDisposable
  {
    public CommentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
      // DBConfiguration.ConnectionString = "Data Source=DESKTOP-7OLC9FT\\SQLEXPRESS;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Comment.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Comment.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_ChecksIfCommentsAreEqual()
    {
      //Arrange, Act
      Comment firstComment = new Comment("Matt", "This stuff is really cool!", 0);
      Comment secondComment = new Comment("Matt", "This stuff is really cool!", 0);
      //Assert
      Assert.Equal(firstComment, secondComment);
    }


    [Fact]
   public void Comment_SavesToDatabase()
   {
     //Arrange
     Comment newComment = new Comment("Matt", "This stuff is really cool!", 0);
     newComment.Save();
     //Act
     List<Comment> result = Comment.GetAll();
     List<Comment> testList = new List<Comment>{newComment};
     //Assert
     Assert.Equal(testList, result);
   }

   [Fact]
   public void Comment_SavesSavesWithID()
   {
     //Arrange
     Comment newComment = new Comment("Matt", "This stuff is really cool!", 0);
     newComment.Save();
     //Act
     Comment savedComment = Comment.GetAll()[0];
     int result = newComment.GetId();
     int testId = savedComment.GetId();
     //Assert
     Assert.Equal(testId, result);
   }
   [Fact]
    public void Comment_FindsCommentInDatabase()
    {
      //Arrange
      Comment newComment = new Comment("Matt", "This stuff is really cool!", 0);
      newComment.Save();
      //Act
      Comment foundComment = Comment.Find(newComment.GetId());
      //Assert
      Assert.Equal(newComment, foundComment);
    }

    [Fact]
    public void Comment_UpdateUpdatesCommentInDatabase()
    {
      //Arrange
      Comment newComment = new Comment("Matt", "This stuff is really cool!", 0);
      newComment.Save();
      string newMainText = "This stuff is really cool";
      //Act
      newComment.Update(newMainText);
      string updateMainText = newComment.GetMainText();
      //Assert
      Assert.Equal(newMainText, updateMainText);
    }

    [Fact]
    public void Comment_Delete_DeletesCommentFromDatabase()
    {
      //Arrange
      string mainText1 = "This stuff is really cool";
      Comment testComment1 = new Comment("Matt", mainText1, 0);
      testComment1.Save();

      string mainText2 = "This stuff is lame";
      Comment testComment2 = new Comment("Matt", mainText2, 0);
      testComment2.Save();
      //Act
      testComment2.Delete();
      List<Comment> resultComment = Comment.GetAll();
      List<Comment> testComment = new List<Comment> {testComment1};
      //Assert
      Assert.Equal(testComment, resultComment);
    }
    [Fact]
    public void Comment_Remove_RemovesMainTextInDatabase()
    {
      string mainText = "This stuff is really cool";
      Comment testComment = new Comment("Matt", mainText, 0);
      testComment.Save();
      //Act
      testComment.Remove();
      string testText = "[Removed]";
      string resultText = testComment.GetMainText();
      //Assert
      Assert.Equal(testText, resultText);
    }
    [Fact]
    public void Comment_SetParentId_SetsParentId()
    {
      //Arrange
      Comment newComment = new Comment("Matt", "This stuff is really cool!", 0);
      newComment.Save();
      //Act
      newComment.SetParentId(4);
      int result = newComment.GetParentId();
      //Assert
      Assert.Equal(4, result);
    }
  }
}
