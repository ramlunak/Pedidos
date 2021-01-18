CREATE PROCEDURE AddCategoriaInAdicionalCategorias
    @idAdicional int,    
    @idCategoria int,
    @idCuenta int
 AS
  DECLARE @id1 int = @idAdicional ,@id2 int =  @idCategoria ,@id3 int = @idCuenta, @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = @idAdicional and idCuenta = @idCuenta)  
   UPDATE [dbo].[P_AdicionalCategorias] SET idsCategoria = 
  (SELECT CONCAT(STRING_AGG(value, ','),',',@idCategoria) from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC )
  where idAdicional = @idAdicional and idCuenta = @idCuenta
  EXEC AddCategoriaAdicional @idAdicional = @id1 , @idCategoria = @id2, @idCuenta = @id3
