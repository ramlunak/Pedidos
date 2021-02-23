 ALTER PROCEDURE GetSaboresPorProducto
    @idProducto int,
    @idCuenta int
 AS
   DECLARE @tags NVARCHAR(MAX) = (select top(1) idsSabor from [dbo].[P_SaboresProducto] where idProducto = @idProducto and idCuenta = @idCuenta)  
  
  SELECT 
  id,
  nombre, 
  idCuenta,
  valor,
  CAST(CASE WHEN value IS NULL THEN 0 ELSE 1 END AS bit) AS activo  

 from (SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> '') as AC
 right join [dbo].[P_Sabores] as ING on AC.value = ING.id
 where ING.activo = 1 and ING.idCuenta = @idCuenta
