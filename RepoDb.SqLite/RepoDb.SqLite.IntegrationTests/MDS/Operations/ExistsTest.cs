﻿using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Enumerations;
using RepoDb.SqLite.IntegrationTests.Models;
using RepoDb.SqLite.IntegrationTests.Setup;
using System;
using System.Linq;

namespace RepoDb.SqLite.IntegrationTests.Operations.MDS
{
    [TestClass]
    public class ExistsTest
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

        #region DataEntity

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionExistsWithoutExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists<MdsCompleteTable>((object)null);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var ids = new[] { tables.First().Id, tables.Last().Id };

                // Act
                var result = connection.Exists<MdsCompleteTable>(e => ids.Contains(e.Id));

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaDynamic()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists<MdsCompleteTable>(new { tables.First().Id });

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaQueryField()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists<MdsCompleteTable>(new QueryField("Id", tables.First().Id));

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaQueryFields()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };

                // Act
                var result = connection.Exists<MdsCompleteTable>(queryFields);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaQueryGroup()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.Exists<MdsCompleteTable>(queryGroup);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionOnSqLiteConnectionExistsWithHints()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                connection.Exists<MdsCompleteTable>((object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncWithoutExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>((object)null).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var ids = new[] { tables.First().Id, tables.Last().Id };

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>(e => ids.Contains(e.Id)).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaDynamic()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>(new { tables.First().Id }).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaQueryField()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>(new QueryField("Id", tables.First().Id)).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaQueryFields()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>(queryFields).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaQueryGroup()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.ExistsAsync<MdsCompleteTable>(queryGroup).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionOnSqLiteConnectionExistsAsyncWithHints()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                connection.ExistsAsync<MdsCompleteTable>((object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion

        #region TableName

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionExistsViaTableNameWithoutExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    (object)null);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaTableNameViaDynamic()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    new { tables.First().Id });

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaTableNameViaQueryField()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    new QueryField("Id", tables.First().Id));

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaTableNameViaQueryFields()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };

                // Act
                var result = connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    queryFields);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsViaTableNameViaQueryGroup()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    queryGroup);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionOnSqLiteConnectionExistsViaTableNameWithHints()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                connection.Exists(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    (object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaTableNameWithoutExpression()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    (object)null).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaTableNameViaDynamic()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    new { tables.First().Id }).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaTableNameViaQueryField()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                var result = connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    new QueryField("Id", tables.First().Id)).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaTableNameViaQueryFields()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };

                // Act
                var result = connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    queryFields).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionExistsAsyncViaTableNameViaQueryGroup()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);
                var queryFields = new[]
                {
                    new QueryField("Id", Operation.GreaterThan, tables.First().Id),
                    new QueryField("Id", Operation.LessThan, tables.Last().Id)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    queryGroup).Result;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionOnSqLiteConnectionExistsAsyncViaTableNameWithHints()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection);

                // Act
                connection.ExistsAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    (object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion
    }
}
