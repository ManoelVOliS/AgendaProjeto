# Projeto Agenda Full-Stack (CRUD)

Este é um projeto de uma aplicação CRUD (Create, Read, Update, Delete) completa para uma agenda de contatos.

O projeto é construído usando uma arquitetura de microsserviços desacoplada, com um backend .NET 9 (API RESTful) e um frontend (Vue.js 3). A aplicação foi containerizada com Docker.

## Tecnologias Utilizadas

### Backend (.NET)
* **.NET 9** (C#)
* **ASP.NET Core Web API**
* **Entity Framework Core 9** (ORM)
* **PostgreSQL** (Banco de Dados)
* **Arquitetura Limpa** (Camadas `Core`, `Infrastructure`, `Api`)
* **Padrões de Projeto:** Repositório (Repository Pattern) e Serviço (Service Layer)
* **Libs:** AutoMapper, FluentValidation
* **Testes:** xUnit e Moq
* **Containerização:** Docker

### Frontend (Vue.js)
* **Vue.js 3** (Composition API)
* **Vite** (Build tool)
* **Axios** (Cliente HTTP)
* **PrimeVue** (Biblioteca de componentes UI, incluindo tema e ícones)

---

## Pré-requisitos

Para executar este projeto, você precisará ter as seguintes ferramentas instaladas:

1.  **[.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)** (ou superior)
2.  **[Node.js](https://nodejs.org/en)** (Versão LTS recomendada)
3.  **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (Para rodar os containers)
4.  Um cliente Git (para clonar o repositório)

---

## Como Executar o Projeto (Backend no Docker)

Esta é a forma recomendada de rodar o projeto, pois utiliza os containers Docker para o backend (API) e o banco de dados (Postgres), e roda o frontend localmente para desenvolvimento.

### 1. Iniciar o Container do Banco de Dados (Postgres)

Primeiro, precisamos de um banco de dados rodando. Este comando irá "puxar" a imagem oficial do Postgres e iniciá-la num container chamado `pg-agenda` na porta `5432`.

```bash
docker run --name pg-agenda -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=agenda_db -p 5432:5432 -d postgres
```
### 2. Aplicar as Migrações (Criar as Tabelas)

O container do banco de dados está rodando, mas está **vazio**. Precisamos aplicar as nossas migrações (criadas com o Entity Framework) para criar as tabelas (`Contacts` e `__EFMigrationsHistory`).

Rode este comando da **raiz do projeto** (`AgendaProjeto`) para que o seu .NET local se conecte ao container Docker e crie as tabelas:

```bash
dotnet ef database update --project Agenda.Infrastructure --startup-project Agenda.Api
```
### 3. Construir a Imagem da API (.NET)

Agora, vamos "empacotar" a nossa API .NET. Este comando lê o `Dockerfile` na raiz do projeto e constrói uma imagem Docker local chamada `agenda-api`.

```bash
docker build -t agenda-api .
````
### 4. Iniciar o Container da API

Com a imagem construída e o banco de dados pronto, inicie o container da API na porta `8080`:

```bash
docker run -d -p 8080:8080 --name agenda-app \
  -e "ASPNETCORE_ENVIRONMENT=Development" \
  -e "ConnectionStrings:DefaultConnection=Host=host.docker.internal;Port=5432;Database=agenda_db;Username=admin;Password=admin" \
  agenda-api
```
O backend está agora 100% operacional.

API: http://localhost:8080

Swagger UI: http://localhost:8080/swagger

## Iniciar o Frontend (Vue.js)
---
Finalmente, em **outro terminal**, inicie o servidor de desenvolvimento do Vue:

```bash
# 1. Entre na pasta do frontend
cd agenda-frontend

# 2. Instale as dependências (se for a primeira vez)
npm install

# 3. Inicie o servidor
npm run dev
```

A sua aplicação estará disponível em http://localhost:5173 (ou a porta que o Vite indicar)

