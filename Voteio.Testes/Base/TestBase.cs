public abstract class TestBase
{
    protected static Usuario CreateTestUser()
    {
        return new Usuario
        {
            Id = 1,
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "hashedPassword123",
            Codigo = Guid.NewGuid(),
            Situacao = Situacao.Ativo
        };
    }

    protected static CadastrarUsuarioRequest CreateTestUserRequest()
    {
        return new CadastrarUsuarioRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "password123"
        };
    }

    protected static LoginRequest CreateTestLoginRequest()
    {
        return new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
    }
} 