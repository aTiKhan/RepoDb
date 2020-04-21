﻿using RepoDb.Attributes;
using RepoDb.IntegrationTests.Enumerations;
using System;
using System.Data;

namespace RepoDb.IntegrationTests.Models
{
    [Map("[dbo].[CompleteTable]")]
    public class EnumCompleteTable
    {
        public Guid SessionId { get; set; }
        public BooleanValue? ColumnBit { get; set; }
        public Direction? ColumnNVarChar { get; set; }
        public Direction? ColumnInt { get; set; }
        public Direction? ColumnBigInt { get; set; }
        public Direction? ColumnSmallInt { get; set; }
    }

    [Map("[dbo].[CompleteTable]")]
    public class EnumAsIntForStringCompleteTable
    {
        public Guid SessionId { get; set; }
        [TypeMap(DbType.Int32)]
        public Direction? ColumnNVarChar { get; set; }
    }

    [Map("[dbo].[CompleteTable]")]
    public class TypeLevelMappedForStringEnumCompleteTable
    {
        public Guid SessionId { get; set; }
        public Continent? ColumnNVarChar { get; set; }
    }
}
