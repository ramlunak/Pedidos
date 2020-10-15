CREATE TABLE P_Aplicativos(
    id int not null identity(1,1),   
    nombre varchar(255) not null,  
    activo bit null, 
    idCuenta int null  
  );
    
        