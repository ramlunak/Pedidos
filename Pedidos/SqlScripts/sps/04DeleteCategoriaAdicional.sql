ALTER PROCEDURE DeleteCategoriaAdicional
    @idAdicional int,   
    @idCategoria int,
    @idCuenta int
 AS
  DECLARE  @tags NVARCHAR(MAX) = (select top(1) idsAdicionales from [dbo].[P_CategoriaAdicional] where idCategoria = @idCategoria and idCuenta = @idCuenta)  
  UPDATE [dbo].[P_CategoriaAdicional] SET idsAdicionales =  
  (SELECT STRING_AGG (value, ',')  from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC  where  AC.value <> @idAdicional)
   where idCategoria = @idCategoria and idCuenta = @idCuenta   
   GO;