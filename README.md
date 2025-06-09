##### Sistema de Controle de Investimentos

#### Índice
- Modelagem de Banco de Dados (MySQL)
- Índices, Performance e Atualização de Posições
- Aplicação em .NET Core com C#
- Lógica de Negócio – Preço Médio
- Testes Unitários e Mutantes
- Kafka e Worker Service
- Resiliência e Circuit Breaker
- Escalabilidade e Balanceamento
- APIs REST + OpenAPI
- Inserção de Dados 
- Funcionalidades Adicionais
- Preparação para Produção
- Dicas para Resolução de Problema de senha incache
- Observações Finais


---

### 1. Modelagem de Banco de Dados (MySQL)

#### Estrutura das Tabelas

**usuarios**
- id INT (PK) Auto Increment  
- nome VARCHAR(100) Not Null  
- email VARCHAR(150) Not Null, Unique  
- pct_corretagem DECIMAL(5,2) Percentual de corretagem (%)  

**ativos**
- id INT (PK) Auto Increment  
- codigo VARCHAR(10) Ex: PETR4, VALE3  
- nome VARCHAR(100) Nome descritivo  

**operacoes**
- id INT (PK) Auto Increment  
- usuario_id INT (FK) Relacionado a usuarios(id)  
- ativo_id INT (FK) Relacionado a ativos(id)  
- quantidade INT Quantidade negociada  
- preco_unitario DECIMAL(10,2) Preço por unidade  
- tipo_operacao ENUM('compra','venda')  
- corretagem DECIMAL(10,2) Valor da corretagem aplicada  
- data_hora DATETIME Timestamp da operação  

**cotacoes**
- id INT (PK) Auto Increment  
- ativo_id INT (FK) Relacionado a ativos(id)  
- preco_unitario DECIMAL(10,2) Preço da cotação em tempo real  
- data_hora DATETIME Timestamp da cotação  

**posicoes**
- id INT (PK) Auto Increment  
- usuario_id INT (FK) Relacionado a usuarios(id)  
- ativo_id INT (FK) Relacionado a ativos(id)  
- quantidade INT Quantidade atual do ativo  
- preco_medio DECIMAL(10,2) Preço médio ponderado de aquisição  
- pl DECIMAL(10,2) Profit & Loss: (preco_atual - preco_medio) * quantidade  

#### Justificativas Técnicas

- DECIMAL para precisão em cálculos financeiros  
- ENUM para limitar valores válidos  
- Chaves estrangeiras garantem integridade referencial  
- Índices em colunas estratégicas para performance  
- Charset utf8mb4 com utf8mb4_unicode_ci  
- Convenção snake_case para consistência  

---

### 2. Índices, Performance e Atualização de Posições

**Índices criados**
```sql

CREATE INDEX idx_operacoes_usuario_ativo_data
ON operacoes (usuario_id, ativo_id, data_hora DESC);

CREATE INDEX idx_cotacoes_ativo_data
ON cotacoes (ativo_id, data_hora DESC);

SELECT * FROM operacoes
WHERE usuario_id = ? AND ativo_id = ? AND data_hora >= NOW() - INTERVAL 30 DAY
ORDER BY data_hora DESC;

````
Utilizar em caso de falha de conexão com o banco por senha: dotnet ef database update --connection "server=localhost;port=3306;database=investimentosdb;user=root;password=root"


### 3. Aplicação em .NET Core com C#

Foi desenvolvida uma aplicação completa em .NET Core 8 utilizando boas práticas de arquitetura, separação de responsabilidades e integração com banco relacional (MySQL). A aplicação permite o acompanhamento de investimentos, com exibição do total investido por ativo, posição atual, lucro/prejuízo (P&L) e total de corretagem por cliente.

##Estrutura do Projeto:

- Domain: Define as entidades de negócio (Usuário, Ativo, Operação, Cotação, Posição) e enums como TipoOperacao.
- Application: Contém os serviços com regras de negócio, como PrecoMedioService (cálculo de preço médio ponderado) e PosicaoService (geração de posições e P&L).
- Infrastructure: Responsável pela persistência via Entity Framework (MySQL) e integração com Kafka no CotacoesKafkaWorker.
- API: Expõe endpoints RESTful utilizando ASP.NET Core, com documentação gerada via Swagger/OpenAPI 3.0.
- Worker: Serviço separado para consumir cotações via Kafka e atualizar dinamicamente as posições no banco.
- Tests: Implementação de testes unitários com xUnit, cobrindo casos positivos e negativos de cálculo do preço médio.

## Tecnologias Utilizadas:

- .NET Core 8
- Entity Framework Core (MySQL)
- xUnit (testes unitários)
- Kafka (.NET Worker Service)
- Swagger (documentação OpenAPI 3.0)
- React + Tailwind

## Boas Práticas Aplicadas:

- Separação clara em camadas (Domain, Application, Infrastructure)
- Uso de async/await com Entity Framework
- Injeção de dependência, princípios SOLID e foco em coesão
- Responsabilidade única por classe e organização modular
- Documentação clara e rastreável dos endpoints REST
- Atualização em tempo real das posições via consumo de eventos Kafka

---

### 4. Lógica de Negócio – Preço Médio

#### Fórmula

precoMedio = soma(preco_unitario * quantidade) / soma(quantidade)

#### Regras

- Apenas operações de COMPRA são consideradas  
- Quantidade igual a zero é ignorada  

#### Exceções lançadas para:

- Lista vazia  
- Apenas vendas  
- Nenhuma compra válida com quantidade > 0  

---

### 5. Testes Unitários e Mutantes

#### Testes Unitários com xUnit

- Múltiplas compras → Preço médio calculado corretamente  
- Lista vazia → InvalidOperationException  
- Apenas vendas → InvalidOperationException  
- Compra com qtd = 0 → InvalidOperationException  

#### Testes Mutantes

Mutação 1 (cálculo errado):

return ((precoAnterior * qtdAnterior) + (precoAtual * qtdAtual)) / (qtdAnterior - qtdAtual);

Mutação 2 (erro de validação):

return saldo > valorOperacao; // deveria ser >=

---

### 6. Kafka e Worker Service

#### Integração

- Tópico Kafka: cotacoes-novas  
- Consumidor: CotacaoKafkaConsumer  
- Produtor: Worker externo com dados da API pública da B3 (https://b3api.vercel.app/)  

#### Funcionamento

- Conecta ao tópico  
- Verifica se o ativo existe  
- Verifica se já existe uma cotação com mesmo ativo_id e data_hora  
- Se for nova, insere no banco de dados (tabela cotacoes)  
- Atualiza automaticamente as posições com o novo PL (profit & loss)  

#### Segurança e Robustez

- Mensagens inválidas são ignoradas com logs apropriados  
- Controle de duplicidade implementado  
- Fallback de conexão ao banco  
- Idempotência: cada cotação é única por ativo_id + data_hora  

---


### 7. Resiliência e Circuit Breaker

#### Objetivo

Garantir que falhas em serviços externos (como Kafka ou MySQL) não derrubem a aplicação inteira e permitam recuperação automática.

#### Implementações

- Retry Policy: o Worker tenta reconectar ao banco ou Kafka a cada 5 segundos após falha  
- Fallback Policy: em caso de falha persistente, loga a exceção, ignora a mensagem e continua rodando  
- Isolation: o Worker é isolado da API principal, impedindo que falhas afetem os endpoints  
- Logs estruturados: implementados com ILogger<T>, facilitando rastreamento no console ou ferramentas como Grafana/Prometheus  

---

### 8. Escalabilidade e Balanceamento

#### Estratégias Planejadas

- Escalabilidade horizontal: a aplicação e o Worker podem ser escalados em múltiplas instâncias (containers)  
- Kafka: o consumo é naturalmente balanceado entre múltiplos consumidores com o mesmo groupId  
- Load Balancing: suporte a round-robin (via NGINX ou serviços cloud como Azure Front Door), com futura adaptação para balanceamento por latência  
- API stateless: pode ser replicada sem dependência de sessão, permitindo alta disponibilidade  
- Containers e Orquestração: aplicação pronta para containerização com Docker (imagens separadas para API e Worker)  
- Suporte a Docker Compose e Kubernetes  

---

### 9. APIs REST + OpenAPI

#### Operações

GET /api/operacoes  
GET /api/operacoes/por-usuario/{usuarioId}  
POST /api/operacoes  

#### Cotações

GET /api/cotacoes  
GET /api/cotacoes/ultimo/{ativoId}  

#### Posições

GET /api/posicoes/por-usuario/{usuarioId}  

#### Relatórios

GET /api/relatorios/corretagem-total  
GET /api/relatorios/top-posicoes  
GET /api/preco-medio/usuario/{usuarioId}/ativo/{ativoId}  

#### Swagger

Documentação disponível em:  
https://localhost:5183/swagger/index.html  

---

### 10. Inserção de Dados para Testes

Após subir o banco (criado automaticamente pelas migrations), você pode inserir dados usados no desenvolvimento executando:

```bash

mysql -u root -p investimentosdb < docs/mock_data.sql

```

---


#### 11. Funcionalidades Adicionais

- Endpoint para cálculo de preço médio por usuário e ativo (`/api/preco-medio/usuario/{usuarioId}/ativo/{ativoId}`)  
- Endpoint para relatório de corretagem total acumulada   
- Atualização em tempo real das posições com base nas últimas cotações recebidas via Kafka  
- Testes de comportamento adverso (vendas sem compras, compras inválidas, cotações duplicadas)  
- Integração validada com API pública da B3 para simulação de fluxo real de mercado  
- Worker separado e desacoplado da API, garantindo resiliência e escalabilidade

---

#### 12. Preparação para Produção

- Projeto separado em camadas (Domain, Application, Infrastructure, API, Worker)  
- Aplicação preparada para execução em containers (Docker)  
- Estrutura compatível com orquestração em Docker Compose ou Kubernetes  
- Código pronto para extensões como autenticação, autorização, cache e notificações

### 13. Dicas de Execução e Resolução de Problemas

Caso ocorra falha de conexão com o banco de dados (ex: Access denied for user 'root'@'172.18.0.1' (using password: NO), é possível forçar a atualização do banco via linha de comando com uma string de conexão explícita: dotnet ef database update --connection "server=localhost;port=3306;database=investimentosdb;user=root;password=root"

### 14. Observações Finais

Durante o desenvolvimento, algumas funcionalidades além das obrigatórias foram idealizadas e parcialmente iniciadas, mas não implementadas integralmente devido à priorização do escopo principal e à limitação de tempo. Entre elas:

Montagem de gráficos analíticos: a estrutura para endpoints relacionados já se encontra presente nas controllers, visando futuras visualizações de desempenho e histórico de investimentos. No entanto, a renderização dos gráficos no front-end e a lógica de cálculo ainda não foram concluídas.

Dashboard analítico completo: a versão atual do front-end apresenta cards dinâmicos e posições consolidadas, mas ainda não inclui os gráficos evolutivos planejados inicialmente.

Esses pontos foram considerados extras não obrigatórios, pensados para serem desenvolvidos caso houvesse tempo hábil. Por isso, foram postergados, a fim de não comprometer a entrega técnica essencial do projeto.


