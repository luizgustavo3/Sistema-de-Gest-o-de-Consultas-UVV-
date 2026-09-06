# Sistema de Gestão de Consultas UVV

Aplicação Web em **C# / ASP.NET Core MVC** com **Entity Framework Core (Code First)** para gerenciamento de usuários e registro de consultas médicas/profissionais.

## 🎥 Vídeo demonstrativo

## 🎥 Vídeo demonstrativo

Vídeo mostrando cadastro, login e CRUD de consultas em funcionamento:

**https://youtu.be/-hesw0WE8Yw**

## Tecnologias

- ASP.NET Core 8.0 (MVC + API mínima)
- Entity Framework Core 8 (Code First + Migrations)
- MySQL (via Pomelo.EntityFrameworkCore.MySql), testável com MySQL Workbench
- Autenticação por cookie (`Microsoft.AspNetCore.Authentication.Cookies`)
- Hash de senha com `PasswordHasher<Usuario>` (Microsoft.AspNetCore.Identity)
- Swagger / Swashbuckle para testar a API

## Estrutura do projeto

```
ConsultaUVV/
├── Models/            # Usuario.cs, Consulta.cs
├── Data/              # ApplicationDbContext.cs
├── ViewModels/         # ContaViewModels.cs, ConsultaViewModel.cs
├── Controllers/        # ContaController, ConsultasController, ConsultasApiController
├── Views/              # Conta/ e Consultas/ (Razor)
├── wwwroot/css/         # site.css
├── Program.cs
└── appsettings.json
```

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL Server rodando localmente (pode ser gerenciado pelo MySQL Workbench)
- (Opcional) Ferramenta EF Core: `dotnet tool install --global dotnet-ef`

## Configuração do banco de dados

1. Abra `appsettings.json` e ajuste a connection string `DefaultConnection` com o usuário/senha do seu MySQL local:

   ```json
   "DefaultConnection": "Server=localhost;Port=3306;Database=ConsultaUVVDb;User=root;Password=SUA_SENHA_AQUI;"
   ```

   Troque `SUA_SENHA_AQUI` pela senha do usuário `root` (ou outro usuário) que você usa para conectar no MySQL Workbench. Não é preciso criar o banco `ConsultaUVVDb` manualmente — o comando `dotnet ef database update` cria ele.

2. Restaure os pacotes:

   ```bash
   dotnet restore
   ```

3. Crie a primeira migration (gera as tabelas `Usuarios` e `Consultas` a partir dos Models):

   ```bash
   dotnet ef migrations add InitialCreate
   ```

4. Aplique a migration no banco de dados:

   ```bash
   dotnet ef database update
   ```

   > Isso cria o banco `ConsultaUVVDb` (ou o nome definido na connection string) com as tabelas e o relacionamento `Consulta -> Usuario`.

5. Rode a aplicação:

   ```bash
   dotnet run
   ```

6. Acesse:
   - Aplicação (telas MVC): `https://localhost:5001` (ou a porta exibida no terminal)
   - Swagger (API de Consultas): `https://localhost:5001/swagger`

## Fluxo de uso

1. **Criar conta**: acesse `/Conta/Cadastro`, informe nome, e-mail e senha.
2. **Login**: acesse `/Conta/Login`. Após autenticar, você é redirecionado para `/Consultas`.
3. **Gerenciar consultas**: em `/Consultas`, é possível criar, editar e excluir consultas — cada usuário só vê e manipula as próprias consultas.
4. **Testar a API**: com uma sessão de cookie ativa (logado pelo navegador), abra `/swagger` e teste os endpoints `GET/POST/PUT/DELETE /api/consultas`.

## Segurança implementada

- Senhas nunca são salvas em texto puro — são armazenadas como hash (`Usuario.SenhaHash`), gerado com `PasswordHasher<Usuario>`.
- Rotas de consulta (MVC e API) protegidas com `[Authorize]`.
- `app.UseAuthentication()` configurado **antes** de `app.UseAuthorization()` em `Program.cs`.
- Validação de entrada no servidor via Data Annotations (`[Required]`, `[EmailAddress]`, `[StringLength]`, `[Compare]`) nos ViewModels e Models.
- Cada usuário só acessa/edita/exclui as próprias consultas (filtro por `UsuarioId` obtido do cookie de autenticação, nunca por um Id enviado livremente pelo cliente).

## Possíveis evoluções

- Trocar a autenticação manual por ASP.NET Core Identity completo.
- Adicionar paginação e filtros na listagem de consultas.
- Adicionar testes automatizados (xUnit) para Controllers e regras de negócio.
