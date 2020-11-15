 CREATE PROCEDURE GetCategoriasPorAdicional
    @idAdicional int,
    @idCuenta int
 AS
   DECLARE @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = @idAdicional and idCuenta = @idCuenta)  
  
  SELECT 
  id,
  nombre, 
  idCuenta,
  CAST(CASE WHEN value IS NULL THEN 0 ELSE 1 END AS bit) AS activo  

 from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC
 right join [dbo].[P_Categorias] as CT on AC.value = CT.id
 where CT.activo = 1 and CT.idCuenta = @idCuenta

GO;