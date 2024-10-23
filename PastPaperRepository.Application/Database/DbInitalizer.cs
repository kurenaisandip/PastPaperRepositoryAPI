using Dapper;

namespace PastPaperRepository.Application.Database;

public class DbInitalizer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitalizer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            var createPastPaperTable = @"
               CREATE TABLE IF NOT EXISTS PastPapers (
                PastPaperId TEXT PRIMARY KEY,
                Title TEXT NOT NULL,
                Slug TEXT NOT NULL UNIQUE,
                SubjectId INTEGER NOT NULL,
                CategoryId INTEGER NOT NULL,
                Year INTEGER NOT NULL,
                ExamType TEXT NOT NULL,
                DifficultyLevel TEXT NOT NULL,
                ExamBoard TEXT NOT NULL,
                FilePath TEXT NOT NULL
);";
            await connection.ExecuteAsync(createPastPaperTable);
            
            await connection.ExecuteAsync("""
                                          create unique index if not exists IX_PastPapers_Slug on PastPapers(Slug);
                                          """);
        }
    }
}