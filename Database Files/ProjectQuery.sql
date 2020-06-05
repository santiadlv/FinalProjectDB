--RegresaR
ALTER PROC ReturnRange
	@range1 float, 
	@range2 float
AS
	DECLARE @avg_table TABLE(ID char(9), Average float)
	INSERT INTO @avg_table(ID, Average)
		SELECT C.Nomina, 
			(SELECT AVG(x)
			 FROM (VALUES(C1),(C2),(C3),(C4),(C5),(C6)) T(x)) AS Prom
		FROM Lista_calificaciones AS LC JOIN Calificacion AS C ON LC.Calificacion_id = C.Calificacion_id
	SELECT ID, ROUND(Average, 2) AS Average
	FROM @avg_table 
	WHERE Average BETWEEN @range1 AND @range2
GO

EXEC ReturnRange 80, 85
GO

--RegresaR
ALTER PROC ReturnName
	@Fname varchar(20),
	@Lname varchar(20)
AS
	SELECT P.Nomina, P.Nombre, P.Apellido_paterno, P.Apellido_materno, C.Comentario, C.Fecha
	FROM Profesor AS P JOIN Comentarios AS C ON P.Nomina = C.Nomina
	WHERE P.Nombre LIKE '%' + @Fname + '%' AND 
	(P.Apellido_paterno LIKE '%' + @Lname + '%' OR P.Apellido_materno LIKE '%' + @Lname + '%')
GO

EXEC ReturnName 'carlos', 'ventura'
GO


--RegresaRFecha
ALTER PROC ReturnNameDate
	@Fname varchar(20),
	@Lname varchar(20),
	@date datetime
AS
	SELECT P.Nomina, P.Nombre, P.Apellido_paterno, P.Apellido_materno, C.Comentario, C.Fecha
	FROM Profesor AS P JOIN Comentarios AS C ON P.Nomina = C.Nomina
	WHERE P.Nombre LIKE '%' + @Fname + '%' AND 
	(P.Apellido_paterno LIKE '%' + @Lname + '%' OR P.Apellido_materno LIKE '%' + @Lname + '%') AND
	C.Fecha = @date
GO

EXEC ReturnNameDate 'Carlos', 'Ventura', '2020-04-28 00:00:10'
GO

--InsertaComentario
ALTER PROC InsertComment
	@Fname varchar(20),
	@Lname varchar(20),
	@comment varchar(200),
	@date datetime
AS
	BEGIN TRAN
		BEGIN TRY
			DECLARE @ID char(9)
			SET @ID = (SELECT P.Nomina FROM Profesor AS P WHERE P.Nombre = @Fname AND P.Apellido_paterno = @Lname)

			IF @ID IS NOT NULL
				BEGIN
				INSERT INTO Comentarios(Nomina, Comentario, Fecha)
				VALUES(@ID, @comment, @date)
				PRINT('Comment made successfully.')
				COMMIT TRAN
				END
			ELSE
				BEGIN
				RAISERROR('Error encountered.', 16, 1)
				END
		END TRY
		BEGIN CATCH
			PRINT('Comment to professor at specified date has already been made, or professor doesn''t exist in the current context. Please try again.')
			ROLLBACK TRAN
		END CATCH
GO

EXEC InsertComment 'Raime Alejandro', 'Bustos', '¡Feliz Navidad!', '2019-12-25'
GO

--RegresaAgendaHoy
ALTER PROC ReturnTodaySchedule
	@days int
AS
	DECLARE @temp TABLE(nomina char(9), diff int)
	INSERT INTO @temp (nomina, diff)
		SELECT C.Nomina, MIN(DATEDIFF(DAY, C.Fecha, GETDATE())) as diff
		FROM Comentarios AS C
		GROUP BY C.Nomina
	SELECT * FROM @temp WHERE diff > @days
GO

EXEC ReturnTodaySchedule 20
GO

--BorraComentario
ALTER PROC EraseComment
	@ID char(9),
	@date datetime
AS
	BEGIN TRAN
		BEGIN TRY			
			DECLARE @size int
			SET @size = (SELECT COUNT(*) FROM (SELECT *
											   FROM Comentarios
											   WHERE Nomina = @ID AND Fecha = @date) as subquery)

			IF @size = 0
				BEGIN
					PRINT('Comment with specified ID and date couldn''t be found. Please try again.')
					RAISERROR('Error encountered.', 16, 1);
				END
			ELSE
				BEGIN
					DELETE
					FROM Comentarios
					WHERE Nomina = @ID AND Fecha = @date
					PRINT('Comment deleted successfully.')
					COMMIT
				END
		END TRY
		BEGIN CATCH
			ROLLBACK
		END CATCH
GO

INSERT INTO Comentarios(Nomina, Comentario, Fecha) VALUES('L00731250', 'test', '2020-05-25')
EXEC EraseComment 'L00731250', '2020-05-25'
GO

--ActualizaRubro
ALTER PROC UpdateValue
	@column int,
	@ID char(9),
	@newScore float
AS
	BEGIN TRAN
		BEGIN TRY
			DECLARE @table nvarchar(20)
			SET @table = N'Lista_calificaciones'

			DECLARE @CalID nvarchar(1)
			SET @CalID = CONVERT(nvarchar, (SELECT Calificacion_id
											FROM Calificacion
											WHERE Nomina = @ID))

			DECLARE @UpdateColumn nvarchar(2)
			SET @UpdateColumn = (SELECT
								 CASE @column
									WHEN 1 THEN 'C1'
									WHEN 2 THEN 'C2'
									WHEN 3 THEN 'C3'
									WHEN 4 THEN 'C4'
									WHEN 5 THEN 'C5'
									WHEN 6 THEN 'C6'
										   ELSE NULL
								 END AS SelectedColumn)

			IF @CalID IS NULL OR @UpdateColumn IS NULL
				BEGIN
					RAISERROR('Error encountered with ID or Column.', 16, 1);
				END
			ELSE
				BEGIN
					DECLARE @sql nvarchar(500)
					SET @sql = N'UPDATE ' + @table + 
								' SET ' + @UpdateColumn + ' = ' + CONVERT(nvarchar, @newScore) +
								' WHERE ' + @table + '.Calificacion_id = ' + @CalID;
					EXEC sp_executesql @sql
					PRINT('Update successful.')
					COMMIT
				END
		END TRY
		BEGIN CATCH
			PRINT('Update couldn''t be completed. Make sure ID and column are valid. Please try again.')
			ROLLBACK
		END CATCH
GO

EXEC UpdateValue 6, 'L00731250', 90
GO

--Reporte
ALTER PROC Report
	@range1 datetime,
	@range2 datetime
AS
	BEGIN
		SELECT P.Nomina, P.Nombre, P.Apellido_paterno, P.Apellido_materno, C.Comentario, C.Fecha
		FROM Profesor AS P JOIN Comentarios AS C ON P.Nomina = C.Nomina
		WHERE C.Fecha BETWEEN @range1 AND @range2
	END
GO

EXEC Report '2020-04-27', '2020-05-28'
GO


--VIEWS

ALTER VIEW VProfesores AS
	SELECT *
	FROM Profesor
GO

SELECT * FROM VProfesores
GO

ALTER VIEW VComentarios AS
	SELECT C.Nomina, P.Nombre, P.Apellido_paterno AS Apellido, C.Comentario, C.Fecha
	FROM Comentarios AS C JOIN Profesor AS P ON C.Nomina = P.Nomina
GO

SELECT * FROM VComentarios
GO

ALTER VIEW VCalificaciones AS
	SELECT P.Nomina, P.Nombre, P.Apellido_paterno AS Apellido, LC.C1, LC.C2, LC.C3, LC.C4, LC.C5, LC.C6
	FROM Lista_calificaciones AS LC JOIN Calificacion AS C ON LC.Calificacion_id = C.Calificacion_id
		 JOIN Profesor AS P ON C.Nomina = P.Nomina
GO

SELECT * FROM VCalificaciones
GO

ALTER VIEW VPromedios AS 
	SELECT P.Nomina, P.Nombre, P.Apellido_paterno AS Apellido,
		   (SELECT ROUND(Grades.average, 2)
			FROM (SELECT AVG(x) AS average
				  FROM (VALUES(C1),(C2),(C3),(C4),(C5),(C6)) T(x)) AS Grades) AS Promedio
	FROM Lista_calificaciones AS LC JOIN Calificacion AS C ON LC.Calificacion_id = C.Calificacion_id
		 JOIN Profesor AS P ON C.Nomina = P.Nomina
GO

SELECT * FROM VPromedios
GO