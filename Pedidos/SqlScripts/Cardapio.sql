CREATE TABLE P_Cardapios(
    id int not null identity(1,1),   
    idProducto int not null,   
    valor decimal not null,   
    fecha datetime null,     
    minutosPreparacion int null,        
    horasPreparacion int null,        
    activo bit null, 
    idCuenta int null  
);