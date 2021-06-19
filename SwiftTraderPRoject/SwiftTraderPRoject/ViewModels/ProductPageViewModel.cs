﻿using SwiftTraderPRoject.Models;
using SwiftTraderPRoject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTraderPRoject.ViewModels
{
    public class ProductPageViewModel : BaseViewModel
    {
        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value; OnPropertyChanged(); }
        }

        private int totalProductItem;
        public int TotalProductItem
        {
            get { return totalProductItem; }
            set { totalProductItem = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Products> ItemsByCategory { get; set; }

        public ProductPageViewModel(Category category)
        {
            SelectedCategory = category;
            ItemsByCategory = new ObservableCollection<Products>();
            GetProductItemsAsync(category.CatId);
        }
        public ProductPageViewModel() { }

        private async void GetProductItemsAsync(int catId)
        {
            var data = await new ProductServices().GetProductsByCategoryAsync(catId);
            ItemsByCategory.Clear();

            foreach (var item in data)
            {
                ItemsByCategory.Add(item);
            }

            TotalProductItem = ItemsByCategory.Count();
        }


    }
}
