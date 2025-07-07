# OperationsDataBase
A library of CRUD command abstractions for working with IMediatR.

1.Create user data transfer object:

public record UserDto(string Name, string HashPass, int Id = 0) : BaseDto(Id);

2. The BaseCommandsOperations folder contains basic CRUD requests. Create a command to create an entity in the database:


public sealed record CreateUserRequest(UserDto NewUserDto) : Create<UserDto>(NewUserDto)
{
    public override string ToString()
    {
        string name = $"{nameof(CreateUserRequest)} {NewUserDto.Name}";
        return name;
    }
}

Create handler:
public record CreateUserRequestHandler : IRequestHandler<CreateUserRequest, IStatusGeneric>
{
    private readonly ILogger<CreateWordRequestHandler> _logger;

    public CreateUserRequestHandler(IServiceProvider serviceProvider, ILogger<CreateWordRequestHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<IStatusGeneric> Handle(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{createUserRequest} Started");
        StatusGenericHandler status = new StatusGenericHandler();
        return status;
    }
}

3. Reading request:
public record GetUsersRequest() : 
        ReadCommandRequest<User,UserDto>
{
    public override string ToString()
    {
        string name = $"{nameof(GetUsersRequest)}";
        return name;
    }
}

internal record GetUsersRequestHandler : IRequestHandler<GetUsersRequest, IStatusGeneric<IEnumerable<UserDto>>>
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<GetWordsRequestHandler> _logger;

    public GetUsersRequestHandler(IServiceProvider provider, ILogger<GetWordsRequestHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<IStatusGeneric<IEnumerable<UserDto>>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{request} Started");
        var status = new StatusGenericHandler<IEnumerable<UserDto>>();
        IEnumerable<UserDto> result = null!;
        await using var scope = _provider.CreateAsyncScope();
        try
        {
            var query =
                scope.ServiceProvider
                    .GetRequiredService<IEntityCollection<User>>().Get(request);

            if (!await query.AnyAsync(cancellationToken: cancellationToken))
            {
                status.AddError("items is empty");
                return status;
            }

            var list = await query
                .Select(x =>
                    new
                    {
                        n = x.UserName,
                        p = x.PasswordHash,
                        id = x.Id
                    }).ToListAsync(cancellationToken: cancellationToken);

            result = list
                .Select(x => new UserDto(x.n, x.p, Id: x.id));
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        _logger.LogInformation($"{request} Finish");
        return status.SetResult(result);
    }
}


4. Add service in service collection:
     public static IServiceCollection AddRepository(this IServiceCollection serviceCollection)
        {
           return serviceCollection
               .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
               .AddScoped<IRepository, DataBaseOperationHelper.RepositoryService.Repository>();
            ;
        } 

5. How use:

a. 
    public async Task<IStatusGeneric> Register(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        CreateUserRequest createUserRequestCreateCommand = new CreateUserRequest(
            new UserDto(userName,
                _hashService.Hash(password).GetHashed()));

        return await _repository.DataBaseOperationAsync(createUserRequestCreateCommand, cancellationToken);
    }


b.

 public async Task<IStatusGeneric<string>> LogIn(string userName, string password, CancellationToken cancellationToken = default)
    {
        var status = new StatusGenericHandler<string>()
        {
            Header = $"User authentication {userName}"
        };

        GetUsersRequest usersRequest = new GetUsersRequest()
        {
            Filters = new List<Expression<Func<User, bool>>>()
            {
                x => x.UserName == userName,
            },
            Includes = new List<Expression<Func<User, object>>>()
            {
            },
            Size = 1,
            ZeroStart = 0
        };
        IStatusGeneric<IEnumerable<UserDto>> getUserCommand = await _repository.GetItemsAsync(usersRequest, cancellationToken);
        if (getUserCommand.HasErrors)
            return status.AddError(string.Join(";", getUserCommand.Errors));

        UserDto userInDataBase = getUserCommand.Result.Single();

        if (!_hashService.CreateValidating(userInDataBase.HashPass).Validate(password))
            return status.AddError("Неправильный пароль");

        return status.SetResult(_generateToken.Generate(userInDataBase));
    }

c. Extensions Build an expression with a union using OR:

   var keywords = new[] { "Bob", "Dan", "Marry" };
        Expression<Func<User, string>> prop = f => f.UserName;

        Expression<Func<User, bool>> filterExpr = keywords.BuildContainsOrExpression(prop);

        GetUsersRequest usersRequest = new GetUsersRequest()
        {
            Filters = new List<Expression<Func<User, bool>>>()
            {
                filterExpr
            },
            Includes = new List<Expression<Func<User, object>>>()
            {
            },
            Size = 1,
            ZeroStart = 0
        };

 

