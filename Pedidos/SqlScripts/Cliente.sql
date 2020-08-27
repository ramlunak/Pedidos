CREATE TABLE P_Clientes(
    id int not null identity(1,1),   
    nombre varchar(255) not null,   
    telefono varchar(255) not null,     
    activo bit null,   
    idCuenta int null  
);