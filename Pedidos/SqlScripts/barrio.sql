CREATE TABLE P_Barrios(
    id int not null identity(1,1),   
    estado varchar(50) not null,   
    municipio varchar(255) not null,   
    nombre varchar(255) not null,   
    idCuenta int not null,
    activo bit not null
);
        