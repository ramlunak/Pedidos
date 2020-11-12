ALTER PROCEDURE AddCategoriaInAdicionalCategorias
    @idAdicional int,
    @idCuenta int,
    @idCategoria int
 AS
  DECLARE @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = @idAdicional and idCuenta = @idCuenta)  
   UPDATE [dbo].[P_AdicionalCategorias] SET idsCategoria = 
  (SELECT CONCAT(STRING_AGG(value, ','),',',@idCategoria) from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC )
  where idAdicional = @idAdicional and idCuenta = @idCuenta
  GO;