CREATE TABLE Produtos (
	"Id" uuid PRIMARY KEY,
	nome varchar(100) NOT NULL,
	descricao varchar(150) NOT NULL,
	preco numeric(14,2) NOT NULL,	
	quantidade_estoque integer,
	created_at timestamp with time zone NULL DEFAULT NOW(),
	modified_at timestamp with time zone NULL
);
