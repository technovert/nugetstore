using Dapper;
using ORM.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ORM.Dapper
{
    /// <summary>
    /// Dapper Repository
    /// </summary>
    /// <seealso cref="ORM.Core.BaseRepository" />
    /// <seealso cref="ORM.Core.IRepository" />
    public class Repository : BaseRepository, IRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public Repository(string connectionStringName) : base(connectionStringName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public Repository(IDbConnection connection) : base(connection)
        {
        }

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
        public override IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            this.OpenSharedConnection();

            try
            {
                return this.Connection.Query<T>(sql, args).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseSharedConnection();
            }
        }

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
        public override int Execute(string sql, params object[] args)
        {
            this.OpenSharedConnection();

            try
            {
                return this.Connection.Execute(sql, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseSharedConnection();
            }
        }

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
        public override T Insert<T>(string sql, params object[] args)
        {
            this.OpenSharedConnection();

            try
            {
                return this.Connection.ExecuteScalar<T>(sql, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseSharedConnection();
            }
        }
    }
}
