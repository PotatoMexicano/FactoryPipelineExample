# 📦 API de Produtos

## Obs.: Projeto de portfólio

## 📌 Visão Geral
Esta é uma API REST para gestão de produtos, que inclui autenticação via **JWT**, suporte a **pipeline para customização de DTOs**, e endpoints protegidos para operações CRUD de produtos.

## 🚀 Tecnologias Utilizadas
- **ASP.NET Core**
- **JWT (JSON Web Token) para autenticação**
- **Entity Framework Core** (para repositório de dados)
- **Pipeline de customização de DTOs**

## 🔐 Autenticação
Os endpoints estão protegidos com **JWT**, sendo necessário um token válido para acessá-los.
- **Usuários precisam estar autenticados** para consumir qualquer endpoint.
- O cadastro de produtos requer permissões de **Admin**.

## 📄 Endpoints

### 🔍 Obter Produto por ID
```http
GET /api/product/{idProduct}
```
**Requer autenticação:** ✅ Sim

#### 📥 Parâmetros
| Nome       | Tipo    | Obrigatório | Descrição                          |
|------------|--------|-------------|---------------------------------|
| idProduct  | int    | ✅ Sim      | ID do produto a ser recuperado |
| idEmpresa  | int?   | ❌ Não      | Personaliza DTO conforme empresa |

#### 📤 Resposta de Sucesso
```json
{
  "id": 1,
  "nome": "Produto A",
  "preco": 9999
}
```

📌 **Observação:** O preço dos produtos é armazenado em centavos (*inteiro*), portanto, `9999` representa **R$ 99,99**.

---

### 📋 Listar Produtos
```http
GET /api/product
```
**Requer autenticação:** ✅ Sim

#### 📥 Parâmetros
| Nome       | Tipo    | Obrigatório | Descrição                          |
|------------|--------|-------------|---------------------------------|
| idEmpresa  | int?   | ❌ Não      | Personaliza DTO conforme empresa |

#### 📤 Resposta de Sucesso
```json
[
  { "id": 1, "nome": "Produto A", "preco": 9999 },
  { "id": 2, "nome": "Produto B", "preco": 14999 }
]
```

📌 **Observação:** O preço dos produtos é armazenado em centavos (*inteiro*), portanto, `9999` representa **R$ 99,99**.

---

### ➕ Cadastrar um Produto
```http
POST /api/product
```
**Requer autenticação:** ✅ Sim (Apenas **Admin**)

#### 📥 Corpo da Requisição
```json
{
  "nome": "Novo Produto",
  "preco": 12999
}
```

#### 📤 Resposta de Sucesso
```json
{
  "id": 3,
  "nome": "Novo Produto",
  "preco": 12999
}
```

📌 **Observação:** O preço dos produtos é armazenado em centavos (*inteiro*), portanto, `12999` representa **R$ 129,99**.

---

## ⚙️ Pipeline de DTOs
A API conta com um **pipeline de customização de DTOs**, permitindo que cada empresa obtenha um formato de resposta adaptado. Isso é feito através da interface **`IEmpresaFactory`**, que gera os DTOs dinamicamente.

## 🛠 Como Rodar o Projeto
1. Clone o repositório:
   ```sh
   git clone https://github.com/seu-usuario/seu-repositorio.git
   ```
2. Instale as dependências:
   ```sh
   dotnet restore
   ```
3. Configure a **string de conexão** no `appsettings.json`.
4. Execute a API:
   ```sh
   dotnet run
   ```

## 🏆 Contribuição
Sinta-se à vontade para abrir issues ou enviar PRs para melhorias! 😃

