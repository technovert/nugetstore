using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ORM.Core
{
    /// <summary>
    /// Base Respository class
    /// </summary>
    /// <seealso cref="ORM.Core.IRepository" />
    public abstract class BaseRepository : IRepository
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDbConnection Connection
        {
            get { return _sharedConnection; }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        private string ProviderName { get; set; }

        /// <summary>
        /// The shared connection
        /// </summary>
        private IDbConnection _sharedConnection;

        /// <summary>
        /// The shared connection depth
        /// </summary>
        private int _sharedConnectionDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository" /> class.
        /// </summary>
        protected BaseRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <exception cref="InvalidOperationException"></exception>
        protected BaseRepository(string connectionStringName)
        {
            var entry = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (entry == null)
                throw new InvalidOperationException(string.Format("Can't find a connection string with the name '{0}'", connectionStringName));

            this.ProviderName = !string.IsNullOrEmpty(entry.ProviderName) ? entry.ProviderName : "System.Data.SqlClient";

            this.ConnectionString = entry.ConnectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        protected BaseRepository(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection is null");

            _sharedConnection = connection;
            this.ConnectionString = connection.ConnectionString;

            // Prevent closing external connection
            _sharedConnectionDepth = 2;
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
        public virtual IEnumerable<T> Query<T>(string sql, params object[] args) where T : class
        {
            OpenSharedConnection();

            try
            {
                using (IDbCommand cmd = _sharedConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParam(args);

                    IDataReader reader = cmd.ExecuteReader();

                    return reader.DataReaderMapToList<T>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (Update table set column1 = @column1 where column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public virtual int Execute(string sql, params object[] args)
        {
            OpenSharedConnection();

            try
            {
                using (IDbCommand cmd = _sharedConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParam(args);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSharedConnection();
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
        /// </returns>
        /// <example> please use below syntax for parameters: 
        /// sql - (INSERT INTO table (column1, column2) VALUES (@column1, @column2))
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public virtual T Insert<T>(string sql, params object[] args) where T : class
        {
            OpenSharedConnection();

            try
            {
                using (IDbCommand cmd = _sharedConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParam(args);

                    object val = cmd.ExecuteScalar();

                    // Handle nullable types
                    Type u = Nullable.GetUnderlyingType(typeof(T));
                    if (u != null && (val == null || val == DBNull.Value))
                        return default(T);

                    return (T)Convert.ChangeType(val, u == null ? typeof(T) : u);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        /// <summary>
        /// Gets the current connection.
        /// </summary>
        /// <returns>
        /// The DB connection.
        /// </returns>
        public virtual IDbConnection GetCurrentConnection()
        {
            return this.Connection;
        }

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <returns>
        /// The DB connection state.
        /// </returns>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        public ConnectionState GetConnectionState()
        {
            if (this.GetCurrentConnection() == null)
            {
                throw new ArgumentNullException("connection is null");
            }

            return this.GetCurrentConnection().State;
        }

        /// <summary>
        /// Fetches the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// A List holding the results of the query.
        /// </returns>
        /// <exception cref="ArgumentException">source is null.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public List<T> Fetch<T>(string sql, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("source is null.");
            }
        }

        /// <summary>
        /// Singles the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The single record matching the specified primary key value.
        /// </returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element.-or-The input sequence is empty.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T Single<T>(string sql, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).Single();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source is null.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The input sequence contains more than one element.-or-The input sequence is empty.");
            }
        }

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The single record matching the specified primary key value, or default(T) if no matching rows.
        /// </returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T SingleOrDefault<T>(string sql, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).SingleOrDefault();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source is null.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The input sequence contains more than one element.");
            }
        }

        /// <summary>
        /// Firsts the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The first record in the result set.
        /// </returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T First<T>(string sql, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).First();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source is null.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The source sequence is empty.");
            }
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The first record in the result set, or default(T) if no matching rows.
        /// </returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T FirstOrDefault<T>(string sql, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).FirstOrDefault();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source is null.");
            }
        }

        /// <summary>
        /// Fetches the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// A List holding the results of the query.
        /// </returns>
        /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public List<T> Fetch<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).Where(predicate).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source or predicate is null.");
            }
        }

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
        /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in predicate.-or-More than one element satisfies the condition in predicate.-or-The source sequence is emp.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T Single<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).Single(predicate);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source or predicate is null.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("No element satisfies the condition in predicate.-or-More than one element satisfies the condition in predicate.-or-The source sequence is emp.");
            }
        }

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The single record matching the specified primary key value, or default(T) if no matching rows.
        /// </returns>
        /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T SingleOrDefault<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).SingleOrDefault(predicate);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source or predicate is null.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The input sequence contains more than one element.");
            }
        }

        /// <summary>
        /// Firsts the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The first record in the result set.
        /// </returns>
        /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in predicate.-or-The source sequence is empty.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T First<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).First(predicate);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source or predicate is null.");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("No element satisfies the condition in predicate.-or-The source sequence is empty.");
            }
        }

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
        /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
        /// <example> please use below syntax for parameters: 
        /// sql - (select * from table where column1 = @column1 and column2 = @column2)
        /// args - (new {column1 = value}, new {column2 = value})
        /// </example>
        public T FirstOrDefault<T>(string sql, Func<T, bool> predicate, params object[] args) where T : class
        {
            try
            {
                return this.Query<T>(sql, args).FirstOrDefault(predicate);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("source or predicate is null.");
            }
        }

        /// <summary>
        /// Opens the shared connection.
        /// </summary>
        protected void OpenSharedConnection()
        {
            if (_sharedConnectionDepth == 0)
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(this.ProviderName);
                _sharedConnection = _factory.CreateConnection();
                _sharedConnection.ConnectionString = this.ConnectionString;

                if (_sharedConnection.State == ConnectionState.Broken)
                    _sharedConnection.Close();

                if (_sharedConnection.State == ConnectionState.Closed)
                    _sharedConnection.Open();
            }

            _sharedConnectionDepth++;
        }

        /// <summary>
        /// Closes the shared connection.
        /// </summary>
        protected void CloseSharedConnection()
        {
            if (_sharedConnectionDepth > 0)
            {
                _sharedConnectionDepth--;
                if (_sharedConnectionDepth == 0)
                {
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }
            }
        }
    }
}
