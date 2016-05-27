﻿#region usings

using SQLiteKei.Exceptions.Queries;
using SQLiteKei.Queries.Data;
using SQLiteKei.Queries.Enums;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

#endregion

namespace SQLiteKei.Queries.Builders
{
    public class CreateQueryBuilder : QueryBuilder
    {
        private List<ColumnData> Columns { get; set; } 

        public CreateQueryBuilder(string table)
        {
            this.table = table;
            Columns = new List<ColumnData>();
        }

        public CreateQueryBuilder AddColumn(string columnName, DataType dataType, bool isPrimary = false, bool isNullable = false)
        {
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new QueryBuilderException("Create query could not be built. A provided column name was null or empty.");
            }

            CheckIfColumnAlreadyAdded(columnName);

            var columnData = new ColumnData
            {
                ColumnName = columnName,
                DataType = dataType,
                IsPrimary = isPrimary,
                IsNullable = isNullable
            };

            Columns.Add(columnData);
            return this;
        }

        private void CheckIfColumnAlreadyAdded(string columnName)
        {
            var result = Columns.Find(c => c.ColumnName.Equals(columnName));

            if(result != null)
            {
                throw new QueryBuilderException("Create query could not be built. A column has was provided multiple times.");
            }
        }

        public override string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
            {
                throw new QueryBuilderException("Create query could not be built. An empty or invalid table name has been provided.");
            }

            if(!Columns.Any())
            {
                throw new QueryBuilderException("Create query could not be built. No columns were provided.");
            }

            string columns = string.Join(",\n", Columns);

            return string.Format("CREATE TABLE {0}\n(\n{1}\n);", table, columns);
        }
    }
}
