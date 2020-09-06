CREATE TABLE P_Direcciones(
    id int not null identity(1,1),   
    code varchar(255) null,   
    state varchar(255) null,   
    city varchar(255) null,   
    district varchar(255) null,   
    address varchar(255) null,   
    numero varchar(255) null,   
    complemento varchar(255) null,     
    activo bit null,   
    idCuenta int null,  
    idCliente int null, 
    idPedido int null  
);