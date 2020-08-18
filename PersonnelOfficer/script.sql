USE [master]
GO
 
IF DB_ID(N'WAREHOUS') IS NULL 
BEGIN
	CREATE DATABASE [WAREHOUS]
	 CONTAINMENT = NONE
	 ON  PRIMARY 
	( NAME = N'warehous', FILENAME = N'D:\Working\Database\SQL Server\WAREHOUS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
	 LOG ON 
	( NAME = N'warehous_log', FILENAME = N'D:\Working\Database\SQL Server\WAREHOUS.ldf' , SIZE = 8192KB , MAXSIZE = 52428800KB , FILEGROWTH = 65536KB )
	 WITH CATALOG_COLLATION = DATABASE_DEFAULT
END
GO

USE WAREHOUS
GO
  
IF SCHEMA_ID('Org') IS NULL
BEGIN
	EXEC('CREATE SCHEMA [Org]') 
END
GO

DROP TABLE IF EXISTS [Org].Department
GO

CREATE TABLE [Org].Department
(
   Id int not null identity(1,1) CONSTRAINT [PK_Org_Department_Id] PRIMARY KEY CLUSTERED ,
   [Name] nvarchar(255) NOT NULL CONSTRAINT [UI_Org_Department_Name] UNIQUE,
   [Telephone] nvarchar(100),
   [Address] nvarchar(350),
   [RommNumber] nvarchar(100),
   [LastEditState] nvarchar(50)
)
GO

DROP TABLE IF EXISTS [Org].Position
GO

CREATE TABLE [Org].Position
(
   Id int not null identity(1,1) CONSTRAINT [PK_Org_Position_Id] PRIMARY KEY CLUSTERED ,
   [Name] nvarchar(255) NOT NULL CONSTRAINT [UI_Org_Position_Name] UNIQUE,
   SalaryFrom DECIMAL(16,2),
   SalaryTo DECIMAL(16,2),
   DepartmentId int not null,
   [LastEditState] nvarchar(50)
)
GO


DROP TABLE IF EXISTS [Org].Employee
GO


CREATE TABLE [Org].Employee
(
   Id int not null identity(1,1) CONSTRAINT [PK_Org_Employee_Id] PRIMARY KEY CLUSTERED ,
   [FirstName] nvarchar(255) NOT NULL,
   [Patronymic] nvarchar(255) NULL,
   [Surname] nvarchar(255) NOT NULL,
   [Title] nvarchar(255) NOT NULL,
   DateOfBirth DATE NOT NULL,
   Sex Nchar(1) NOT NULL,
   Married bit not null default(0),
   [Telephone] nvarchar(100),
   [Mobilephone] nvarchar(100),
   [Address] nvarchar(350),
   [Email] nvarchar(350),
   PositionId int not null,
   DepartmentId int not null,
   Salary DECIMAL(16,2),
   [LastEditState] nvarchar(50),
   CONSTRAINT [UI_Org_Employee_Name] UNIQUE ([FirstName], [Patronymic], [Surname]),
)
GO
 
use [WAREHOUS]
go

create or alter procedure Org.p_SaveDepartment
(
 @Id int
,@Name nvarchar(255) 
,@Telephone nvarchar(100) 
,@Address nvarchar(350) 
,@RommNumber nvarchar(100) 
,@LastEditState nvarchar(50)
)
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Department] where Id = @Id))
	 begin
	     raiserror('Отдел не существует!', 16, 1)
		 return (-1)
	 end
	 	 
     ;merge [Org].[Department] s
	 using ( select 
	          isnull(@Id,0) Id 
			, @Name [Name]
			, @Telephone Telephone
			, @Address Address
			, @RommNumber RommNumber
			, @LastEditState LastEditState) t
	 on s.Id = t.Id
	 when matched then update set Name = t.Name, Telephone = t.Telephone, Address = t.Address, RommNumber = t.RommNumber, LastEditState = t.LastEditState
	 when not matched then insert ([Name], [Telephone], [Address], [RommNumber], [LastEditState]) values(t.[Name], t.[Telephone], t.[Address], t.[RommNumber], t.[LastEditState])
	 output inserted.Id; 

	 return (0);
end
go

create or alter procedure Org.p_DeleteDepartment
( @Id int )
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Department] where Id = @Id))
	 begin
	     raiserror('Отдел не существует!', 16, 1)
		 return (-1)
	 end

	 if (exists(select * from [Org].Employee where DepartmentId = @Id))
	 begin
	     raiserror('Нельзя удалить отдел, есть сотрудники!', 16, 1)
		 return (-1)
	 end 

begin try

	 begin tran

	 delete from [Org].[Department] where Id = @Id; 

	 delete from [Org].Position where DepartmentId = @Id

	 commit tran;

	 return (0);
end try
begin catch
    declare @err nvarchar(max) = ERROR_MESSAGE()
	if @@TRANCOUNT <> 0 rollback tran

	raiserror(@err, 16, 1)
	return (-1)
end catch
end
go

create or alter procedure Org.p_GetDepartment
as
begin
    SELECT  [Id]
	       ,[Name]
           ,[Telephone]
           ,[Address]
           ,[RommNumber]
           ,[LastEditState] 
	  FROM [WAREHOUS].[Org].[Department]
	return (0)
end
go

create or alter procedure Org.p_SavePosition
(
 @Id int
,@Name nvarchar(255) 
,@SalaryFrom decimal(16,2)
,@SalaryTo decimal(16,2)
,@DepartmentId int
,@LastEditState nvarchar(50)
)
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Position] where Id = @Id))
	 begin
	     raiserror('Должность не существует!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@DepartmentId,0) = 0 or not exists(select * from [Org].[Department] where Id = @DepartmentId))
	 begin
	     raiserror('Отдел не указан или не существует!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@SalaryFrom,0) < 0 or isnull(@SalaryTo,0) < 0)
	 begin
	     raiserror('Оклад должен быть положительным числом!', 16, 1)
		 return (-1)
	 end

begin try

	 begin tran

     ;merge [Org].[Position] s
	 using ( select 
	          isnull(@Id,0) Id 
			, @Name [Name]
			, @SalaryFrom SalaryFrom
			, case when @SalaryFrom > 0 and isnull(@SalaryTo,0) = 0 then @SalaryFrom else @SalaryTo end  SalaryTo
			, @DepartmentId DepartmentId
			, @LastEditState LastEditState) t
	 on s.Id = t.Id
	 when matched then update set Name = t.Name, SalaryFrom = t.SalaryFrom, SalaryTo = t.SalaryTo, DepartmentId = t.DepartmentId, LastEditState = t.LastEditState
	 when not matched then insert ([Name], SalaryFrom, SalaryTo, DepartmentId, [LastEditState]) values(t.[Name], t.SalaryFrom, t.SalaryTo, t.DepartmentId, t.[LastEditState])
	 output inserted.Id; 

	 update Org.Employee set DepartmentId = @DepartmentId
	  from Org.Employee
	  where PositionId = @Id and DepartmentId <> @DepartmentId

	 commit tran;

	 return (0);
end try
begin catch
    declare @err nvarchar(max) = ERROR_MESSAGE()
	if @@TRANCOUNT <> 0 rollback tran

	raiserror(@err, 16, 1)
	return (-1)
end catch
end
go

create or alter procedure Org.p_DeletePosition
( @Id int )
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Position] where Id = @Id))
	 begin
	     raiserror('Должность не существует!', 16, 1)
		 return (-1)
	 end

	 if (exists(select * from [Org].Employee where PositionId = @Id))
	 begin
	     raiserror('Нельзя удалить должность, есть сотрудники!', 16, 1)
		 return (-1)
	 end 

	 delete from [Org].[Position] where Id = @Id; 

	 return (0);
end
go

create or alter procedure Org.p_GetPosition
as
begin
    SELECT [Id]
		  ,[Name]
		  ,[SalaryFrom]
		  ,[SalaryTo]
		  ,[DepartmentId]
		  ,[LastEditState]
	  FROM [WAREHOUS].[Org].Position
	return (0)
end
go

create or alter procedure Org.p_SaveEmployee
(
 @Id int
,@FirstName nvarchar(255) 
,@Patronymic nvarchar(255) 
,@Surname nvarchar(255) 
,@Title nvarchar(255) 
,@DateOfBirth date
,@Sex nchar(1)
,@Married bit
,@Telephone nvarchar(100) 
,@Mobilephone nvarchar(100) 
,@Address nvarchar(350) 
,@Email nvarchar(350) 
,@DepartmentId int
,@PositionId int
,@Salary decimal(16,2)
,@LastEditState nvarchar(50)
)
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Employee] where Id = @Id))
	 begin
	     raiserror('Сотрудник не существует!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@PositionId,0) = 0 or not exists(select * from [Org].[Position] where Id = @PositionId))
	 begin
	     raiserror('Должность не указана или не существует!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@DepartmentId,0) = 0 or not exists(select * from [Org].[Department] where Id = @DepartmentId))
	 begin
	     raiserror('Отдел не указан или не существует!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@Salary,0) < 0)
	 begin
	     raiserror('Оклад должен быть положительным числом!', 16, 1)
		 return (-1)
	 end

	 if (isnull(@Sex,'') not in ('М', 'Ж'))
	 begin
	     raiserror('Пол не указан', 16, 1)
		 return (-1)
	 end

     ;merge [Org].[Employee] s
	 using ( select 
	          isnull(@Id,0) Id 
			 ,@FirstName FirstName
			 ,@Patronymic Patronymic
			 ,@Surname Surname
			 ,@Title   Title
			 ,@DateOfBirth DateOfBirth
			 ,@Sex  Sex
			 ,@Married Married
			 ,@Telephone Telephone 
			 ,@Mobilephone Mobilephone
			 ,@Address     Address
			 ,@Email       Email
			 ,@DepartmentId DepartmentId
			 ,@PositionId PositionId
			 ,@Salary  Salary
			 ,@LastEditState  LastEditState) t
	 on s.Id = t.Id
	 when matched then update set FirstName = t.FirstName, Patronymic = t.Patronymic, Surname = t.Surname, Title = t.Title, DateOfBirth = t.DateOfBirth, Sex = t.Sex, Married = t.Married, 
	                              Telephone = t.Telephone, Mobilephone = t.Mobilephone, Address = t.Address, Email = t.Email, DepartmentId = t.DepartmentId, PositionId = t.PositionId, 
								  Salary = t.Salary, LastEditState = t.LastEditState
	 when not matched then insert (FirstName, Patronymic, Surname, Title, DateOfBirth, Sex, Married, Telephone, Mobilephone, Address, Email, DepartmentId, PositionId, Salary, LastEditState) 
	                       values (t.FirstName, t.Patronymic, t.Surname, t.Title, t.DateOfBirth, t.Sex, t.Married, t.Telephone, t.Mobilephone, t.Address, t.Email, t.DepartmentId, t.PositionId, t.Salary, t.LastEditState)
	 output inserted.Id; 

	 return (0);
end
go

create or alter procedure Org.p_DeleteEmployee
( @Id int )
as
begin
     if (@Id > 0 and not exists(select * from [Org].[Employee] where Id = @Id))
	 begin
	     raiserror('Сотрудник не существует!', 16, 1)
		 return (-1)
	 end

	 delete from [Org].[Employee] where Id = @Id; 

	 return (0);
end
go

create or alter procedure Org.p_GetEmployee
as
begin
    SELECT 
		   [Id]
		  ,[FirstName]
		  ,[Patronymic]
		  ,[Surname]
		  ,[Title]
		  ,[DateOfBirth]
		  ,[Sex]
		  ,[Married]
		  ,[Telephone]
		  ,[Mobilephone]
		  ,[Address]
		  ,[Email]
		  ,[PositionId]
		  ,[DepartmentId]
		  ,[Salary]
		  ,[LastEditState]
	  FROM [WAREHOUS].[Org].Employee
	return (0)
end
go


INSERT INTO [Org].[Department]
           ([Name]
           ,[Telephone]
           ,[Address]
           ,[RommNumber]
           ,[LastEditState])
     VALUES
           ('Управление'
           ,'15'
           ,''
           ,'78'
           ,'Edit')
GO

INSERT INTO [Org].[Department]
           ([Name]
           ,[Telephone]
           ,[Address]
           ,[RommNumber]
           ,[LastEditState])
     VALUES
           ('Бухгалтерия'
           ,'35'
           ,''
           ,'167'
           ,'Insert')
GO


INSERT INTO [Org].[Position]
           ([Name]
           ,[SalaryFrom]
           ,[SalaryTo]
           ,[DepartmentId]
           ,[LastEditState])
     VALUES
           ('Генеральный директор'
           ,100000.00
           ,500000.00
           ,1
           ,'Insert')
GO

INSERT INTO [Org].[Position]
           ([Name]
           ,[SalaryFrom]
           ,[SalaryTo]
           ,[DepartmentId]
           ,[LastEditState])
     VALUES
           ('Бухгалтер'
           ,50000.00
           ,80000.00
           ,2
           ,'Insert')
GO

INSERT INTO [Org].[Employee]
           ([FirstName]
           ,[Patronymic]
           ,[Surname]
           ,[Title]
           ,[DateOfBirth]
           ,[Sex]
           ,[Married]
           ,[Telephone]
           ,[Mobilephone]
           ,[Address]
           ,[Email]
           ,[PositionId]
           ,[DepartmentId]
           ,[Salary]
           ,[LastEditState])
     VALUES
           ('Анатолий'
           ,'Юрьевич'
           ,'Кирдяшев'
           ,'Анатолий Юрьевич'
           ,'19791227'
           ,'М'
           ,1
           ,'678'
           ,''
           ,''
           ,''
           ,1
           ,1
           ,210000.00
           ,'Insert'
		   )
	  INSERT INTO [Org].[Employee]
           ([FirstName]
           ,[Patronymic]
           ,[Surname]
           ,[Title]
           ,[DateOfBirth]
           ,[Sex]
           ,[Married]
           ,[Telephone]
           ,[Mobilephone]
           ,[Address]
           ,[Email]
           ,[PositionId]
           ,[DepartmentId]
           ,[Salary]
           ,[LastEditState])
     VALUES
           ('Лидия'
           ,'Анатольевна'
           ,'Васильева'
           ,'Лидия Анатольевна'
           ,'20000229'
           ,'Ж'
           ,0
           ,'565'
           ,''
           ,''
           ,''
           ,2
           ,2
           ,80000.00
           ,'Edit'
		   )
GO