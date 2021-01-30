alter table [dbo].[P_Caja] add idPrimerPedido int null
alter table [dbo].[P_Caja] alter column  idUltimoPedido int null
alter table [dbo].[P_Caja] add fechaCierre datetime null
alter table [dbo].[P_Caja] add codigoPrimerPedido varchar(50) null
alter table [dbo].[P_Caja] add codigoUltimoPedido varchar(50) null