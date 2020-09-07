CREATE TABLE P_Ventas(
    id int not null identity(1,1),   
    nombre varchar(255) not null,  
    idProducto int not null,
    idPedido int not null,
    data datetime not null,
    valor decimal(18,5) not null,
    status tinyint null,
    activo bit null, 
    idCuenta int null  
);
        