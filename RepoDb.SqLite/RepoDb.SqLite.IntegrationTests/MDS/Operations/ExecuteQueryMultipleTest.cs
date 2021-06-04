﻿using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Extensions;
using RepoDb.SqLite.IntegrationTests.Models;
using RepoDb.SqLite.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepoDb.SqLite.IntegrationTests.Operations.MDS
{
    [TestClass]
    public class ExecuteQueryMultipleTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultiple()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultiple(@"SELECT * FROM [MdsCompleteTable];
                    SELECT * FROM [MdsCompleteTable];"))
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        Assert.AreEqual(tables.Count(), item.Count());
                        tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, item.First(e => e.Id == table.Id)));
                    });
                }
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultipleWithParameters()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultiple(@"SELECT * FROM [MdsCompleteTable] WHERE Id = @Id1;
                    SELECT * FROM [MdsCompleteTable] WHERE Id = @Id2;",
                    new
                    {
                        Id1 = tables.First().Id,
                        Id2 = tables.Last().Id
                    }))
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        item.AsList().ForEach(current => Helper.AssertPropertiesEquality(current, tables.First(e => e.Id == current.Id)));
                    });
                }
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultipleWithSharedParameters()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultiple(@"SELECT * FROM [MdsCompleteTable] WHERE Id = @Id;
                    SELECT * FROM [MdsCompleteTable] WHERE Id = @Id;",
                    new { Id = tables.Last().Id }))
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        item.AsList().ForEach(current => Helper.AssertPropertiesEquality(current, tables.First(e => e.Id == current.Id)));
                    });
                }
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultipleAsync()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultipleAsync(@"SELECT * FROM [MdsCompleteTable];
                    SELECT * FROM [MdsCompleteTable];").Result)
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        Assert.AreEqual(tables.Count(), item.Count());
                        tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, item.First(e => e.Id == table.Id)));
                    });
                }
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultipleAsyncWithParameters()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultipleAsync(@"SELECT * FROM [MdsCompleteTable] WHERE Id = @Id1;
                    SELECT * FROM [MdsCompleteTable] WHERE Id = @Id2;",
                    new
                    {
                        Id1 = tables.First().Id,
                        Id2 = tables.Last().Id
                    }).Result)
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        item.AsList().ForEach(current => Helper.AssertPropertiesEquality(current, tables.First(e => e.Id == current.Id)));
                    });
                }
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExecuteQueryMultipleAsyncWithSharedParameters()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                using (var extractor = connection.ExecuteQueryMultipleAsync(@"SELECT * FROM [MdsCompleteTable] WHERE Id = @Id;
                    SELECT * FROM [MdsCompleteTable] WHERE Id = @Id;",
                    new { Id = tables.Last().Id }).Result)
                {
                    var list = new List<IEnumerable<MdsCompleteTable>>();

                    // Act
                    list.Add(extractor.Extract<MdsCompleteTable>());
                    list.Add(extractor.Extract<MdsCompleteTable>());

                    // Assert
                    list.ForEach(item =>
                    {
                        item.AsList().ForEach(current => Helper.AssertPropertiesEquality(current, tables.First(e => e.Id == current.Id)));
                    });
                }
            }
        }

        #endregion
    }
}
