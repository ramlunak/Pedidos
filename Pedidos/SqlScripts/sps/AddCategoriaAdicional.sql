 ALTER PROCEDURE AddCategoriaAdicional
    @idAdicional int,   
    @idCategoria int,
    @idCuenta int
 AS  
   INSERT INTO [dbo].[P_CategoriaAdicional] (idAdicional,idCategoria,idCuenta)  VALUES(@idAdicional,@idCategoria,@idCuenta)
GO;