
USE ZustDb

SELECT * FROM AspNetUsers 
SELECT A.UserName , A.IsOnline FROM AspNetUsers AS A


--  DELETE FROM AspNetUsers   --WHERE UserName = ''


-- (localdb)\MSSQLLocalDB    <- Server Name



---- Post Start -----

SELECT * FROM Posts
SELECT * FROM Tags
SELECT * FROM PostTags
SELECT * FROM Comments
SELECT * FROM ReplyToComments
SELECT * FROM Likes
SELECT * FROM FriendshipRequests
SELECT * FROM UserFriends
SELECT * FROM [Messages]

--  DELETE FROM Posts
--  DELETE FROM Tags
--  DELETE FROM PostTags
--  DELETE FROM Likes
--  DELETE FROM FriendshipRequests
--  DELETE FROM UserFriends
--  DELETE FROM Messages

--  UPDATE AspNetUsers SET FirstName = 'Test_000'  WHERE Id ='f14ae9c0-53ff-4126-bc6a-756c4f7f49ef'
--  UPDATE AspNetUsers SET IsOnline = 0 

----  Post End  -----


 