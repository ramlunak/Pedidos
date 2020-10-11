CREATE TABLE P_PedidoProductos(
    id int not null identity(1,1),   
    idPedido int not null,   
    idProducto int not null,
    valorProducto decimal(18,5) null, 
  );
    
        