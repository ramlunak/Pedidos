CREATE PROCEDURE AddIngredienteInProducto
    @idIngrediente int,    
    @idProducto int,
    @idCuenta int
 AS
  DECLARE  @tags NVARCHAR(MAX) = (select top(1) idsIngrediente from [dbo].[P_IngredientesProducto] where idProducto = @idProducto and idCuenta = @idCuenta)  
   UPDATE [dbo].[P_IngredientesProducto] SET idsIngrediente = 
  (SELECT CONCAT(STRING_AGG(value, ','),',',@idIngrediente) from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC )
  where idProducto = @idProducto and idCuenta = @idCuenta
