CREATE TABLE Pedidos (
	"Id" uuid PRIMARY KEY,
	descricao varchar(150) NOT NULL,
	valor_total numeric(14, 2) NOT NULL,
	status int4 NULL,
	situacao varchar(100) NULL,
	created_at timestamptz DEFAULT now() NULL,
	modified_at timestamptz NULL,
	codigo int4 NULL,
	CONSTRAINT constraint_codigo UNIQUE (codigo)
);

CREATE TABLE Pedidos_Items (
	"id" uuid NOT NULL,
	pedido_id uuid NOT NULL,
	item_id uuid NOT NULL,	
	item_nome varchar(100) NOT NULL,
	preco numeric(14,2) NOT NULL,
	quantidade integer NOT NULL,
	created_at timestamp with time zone NULL DEFAULT NOW(),
	modified_at timestamp with time zone NULL,
	FOREIGN KEY (pedido_id) REFERENCES Pedidos ("Id"),
	PRIMARY KEY(pedido_id, item_id)
);
