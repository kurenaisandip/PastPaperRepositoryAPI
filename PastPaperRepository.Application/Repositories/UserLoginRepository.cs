using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class UserLoginRepository : IUserLoginRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserLoginRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> Login(UsersLogin userLogin, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND Password = @Password";
                    var result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(query,
                        new { userLogin.Email, userLogin.Password }, transaction, cancellationToken: token));

                    if (result == 0) return false;

                    transaction.Commit();
                    return result > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<bool> Register(UsersLogin userLogin, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Check if the email already exists
                    var checkQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    var userExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
                            checkQuery,
                            new { userLogin.Email },
                            transaction,
                            cancellationToken: token
                        )
                    );

                    if (userExists > 0) return false; // User already exists

                    // Insert the new user
                    var insertQuery =
                        "INSERT INTO Users (user_name, email, password, role_id) VALUES (@Name, @Email, @Password, @Role)";
                    var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(
                            insertQuery,
                            new
                            {
                                userLogin.Name,
                                userLogin.Email,
                                userLogin.Password,
                                Role = 2
                            },
                            transaction,
                            cancellationToken: token
                        )
                    );

                    // Commit the transaction if the insertion is successful
                    transaction.Commit();
                    return rowsAffected > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<bool> SaveLoggedInUserDetails(LoggedInUserDetails userDetails, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = @"
                    INSERT INTO LoggedinUser 
                    (user_id, institution_name, phone_number, gradeId, academic_background, optional_subject, IsUserDataComplete) 
                    VALUES (@UserId, @InstitutionName, @PhoneNumber, @Grade, @AcademicBackground, @Semester, @IsCompleted)";

                    var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(
                        query,
                        new
                        {
                            userDetails.UserId,
                            userDetails.InstitutionName,
                            userDetails.PhoneNumber,
                            userDetails.Grade,
                            userDetails.AcademicBackground,
                            userDetails.Semester,
                            IsCompleted = 1
                        },
                        transaction,
                        cancellationToken: token
                    ));

                    transaction.Commit();
                    return rowsAffected > 0;
                }
                catch (Exception e)
                {
                    transaction.Rollback(); // Rollback transaction in case of an error
                    Console.WriteLine($"Error saving user details: {e.Message}");
                    throw;
                }
            }
        }
    }

    public async Task<UserClaimModel> GetUserClaimModel(string email, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = @"
                    select 
                        u.user_id as Id, 
                        l.IsUserDataComplete,
                        u.user_name as Name,
                        case 
                            when p.user_id is not null then 'paid'
                            else 'unpaid'
                        end as UserType
                    from Users as u
                    left join Payments as p on u.user_id = p.user_id
                    left join LoggedInUser as l on u.user_id = l.user_id
                    where u.email = @Email";

                    // Use an anonymous object to bind the email parameter
                    var user = await connection.QuerySingleOrDefaultAsync<UserClaimModel>(
                        new CommandDefinition(query, new { Email = email }, transaction, cancellationToken: token));

                    // If no user is found, return null or throw an exception as per your logic
                    if (user == null)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();
                    return user;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    //This method returns the user claim model when user submits the model data
    public async Task<UserClaimModel> ReturnUserClaimModel(int userId, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = @"
                    select 
                        u.user_id as Id, 
                        l.IsUserDataComplete,
                        u.user_name as Name,
                        case 
                            when p.user_id is not null then 'paid'
                            else 'unpaid'
                        end as UserType
                    from Users as u
                    left join Payments as p on u.user_id = p.user_id
                    left join LoggedInUser as l on u.user_id = l.user_id
                    where u.user_id = @UserId";

                    var user = await connection.QuerySingleOrDefaultAsync<UserClaimModel>(
                        new CommandDefinition(query, new { UserId = userId }, transaction, cancellationToken: token));

                    if (user == null)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();
                    return user;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}