﻿using Moq;

using NUnit.Framework;

using SQLiteKei.Util.Interfaces;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.ViewModels.MainWindow.DBTreeView;
using SQLiteKei.ViewModels.MainWindow.DBTreeView.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLiteKei.IntegrationTests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel viewModel;

        private TableItem LookupItem;

        private IndexFolderItem LookupItemParent;

        private DatabaseItem LookupItemDatabase;

        private TableItem ComparisonItem;

        private IndexFolderItem ComparisonItemParent;

        [SetUp]
        public void Setup()
        {
            LookupItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1"
            };

            LookupItemParent = new IndexFolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItem }
            };

            LookupItemDatabase = new DatabaseItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItemParent }
            };

            ComparisonItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2"
            };

            ComparisonItemParent = new IndexFolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2",
                Items = new ObservableCollection<TreeItem> { ComparisonItem }
            };

            var treeSaveHelperMock = new Mock<ITreeSaveHelper>();

            viewModel = new MainWindowViewModel(treeSaveHelperMock.Object)
            {
                TreeViewItems = new ObservableCollection<TreeItem>
                {
                    LookupItemDatabase,
                    new DatabaseItem
                    {
                        DisplayName = "Name",
                        DatabasePath = "Database2",
                        Items = new ObservableCollection<TreeItem>
                        {
                            ComparisonItemParent
                        }
                    }
                }
            };
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_RemovesSpecifiedItem()
        {
            viewModel.RemoveItemFromTree(LookupItem);

            var result = LookupItemParent.Items.Any();
            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromSameDatabase()
        {
            viewModel.RemoveItemFromTree(LookupItem);

            var result = LookupItemDatabase.Items.Any();
            Assert.IsTrue(result);
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromOtherDatabase()
        {
            viewModel.RemoveItemFromTree(LookupItem);

            var result = ComparisonItemParent.Items.Any();
            Assert.IsTrue(result);
        }
    }
}
