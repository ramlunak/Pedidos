 ALTER PROCEDURE InsertLog
    @id INT OUTPUT,
    @idCuenta int,
    @fecha datetime,
    @ex text
 AS  
      INSERT INTO [dbo].[P_Logs] (idCuenta,fecha,ex) OUTPUT INSERTED.ID  VALUES(@idCuenta,@fecha,@ex)
 