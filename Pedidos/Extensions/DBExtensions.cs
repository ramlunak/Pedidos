//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Internal;
//using Microsoft.EntityFrameworkCore.Storage;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Microsoft.EntityFrameworkCore
//{
//    public static class RDFacadeExtensions
//    {
//        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
//        {
//            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

//            using (concurrencyDetector.EnterCriticalSection())
//            {
//                var rawSqlCommand = databaseFacade
//                    .GetService<IRawSqlCommandBuilder>()
//                    .Build(sql, parameters);

//                return rawSqlCommand
//                    .RelationalCommand
//                    .ExecuteReader(
//                        databaseFacade.GetService<IRelationalConnection>(),
//                        parameters: rawSqlCommand.ParameterValues);
//            }
//        }

//        public static async Task<RelationalDataReader> ExecuteSqlQueryAsync(this DatabaseFacade databaseFacade,
//                                                             string sql,
//                                                             CancellationToken cancellationToken = default(CancellationToken),
//                                                             params object[] parameters)
//        {

//            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

//            using (concurrencyDetector.EnterCriticalSection())
//            {
//                var rawSqlCommand = databaseFacade
//                    .GetService<IRawSqlCommandBuilder>()
//                    .Build(sql, parameters);

//                return await rawSqlCommand
//                    .RelationalCommand
//                    .ExecuteReaderAsync(
//                        databaseFacade.GetService<IRelationalConnection>(),
//                        parameterValues: rawSqlCommand.ParameterValues,
//                        cancellationToken: cancellationToken);
//            }
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class DBHelper
{
    private readonly AppDbContext _context;

    public DBHelper()
    {
    }

    public DBHelper(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<P_Productos>> ProductosFromCmd(string query)
    {
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var result = await command.ExecuteReaderAsync())
            {
                var entities = new PreencheEntidade<P_Productos>().GetListaEntidade(result);             
                return entities;
            }
        }
    }

    //public static async Task<List<T>> CustomDbQuery<T>(this AppDbContext _context, string query)
    //{
    //    using (_context)
    //    {
    //        using (var command = _context.Database.GetDbConnection().CreateCommand())
    //        {
    //            command.CommandText = query;
    //            command.CommandType = CommandType.Text;

    //            _context.Database.OpenConnection();

    //            using (var result = await command.ExecuteReaderAsync())
    //            {
    //                //if (typeof(T) is P_Productos)
    //                //{
    //                //    var entities = new PreencheEntidade<T>().GetEntidade(result);
    //                //    return 
    //                //}
    //                //if (typeof(T) is List<P_Productos>)
    //                //{
    //                var entities = new PreencheEntidade<T>().GetListaEntidade(result);
    //                return entities;
    //                //}


    //                //while (await result.ReadAsync())
    //                //{
    //                //    //var map =new Func<DbDataReader, T>();
    //                //    entities.Add(result.);
    //                //}
    //                //return entities;
    //            }
    //        }
    //    }
    //}

}

internal class PreencheEntidade<T> where T : class, new()
{
    public T GetEntidade(IDataReader dataReader)
    {
        T Entidade = default(T);

        if (dataReader.Read())
        {
            Entidade = new T();
            CarregaEntidade(dataReader, Entidade);
        }
        return Entidade;
    }

    public List<T> GetListaEntidade(IDataReader dataReader, Action<IDataReader, T> afterLoad = null)
    {
        var EntidadeColecao = new List<T>();

        while (dataReader.Read())
        {
            T Entidade = new T();
            CarregaEntidade(dataReader, Entidade);
            afterLoad?.Invoke(dataReader, Entidade);
            EntidadeColecao.Add(Entidade);
        }

        return EntidadeColecao;
    }

    private static Dictionary<Type, Dictionary<string, PropertyInfo>> DicionarioResumoClasses = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

    private void CarregaEntidade(IDataReader dataReader, T entidade)
    {
        var TipoEntidade = typeof(T);

        if (!DicionarioResumoClasses.ContainsKey(TipoEntidade))
        {
            lock (DicionarioResumoClasses)
            {
                if (!DicionarioResumoClasses.ContainsKey(TipoEntidade))
                {
                    var RegistroDePropriedadeColecao = TipoEntidade.GetProperties().AsEnumerable();
                    DicionarioResumoClasses.Add(TipoEntidade, RegistroDePropriedadeColecao.ToDictionary(x => x.Name, x => x));
                }
            }
        }

        var PropriedadeColecao = DicionarioResumoClasses[TipoEntidade];

        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            string NomeColuna = dataReader.GetName(i);
            PropertyInfo propriedade = PropriedadeColecao.ContainsKey(NomeColuna) ? PropriedadeColecao[NomeColuna] : null;

            if (propriedade != null)
            {
                object valor = dataReader.GetValue(i);

                if (System.DBNull.Equals(valor, DBNull.Value))
                {
                    if (propriedade.GetType().IsEnum)
                    {
                        valor = 0;
                    }
                    else
                    {
                        valor = null;
                    }
                }
                else
                {
                    var tipo = propriedade.PropertyType;
                    if (tipo.IsGenericType)
                    {
                        tipo = tipo.GenericTypeArguments[0];
                    }

                    if (tipo.IsEnum)
                    {
                        try
                        {
                            valor = System.Enum.Parse(tipo, valor.ToString());
                        }
                        catch
                        {
                            valor = 0;
                        }
                    }
                }

                propriedade.SetValue(entidade, valor, null);
            }
        }
    }
}