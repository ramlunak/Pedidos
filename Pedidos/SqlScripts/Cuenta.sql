CREATE TABLE P_Cuentas(
    id int not null identity(1,1),   
    usuario varchar(255) not null,   
    password varchar(255) not null,     
    rol varchar(255)  null,     
    idPlano int  null,    
    activo bit not null,

    estado varchar(255)  null,  
    municipio varchar(255)  null,  
);
        