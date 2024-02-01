-- Criar as tabelas
-- Usei a porta 5445 para o postgres no Docker
create database teste_dev_db;

create table user_table (
	texto text not null,
	numero INT unique not null check (numero>0)
);

create table user_logs (
	operacao text not null,
	datahora TIMESTAMP default CURRENT_TIMESTAMP
);

-- Criar as triggers / funções para inserir os logs na tabela logs
-- Função do insert
CREATE OR REPLACE FUNCTION log_insert_on_logs()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO user_logs (operacao, datahora) 
    VALUES ('INSERT ' || NEW.texto || ' , ' || NEW.numero, CURRENT_TIMESTAMP);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_to_get_insert_on_logs 
AFTER INSERT ON user_table
FOR EACH ROW
EXECUTE FUNCTION log_insert_on_logs();


-- Função do update
CREATE OR REPLACE FUNCTION log_update_on_logs()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO user_logs (operacao, datahora) 
    VALUES ('UPDATE ' || NEW.texto || ' , ' || NEW.numero || ' WHERE ' || OLD.numero, CURRENT_TIMESTAMP);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_to_get_update_on_logs 
AFTER UPDATE ON user_table
FOR EACH ROW
EXECUTE FUNCTION log_update_on_logs();

-- Função do delete
CREATE OR REPLACE FUNCTION log_delete_on_logs()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO user_logs (operacao, datahora) 
    VALUES ('DELETE ' || OLD.texto || ',' || OLD.numero, CURRENT_TIMESTAMP);
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_to_get_delete_on_logs 
AFTER DELETE ON user_table
FOR EACH ROW
EXECUTE FUNCTION log_delete_on_logs();

