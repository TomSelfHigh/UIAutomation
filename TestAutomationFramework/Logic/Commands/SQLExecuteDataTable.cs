﻿using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Logic.Commands
{
    public class SQLExecuteDataTable : SQLExecute
    {
        public SQLExecuteDataTable(String id, String description, KeyValuePair<String, Tuple<Type, String>> output, Dictionary<String, Tuple<Type, String>> parameters
            , String database)
            : base(id, description, output, parameters, database)
        {

        }

        public override void Run(TestContainer container)
        {
            FileConfigurationSource dataSource = new FileConfigurationSource(container._configFilePath);
            DatabaseProviderFactory factory = new DatabaseProviderFactory(dataSource);
            Database sqlDB = factory.Create(this.Database);

            var sqlStatement = this.Parameters["Statement"].Item2;

            var ds = sqlDB.ExecuteDataSet(CommandType.Text, sqlStatement);
            if (String.IsNullOrEmpty(this.Output.Key) == false)
            {
                var json = ds.Tables[0].SerializeToJSon();
                this.Output = new KeyValuePair<String, Tuple<Type, String>>(this.Output.Key, new Tuple<Type, String>(typeof(List<Dictionary<String, String>>), json));
            }
            this.PassTest = true;
        }

        public new SQLExecuteDataTable DeepCopy()
        {
            var tmpDatabase = this.Database;
            SQLExecuteDataTable other = new SQLExecuteDataTable(this.Id, this.Description, new KeyValuePair<String, Tuple<Type, String>>(), new Dictionary<String, Tuple<Type, String>>(), tmpDatabase);
            other.Output = new KeyValuePair<String, Tuple<Type, String>>(this.Output.Key
                , this.Output.Value == null ?
                new Tuple<Type, String>(typeof(String), String.Empty) :
                new Tuple<Type, String>(this.Output.Value.Item1, this.Output.Value.Item2));
            other.Parameters = new Dictionary<String, Tuple<Type, String>>(this.Parameters);
            return other;
        }
    }
}
