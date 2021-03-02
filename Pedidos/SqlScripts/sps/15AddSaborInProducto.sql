ALTER PROCEDURE AddSaborInProducto
    @idSabor int,    
    @idProducto int,
    @idCuenta int
 AS
  DECLARE  @tags NVARCHAR(MAX) = (select top(1) idsSabor from [dbo].[P_SaboresProducto] where idProducto = @idProducto and idCuenta = @idCuenta)  
   UPDATE [dbo].[P_SaboresProducto] SET idsSabor = 
  (SELECT CONCAT(STRING_AGG(value, ','),',',@idSabor) from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC )
  where idProducto = @idProducto and idCuenta = @idCuenta
