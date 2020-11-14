 ALTER PROCEDURE DeleteCategoriaAdicional   
    @idAdicional int,   
    @idCategoria int,
    @idCuenta int
 AS
   DELETE [dbo].[P_CategoriaAdicional]  WHERE idCategoria = @idCategoria and idAdicional = @idAdicional  and idCuenta = @idCuenta
   GO
   