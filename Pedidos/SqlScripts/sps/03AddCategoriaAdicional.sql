CREATE PROCEDURE AddCategoriaAdicional
    @idAdicional int,    
    @idCategoria int,
    @idCuenta int
 AS
  DECLARE  @tags NVARCHAR(MAX) = (select top(1) idsAdicionales from [dbo].[P_CategoriaAdicional] where idCategoria = @idCategoria and idCuenta = @idCuenta)  
   UPDATE [dbo].[P_CategoriaAdicional] SET idsAdicionales = 
  (SELECT CONCAT(STRING_AGG(value, ','),',',@idAdicional) from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC )
  where idCategoria = @idCategoria and idCuenta = @idCuenta
