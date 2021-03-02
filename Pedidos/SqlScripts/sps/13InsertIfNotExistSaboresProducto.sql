 CREATE PROCEDURE InsertIfNotExistSaboresProducto    
    @idProducto int,
    @idCuenta int 
 AS  
  IF NOT EXISTS (SELECT * FROM [P_SaboresProducto] 
                   WHERE idProducto = @idProducto
                   AND idCuenta = @idCuenta)
   BEGIN
      INSERT INTO [dbo].[P_SaboresProducto] (idProducto,idCuenta)  VALUES(@idProducto,@idCuenta)
   END
