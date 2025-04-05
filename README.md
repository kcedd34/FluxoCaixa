
# Sistema de Fluxo de Caixa

Sistema desenvolvido para controlar o fluxo de caixa diário de um comerciante, permitindo o registro de lançamentos (débitos e créditos) e a geração de relatórios consolidados diários.

# Diagrama Modular

![modular_fluxocaixa](https://github.com/user-attachments/assets/92d34b87-470a-444b-ba2b-3117a01a3a7e)


# Diagrama de Camadas

![image](https://github.com/user-attachments/assets/d063e19f-d586-4372-b281-97bde1039588)


# Diagrama de Fluxo

![image](https://github.com/user-attachments/assets/e9680db2-8e42-4ede-a994-a56799a06494)


## 📋 Requisitos de Negócio

- Serviço para controle de lançamentos (débitos e créditos)
- Serviço de consolidado diário (saldo)

## 🏗️ Arquitetura

O sistema foi desenvolvido usando uma arquitetura de microsserviços com comunicação assíncrona, garantindo:

- **Alta disponibilidade**: O serviço de lançamentos continua operacional mesmo quando o serviço de consolidado está indisponível
- **Escalabilidade**: Capacidade de processar 50 requisições por segundo no serviço de consolidado
- **Resiliência**: Mecanismos como Circuit Breaker, retentativas automáticas e cache para garantir operação contínua

### Componentes principais:

1. **API REST**: Interface para acesso às funcionalidades
2. **Serviço de Lançamentos**: Gerenciamento de entradas e saídas financeiras
3. **Serviço de Consolidado**: Processamento e cálculo de saldos diários
4. **Mensageria**: Comunicação assíncrona entre serviços
5. **Cache**: Otimização de performance para dados frequentemente acessados

### Tecnologias utilizadas:

- **Backend**: .NET 6 / C#
- **Banco de dados**: SQL Server
- **Cache**: Redis
- **Mensageria**: RabbitMQ
- **Documentação API**: Swagger
- **Logs**: Serilog

## 🚀 Instalação e Execução

### Pré-requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/sql-server/) (ou SQL Server LocalDB)
- [Redis](https://redis.io/download) (opcional, para cache)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (opcional, para mensageria)

### Configuração do Ambiente

1. Clone o repositório:
   ```
   git clone https://github.com/seuusuario/FluxoCaixa.git
   cd FluxoCaixa
   ```

2. Configure as strings de conexão no arquivo `appsettings.json` no projeto `FluxoCaixa.API`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FluxoCaixaDb;Trusted_Connection=True;MultipleActiveResultSets=true",
       "RedisConnection": "localhost:6379",
       "RabbitMQConnection": "amqp://guest:guest@localhost:5672"
     }
   }
   ```

   > **Nota**: Para desenvolvimento local sem Redis e RabbitMQ, o sistema possui fallbacks para funcionamento em memória.

3. Execute as migrações do banco de dados:
   ```
   cd FluxoCaixa.API
   dotnet ef database update
   ```

### Executando a Aplicação

1. Via Visual Studio:
   - Abra a solução `FluxoCaixa.sln` no Visual Studio
   - Defina o projeto `FluxoCaixa.API` como projeto de inicialização
   - Pressione F5 para iniciar a aplicação

2. Via linha de comando:
   ```
   cd FluxoCaixa.API
   dotnet run
   ```

3. Acesse a documentação Swagger:
   ```
   https://localhost:7001/swagger
   ```
   > A porta pode variar conforme configuração local.

## 📱 Utilizando a API

### Lançamentos

#### Criar um novo lançamento
```http
POST /api/lancamentos
```
```json
{
  "data": "2025-04-04",
  "descricao": "Venda Loja",
  "valor": 1500.00,
  "tipo": "Credito",
  "categoria": "Vendas"
}
```

#### Obter lançamentos por data
```http
GET /api/lancamentos/data/2025-04-04
```

#### Obter lançamentos por período
```http
GET /api/lancamentos/periodo?inicio=2025-04-01&fim=2025-04-05
```

### Consolidado

#### Obter consolidado por data
```http
GET /api/consolidado/data/2025-04-04
```

#### Obter consolidados por período
```http
GET /api/consolidado/periodo?inicio=2025-04-01&fim=2025-04-05
```

## 🔧 Arquitetura e Estrutura de Código

O projeto segue os princípios da Clean Architecture:

1. **FluxoCaixa.Domain**: Contém as entidades e regras de negócio centrais
   - Entidades: `Lancamento`, `ConsolidadoDiario`
   - Eventos de domínio: `LancamentoCriadoEvent`
   - Interfaces de repositórios

2. **FluxoCaixa.Application**: Contém a lógica de aplicação
   - DTOs: `LancamentoDto`, `ConsolidadoDiarioDto`
   - Serviços: `LancamentoService`, `ConsolidadoService`
   - Interfaces: `ILancamentoService`, `IConsolidadoService`, etc.

3. **FluxoCaixa.Infrastructure**: Contém implementações técnicas
   - Repositórios: `LancamentoRepository`, `ConsolidadoRepository`
   - Comunicação: `RabbitMQEventBus`, `RabbitMQConsumer`
   - Cache: `RedisCacheService`
   - Contexto EF: `FluxoCaixaDbContext`
   - Resiliência: `CircuitBreaker`

4. **FluxoCaixa.API**: Contém os endpoints REST
   - Controllers: `LancamentosController`, `ConsolidadoController`
   - Middleware: `ErrorHandlingMiddleware`
   - Configuração: `Program.cs`, `appsettings.json`

5. **FluxoCaixa.Tests**: Contém testes unitários e de integração

## 🌟 Características Principais

### Padrões Implementados

1. **Domain-Driven Design (DDD)**
   - Modelo rico de domínio
   - Entidades com encapsulamento e validações
   - Eventos de domínio para integração

2. **CQRS (Command Query Responsibility Segregation)**
   - Separação entre operações de escrita e leitura
   - Otimização específica para cada tipo de operação

3. **Repository Pattern**
   - Abstração da persistência de dados
   - Facilidade para substituição de implementações e testes

4. **Circuit Breaker**
   - Proteção contra falhas em cascata
   - Auto-recuperação de serviços

### Requisitos Não-Funcionais Atendidos

1. **Alta disponibilidade**
   - Serviço de lançamentos independente do serviço de consolidado
   - Mecanismos de cache para redução de carga no banco

2. **Throughput**
   - Processamento de 50 requisições por segundo no consolidado
   - Máximo de 5% de perda em picos de demanda

3. **Resiliência**
   - Comunicação assíncrona entre serviços
   - Mecanismos de retry e circuit breaker

## 📝 Evoluções Futuras

- Autenticação e autorização (JWT)
- Dashboard para visualização de indicadores financeiros
- Exportação de relatórios em diversos formatos (PDF, Excel)
- Aplicativo mobile para gestão em campo
- Alertas e notificações para eventos específicos

## 🧪 Executando os Testes

Via Visual Studio:
- Use o Test Explorer para executar os testes

Via linha de comando:
```
cd FluxoCaixa.Tests
dotnet test
```
