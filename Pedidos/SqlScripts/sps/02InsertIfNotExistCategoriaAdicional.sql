 ALTER PROCEDURE InsertIfNotExistCategoriaAdicional    
    @idCategoria int,
    @idCuenta int 
 AS  
  IF NOT EXISTS (SELECT * FROM [P_CategoriaAdicional] 
                   WHERE idCategoria = @idCategoria
                   AND idCuenta = @idCuenta)
   BEGIN
      INSERT INTO [dbo].[P_CategoriaAdicional] (idCategoria,idCuenta)  VALUES(@idCategoria,@idCuenta)
   END
GO;