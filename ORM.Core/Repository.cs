using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Core
{
    /// <summary>
    /// Core Repository class
    /// </summary>
    /// <seealso cref="ORM.Core.BaseRepository" />
    /// <seealso cref="ORM.Core.IRepository" />
    public class Repository: BaseRepository, IRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        public Repository(): base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public Repository(string connectionStringName): base(connectionStringName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public Repository(IDbConnection connection): base(connection)
        {
        }
    }
}
