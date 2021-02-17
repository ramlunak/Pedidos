alter table [dbo].[P_Cuentas] add idCuentaPadre int null

CREATE TABLE P_Sabores(
    id int not null identity(1,1),   
    nombre varchar(255) not null,       
    valor money null,   
    idCuenta int not null,
    activo bit not null
);
   

