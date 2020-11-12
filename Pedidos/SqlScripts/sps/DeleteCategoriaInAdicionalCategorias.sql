ALTER PROCEDURE DeleteCategoriaInAdicionalCategorias
    @idAdicional int,
    @idCuenta int,
    @idCategoria int
 AS
  DECLARE @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = @idAdicional and idCuenta = @idCuenta)  
  UPDATE [dbo].[P_AdicionalCategorias] SET idsCategoria =  
  (SELECT STRING_AGG (value, ',')  from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC  where  AC.value <> @idCategoria)
   where idAdicional = @idAdicional and idCuenta = @idCuenta
  GO;