 ALTER PROCEDURE InsertIfNotExistIngredientesProducto    
    @idProducto int,
    @idCuenta int 
 AS  
  IF NOT EXISTS (SELECT * FROM [P_IngredientesProducto] 
                   WHERE idProducto = @idProducto
                   AND idCuenta = @idCuenta)
   BEGIN
      INSERT INTO [dbo].[P_IngredientesProducto] (idProducto,idCuenta)  VALUES(@idProducto,@idCuenta)
   END
GO;