CREATE TABLE P_Pedidos(
    id int not null identity(1,1),        
    idCliente int null,  
    idMesa int null,  
    idAplicativo int null,  
    fecha datetime not null,     
    status tinyint null,
    descripcion varchar(255) null,
    valor decimal(18,5) null,
    activo bit null, 
    idCuenta int null  
);
