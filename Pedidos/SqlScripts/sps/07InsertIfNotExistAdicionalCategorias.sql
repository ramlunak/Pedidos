ALTER PROCEDURE InsertIfNotExistAdicionalCategorias
    @idAdicional int,
    @idCuenta int   
 AS
 BEGIN
   IF NOT EXISTS (SELECT * FROM P_AdicionalCategorias 
                   WHERE idAdicional = @idAdicional
                   AND idCuenta = @idCuenta)
   BEGIN
       INSERT INTO P_AdicionalCategorias(idAdicional,idCuenta)
       VALUES (@idAdicional,@idCuenta)
   END
END