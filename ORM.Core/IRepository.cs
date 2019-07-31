using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Core
{
    /// <summary>
    /// Repository interface.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>An enumerable collection of result records.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        IEnumerable<T> Query<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Fetches the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>A List holding the results of the query.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        List<T> Fetch<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Singles the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The single record matching the specified primary key value.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T Single<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The single record matching the specified primary key value, or default(T) if no matching rows.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T SingleOrDefault<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Firsts the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The first record in the result set.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T First<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The first record in the result set, or default(T) if no matching rows.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T FirstOrDefault<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Fetches the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>A List holding the results of the query.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        List<T> Fetch<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class;

        /// <summary>
        /// Singles the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The single record matching the specified primary key value.
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T Single<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class;

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The single record matching the specified primary key value, or default(T) if no matching rows.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T SingleOrDefault<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class;

        /// <summary>
        /// Firsts the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The first record in the result set.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T First<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class;

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The first record in the result set, or default(T) if no matching rows.
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T FirstOrDefault<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class;

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The number of rows affected.</returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (Update table set column1 = @column1 where column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        int Execute(string sql, params object[] args);

        /// <summary>
        /// Inserts the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The scalar value cast to T.
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (INSERT INTO table (column1, column2) VALUES (@column1, @column2))
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        T Insert<T>(string sql, params object[] args) where T : class;

        /// <summary>
        /// Gets the current connection.
        /// </summary>
        /// <returns>The DB Connection.</returns>
        IDbConnection GetCurrentConnection();

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <returns>The DB connection state.</returns>
        ConnectionState GetConnectionState();
    }
}
