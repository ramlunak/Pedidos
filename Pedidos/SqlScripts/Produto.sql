﻿CREATE TABLE P_Productos(
    id int not null identity(1,1),   
    nombre varchar(255) not null,   
    codigo varchar(255) null,     
    idCategoria int not null,     
    valor decimal not null,     
    activo bit null, 
    idCuenta int null  
);
        