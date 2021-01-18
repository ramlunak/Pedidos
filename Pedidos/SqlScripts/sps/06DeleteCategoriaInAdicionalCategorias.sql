CREATE PROCEDURE DeleteCategoriaInAdicionalCategorias
    @idAdicional int,   
    @idCategoria int,
    @idCuenta int
 AS
  DECLARE @id1 int = @idAdicional ,@id2 int =  @idCategoria ,@id3 int = @idCuenta, @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = @idAdicional and idCuenta = @idCuenta)  
  UPDATE [dbo].[P_AdicionalCategorias] SET idsCategoria =  
  (SELECT STRING_AGG (value, ',')  from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC  where  AC.value <> @idCategoria)
   where idAdicional = @idAdicional and idCuenta = @idCuenta 
   EXEC DeleteCategoriaAdicional   @idAdicional = @id1 , @idCategoria = @id2, @idCuenta = @id3
