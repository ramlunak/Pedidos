
--TRIGGER FUERA DE USO

--DROP TRIGGER UPDATE_CATEGORIA_ADICIONAL

CREATE TRIGGER UPDATE_CATEGORIA_ADICIONAL
ON [P_AdicionalCategorias]
FOR UPDATE
AS
BEGIN
    DECLARE @tags NVARCHAR(MAX) = (select top(1) idsCategoria from [dbo].[P_AdicionalCategorias] where idAdicional = 5 and idCuenta = 5)    
    SELECT value  FROM STRING_SPLIT(@tags, ',') WHERE RTRIM(value) <> ''
    DELETE FROM [dbo].[P_CategoriaAdicional]  WHERE idCategoria = 5
    
END
GO

