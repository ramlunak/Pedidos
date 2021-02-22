alter table [dbo].[P_Cuentas] add idCuentaPadre int null

CREATE TABLE P_Sabores(
    id int not null identity(1,1),   
    nombre varchar(255) not null,       
    valor money null,   
    idCuenta int not null,
    activo bit not null
);
  
alter table [dbo].[P_Productos] add cantidadSabores int null

alter table [dbo].[P_Productos] add actualizarValorSaborMayor bit not null default(0)
alter table [dbo].[P_Productos] add actualizarValorSaborMenor bit not null default(0)
alter table [dbo].[P_Productos] add actualizarValorMediaSabores bit not null default(0)

create table P_SaboresProducto(
id int identity(1,1) not null,
idProducto int not null,
idsSabor text not null,
idCuenta int not null
);