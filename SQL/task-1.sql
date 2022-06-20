CREATE TABLE OddUser
(
	oddId int IDENTITY(1,1) primary key,
	
	userId int Not null,

    [id] int Not null ,

    title nvarchar(200) Not null,

    completed bit Not null,

    edited bit Not null

)

CREATE TABLE EvenUser
(
	evenId int IDENTITY(1,1) primary key,
	
	userId int Not null,

    [id] int Not null ,

    title nvarchar(200) Not null,

    completed bit Not null,

    edited bit Not null

)

CREATE TABLE HistoryUser
(
	HistoryId int IDENTITY(1,1) primary key,
	
	userId int Not null,

    [id] int Not null ,

    title nvarchar(200) Not null,

    completed bit Not null,

    edited bit Not null

)


CREATE PROCEDURE AddandEditData
@id int,
@Userid int ,
@Tableid int,
@Title nvarchar(200),
@completed bit
AS
BEGIN
If @Tableid = 2
	BEGIN
		IF EXISTS(select id from EvenUser where id=@id and userId = @Userid)
			BEGIN
				Update EvenUser
				set
				title =@Title,
				completed = @completed,
				edited = 1
				where id = @id
				select id,userId,title,completed,edited , CAST(1 as bit) as edited_via_api from EvenUser where id = @id;
			END
			
		ELSE
			BEGIN
				Insert Into EvenUser(id,userId,title,completed,edited)
				VALUES(@id,@Userid,@Title,@completed,0);
				select id,userId,title,completed,edited,CAST(0 as bit) as edited_via_api from EvenUser where id = @id;
			END
	END
IF @Tableid = 1
	BEGIN
		IF EXISTS(select id from OddUser where id=@id and userId = @Userid)
			BEGIN
				Update OddUser
				set
				title =@Title,
				completed = @completed,
				edited = 1
				where id = @id
				select id,userId,title,completed,edited,CAST(1 as bit) as edited_via_api from OddUser where id = @id;

			END
		ELSE
			BEGIN
				Insert Into OddUser(id,userId,title,completed,edited)
				VALUES(@id,@Userid,@Title,@completed,0);
				select id,userId,title,completed,edited,CAST(0 as bit) as edited_via_api from OddUser where id = @id;

			END
			
	END
END

CREATE PROCEDURE DeleteUser
@id int,
@Tableid int
AS
BEGIN
If @Tableid = 2
	BEGIN
		delete from EvenUser where id = @id;
	END
IF @Tableid = 1
	BEGIN
		
	delete from OddUser where id = @id;

	END
END

CREATE PROCEDURE EditData
@id int,
@Tableid int,
@Title nvarchar(200),
@completed bit
AS
BEGIN
If @Tableid = 2
	BEGIN
		Insert Into HistoryUser(id,userId,title,completed,edited)
		select id,userId,title,completed,edited from EvenUser where id = @id;

		Update EvenUser
		set
		title =@Title,
		completed = @completed,
		edited = 1
		where id = @id

		select id,userId,title,completed,edited from EvenUser where id = @id;
	END
IF @Tableid = 1
	BEGIN
		Insert Into HistoryUser(id,userId,title,completed,edited)
		select id,userId,title,completed,edited from OddUser where id = @id;

		Update OddUser
		set
		title =@Title,
		completed = @completed,
		edited = 1
		where id = @id
				select id,userId,title,completed,edited from OddUser where id = @id;

	END
END

CREATE PROCEDURE GetTableData
@TableId INT 
AS
BEGIN

IF (@TableId = 1)
BEGIN
SELECT userId,id,titel,completed,edited FROM OddUser
END


IF (@TableId = 2)
BEGIN
SELECT userId,id,titel,completed,edited  FROM EvenUser
END


IF (@TableId = 3)
BEGIN
SELECT * FROM HistoryUser
END

END
