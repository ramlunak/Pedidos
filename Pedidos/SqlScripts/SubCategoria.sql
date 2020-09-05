CREATE TABLE P_SubCategorias(
    id int not null identity(1,1),   
    idCategoria int not null,   
    nombre varchar(255) not null,  
    activo bit null, 
    idCuenta int null  
);
        