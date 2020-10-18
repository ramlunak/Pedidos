CREATE TABLE P_Pedidos(
    id int not null identity(1,1),        
    idCliente int null,  
    idDireccion int null,  
    idMesa int null,  
    idAplicativo int null,  
    fecha datetime not null,     
    status varchar(100) null,
    descripcion varchar(255) null,
    valor decimal(18,5) null,
    descuento decimal(18,5) null,
    activo bit null, 
    idCuenta int null  
);
