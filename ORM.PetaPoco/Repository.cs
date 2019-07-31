using ORM.Core;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ORM.PetaPoco
{
    /// <summary>
    /// PetaPoco Repository.
    /// </summary>
    /// <seealso cref="ORM.Core.BaseRepository" />
    /// <seealso cref="ORM.Core.IRepository" />
    public class Repository : BaseRepository, IRepository
    {
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        private Database DB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        public Repository()
        {
            this.DB = new Database();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public Repository(string connectionStringName)
        {
            this.DB = new Database(connectionStringName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="defaultMapper">The default mapper.</param>
        public Repository(IDbConnection connection, IMapper defaultMapper = null)
        {
            this.DB = new Database(connection, defaultMapper);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerName">Name of the provider.</param>
        public Repository(string connectionString, string providerName)
        {
            this.DB = new Database(connectionString, providerName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="factory">The factory.</param>
        public Repository(string connectionString, DbProviderFactory factory)
        {
            this.DB = new Database(connectionString, factory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultMapper">The default mapper.</param>
        public Repository(string connectionString, IProvider provider, IMapper defaultMapper = null)
        {
            this.DB = new Database(connectionString, provider, defaultMapper);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Repository(IDatabaseBuildConfiguration configuration)
        {
            this.DB = new Database(configuration);
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// An enumerable collection of result records.
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public override IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            try
            {
                return this.DB.Query<T>(sql, args);
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                return this.DB.Execute(sql, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the current connection.
        /// </summary>
        /// <returns>
        /// The DB Connection.
        /// </returns>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        public override IDbConnection GetCurrentConnection()
        {
            if (this.DB == null)
            {
                throw new ArgumentNullException("connection is null");
            }

            return this.DB.Connection;
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
            try
            {
                return this.DB.ExecuteScalar<T>(sql, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
