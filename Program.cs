using System.Data.SqlClient;
using Dapper;
using WebAppTest.Model;

var builder = WebApplication.CreateBuilder(args);
var application = builder.Build();
const string dbConnection =
    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LailaiTest;Integrated Security=True;";



InMemoryDbTest(application);
SqlDatabaseTest(application, dbConnection);


application.UseStaticFiles();
application.Run();

void InMemoryDbTest(WebApplication app)
{
    var Db = new List<TestClass>
    {
        new TestClass(Guid.Empty, "Jan Banan", "En lodden kanin uten halefjær"),
        new TestClass(Guid.Empty, "Asken Espeladd", "Kappeter supreme"),
        new TestClass(Guid.Empty, "World", "Sier \"Hello\""),
    };
    app.MapGet("/test", () => Db);

    app.MapPost("/test", (TestClass testClass) => { Db.Add(testClass); });
    app.MapPut("/test", (TestClass editItem) =>
    {
        var itemToEdit = Db.FirstOrDefault(i => i.Id == editItem.Id);
        itemToEdit.Name = editItem.Name;
        itemToEdit.Description = editItem.Description;
    });
    app.MapDelete("/test/{itemId}", (Guid itemId) =>
    {
        var itemToRemove = Db.FirstOrDefault(i => i.Id == itemId);
        Db.Remove(itemToRemove);
    });
}
void SqlDatabaseTest(WebApplication app, string connString)
{
    app.MapGet("/sqlTest", async() =>
    {
        var connection = new SqlConnection(connString);
        const string sql = "SELECT * FROM TestDb";
        return await connection.QueryAsync<TestClass>(sql);
    });
    app.MapPost("/sqlTest", async (TestClass testClass) =>
    {
        var connection = new SqlConnection(connString);
        const string sql = "INSERT TestDb VALUES (@Id, @Name, @Description )";
        var numberOfRows = await connection.ExecuteAsync(sql, testClass);
        return numberOfRows;
    });
    app.MapPut("/sqlTest", async (TestClass testClass) =>
    {
        var connection = new SqlConnection(connString);
        const string sql = "UPDATE TestDb SET Name = @Name, Description = @Description WHERE ID = @Id";
        var numberOfRows = await connection.ExecuteAsync(sql, testClass);
        return numberOfRows;
    });
    app.MapDelete("/sqlTest/{id}", async (Guid id) =>
    {
        var connection = new SqlConnection(connString);
        const string sql = "DELETE FROM TestDb WHERE Id = @Id";
        var numberOfRows = await connection.ExecuteAsync(sql, new { Id = id });
        return numberOfRows;
    });
}