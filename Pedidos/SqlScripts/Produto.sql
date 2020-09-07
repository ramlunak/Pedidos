CREATE TABLE P_Productos(
    id int not null identity(1,1),   
    nombre varchar(255) not null,   
    codigo varchar(255) null,     
    imagen image null,     
    idCategoria int not null,     
    horasPreparacion int null,     
    minutosPreparacion int null,     
    valor decimal not null,     
    activo bit null, 
    idCuenta int null  
);
        