using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TiendaVirtual.Entidades;

namespace TiendaVirtual.AccesoDatos
{
    class DaoProductoSQLServer : IDaoProducto
    {
        private const string SQL_INSERT = "INSERT INTO productos (Nombre, Precio) VALUES (@Nombre, @Precio)";
        private const string SQL_DELETE = "DELETE FROM productos WHERE Id=@Id";
        private const string SQL_UPDATE = "UPDATE productos SET Nombre=@Nombre,Precio=@Precio WHERE Id=@Id";
        private const string SQL_SELECT = "SELECT Id, Nombre, Precio FROM productos";
        private const string SQL_SELECT_ID = "SELECT Id, Nombre, Precio FROM productos WHERE Id=@Id";
        private const string SQL_SELECT_NICK = "SELECT Id, Nombre, Precio FROM productos WHERE Nick=@Nick";

        private string connectionString;

        public DaoProductoSQLServer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DaoProductoSQLServer()
        {
            this.connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\xampp\htdocs\TiendaVirtual\PresentacionAspNetMvc\App_Data\TiendaVirtualSQL.mdf; Integrated Security = True";
        }

        public void Alta(IProducto producto)
        {
            try
            {
                using (IDbConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    //"Zona declarativa"
                    con.Open();

                    IDbCommand comInsert = con.CreateCommand();

                    comInsert.CommandText = SQL_INSERT;

                    IDbDataParameter parNombre = comInsert.CreateParameter();
                    parNombre.ParameterName = "Nombre";
                    parNombre.DbType = DbType.String;

                    IDbDataParameter parPrecio = comInsert.CreateParameter();
                    parPrecio.ParameterName = "Precio";
                    parPrecio.DbType = DbType.String;

                    comInsert.Parameters.Add(parNombre);
                    comInsert.Parameters.Add(parPrecio);

                    //"Zona concreta"
                    parNombre.Value = producto.Nombre;
                    parPrecio.Value = producto.Precio;

                    int numRegistrosInsertados = comInsert.ExecuteNonQuery();

                    if (numRegistrosInsertados != 1)
                        throw new AccesoDatosException("Número de registros insertados: " +
                            numRegistrosInsertados);
                }
            }
            catch (Exception e)
            {
                throw new AccesoDatosException("No se ha podido realizar el alta", e);
            }
        }

        public void Baja(IProducto producto)
        {
            Baja(producto.Id);
        }

        public void Baja(int id)
        {
            try
            {
                using (IDbConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    //"Zona declarativa"
                    con.Open();

                    IDbCommand comDelete = con.CreateCommand();

                    comDelete.CommandText = SQL_DELETE;

                    IDbDataParameter parId = comDelete.CreateParameter();
                    parId.ParameterName = "Id";
                    parId.DbType = DbType.Int32;

                    comDelete.Parameters.Add(parId);

                    //"Zona concreta"
                    parId.Value = id;

                    int numRegistrosBorrados = comDelete.ExecuteNonQuery();

                    if (numRegistrosBorrados != 1)
                        throw new AccesoDatosException("Número de registros borrados: " +
                            numRegistrosBorrados);
                }
            }
            catch (Exception e)
            {
                throw new AccesoDatosException("No se ha podido realizar el borrado", e);
            }
        }

        public IProducto BuscarPorId(int id)
        {
            try
            {
                using (IDbConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    //"Zona declarativa"
                    con.Open();

                    IDbCommand comSelectId = con.CreateCommand();

                    comSelectId.CommandText = SQL_SELECT_ID;

                    IDbDataParameter parId = comSelectId.CreateParameter();
                    parId.ParameterName = "Id";
                    parId.DbType = DbType.Int32;

                    comSelectId.Parameters.Add(parId);

                    //"Zona concreta"

                    parId.Value = id;

                    IDataReader dr = comSelectId.ExecuteReader();

                    if (dr.Read())
                    {
                        IProducto producto = new Producto();

                        producto.Id = dr.GetInt32(0);
                        producto.Nombre = dr.GetString(1);
                        producto.Precio = dr.GetDecimal(2);

                        return producto;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                throw new AccesoDatosException("No se ha podido buscar ese usuario por ese id", e);
            }
        }

        public IEnumerable<IProducto> BuscarTodos()
        {
            List<IProducto> productos = new List<IProducto>();

            try
            {
                using (IDbConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    //"Zona declarativa"
                    con.Open();

                    IDbCommand comSelect = con.CreateCommand();

                    comSelect.CommandText = SQL_SELECT;

                    //"Zona concreta"
                    IDataReader dr = comSelect.ExecuteReader();

                    IProducto producto;

                    while (dr.Read())
                    {
                        producto = new Producto();

                        producto.Id = dr.GetInt32(0);
                        producto.Nombre = dr.GetString(1);
                        producto.Precio = dr.GetDecimal(2);

                        productos.Add(producto);
                    }

                    return productos;
                }
            }
            catch (Exception e)
            {
                throw new AccesoDatosException("No se ha podido buscar todos los usuarios", e);
            }
        }

        public void Modificacion(IProducto producto)
        {
            try
            {
                using (IDbConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    //"Zona declarativa"
                    con.Open();

                    IDbCommand comUpdate = con.CreateCommand();

                    comUpdate.CommandText = SQL_UPDATE;

                    IDbDataParameter parId = comUpdate.CreateParameter();
                    parId.ParameterName = "Id";
                    parId.DbType = DbType.Int32;

                    IDbDataParameter parNombre = comUpdate.CreateParameter();
                    parNombre.ParameterName = "Nombre";
                    parNombre.DbType = DbType.String;

                    IDbDataParameter parPrecio = comUpdate.CreateParameter();
                    parPrecio.ParameterName = "Precio";
                    parPrecio.DbType = DbType.String;

                    comUpdate.Parameters.Add(parId);
                    comUpdate.Parameters.Add(parNombre);
                    comUpdate.Parameters.Add(parPrecio);

                    //"Zona concreta"
                    parId.Value = producto.Id;
                    parNombre.Value = producto.Nombre;
                    parPrecio.Value = producto.Precio;

                    int numRegistrosModificados = comUpdate.ExecuteNonQuery();

                    if (numRegistrosModificados != 1)
                        throw new AccesoDatosException("Número de registros modificados: " +
                            numRegistrosModificados);
                }
            }
            catch (Exception e)
            {
                throw new AccesoDatosException("No se ha podido realizar la modificación", e);
            }
        }
    }
}
