using ORM.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ORM.EF
{
    /// <summary>
    /// Entity Framework Repository
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
            using (var context = this.CreateContext())
            {
                try
                {
                    return context.Database.SqlQuery<T>(sql, args).ToList();
                }
                catch(Exception ex){
                    throw ex;
                }
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
            using (var context = this.CreateContext())
            {
                try
                {
                    return context.Database.ExecuteSqlCommand(sql, args);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns>The DB context.</returns>
        private DbContext CreateContext()
        {
            return new DbContext(this.ConnectionString);
        }
    }
}
