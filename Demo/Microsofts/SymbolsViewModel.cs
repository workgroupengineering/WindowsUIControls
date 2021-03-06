﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Demo.Models;
using Demo.ViewModel;

using Windows.UI.Xaml.Controls;

namespace Demo.Microsofts
{
    public class SymbolsViewModel : BaseViewModel
    {
        public ObservableCollection<object> Filters;
        public ObservableCollection<ItemsGroup> Groups;

        private bool matchCase = false;
        private bool matchOrder = true;

        public SymbolsViewModel()
        {
            Groups = new ObservableCollection<ItemsGroup>();
            Filters = new ObservableCollection<object>();
        }

        public bool MatchCase
        {
            get => matchCase;
            set => SetProperty(ref matchCase, value);
        }
        public bool MatchOrder
        {
            get => matchOrder;
            set => SetProperty(ref matchOrder, value);
        }

        public async Task LoadItems()
        {
            IsBusy = true;

            Groups.Clear();
            await Task.Delay(100);
            var syms = Enum.GetValues(typeof(Symbol));
            var list = new List<object>();
            foreach (var item in syms)
            {
                Filters.Add(item);
                list.Add(item);
            }
            var groups = from sym in list
                         group sym by sym.ToString().Substring(0, 1) into g
                         orderby g.Key
                         select new ItemsGroup(g);

            foreach (var item in groups)
            {
                Groups.Add(item);
            }

            IsBusy = false;
        }

        public void FilterChanged(string filterText)
        {
            Filters.Clear();
            filterText = filterText.Trim();
            if (!matchCase)
                filterText = filterText.ToLower();

            var filters = filterText.Split(' ');

            foreach (var item in Groups)
            {
                foreach (var s in item.Items)
                {
                    var found = filters.Length < 1;

                    if (!found)
                    {
                        var str = s.ToString();
                        if (!matchCase)
                            str = str.ToLower();

                        if (matchOrder)
                        {
                            string patten = @"\S*";
                            foreach (var f in filters)
                                patten += f + @"\S*";
                            found = Regex.IsMatch(str, patten);
                        }
                        else
                        {
                            found = filters.All((key) =>
                            {
                                return str.Contains(key);
                            });
                        }
                    }

                    if (found)
                        Filters.Add((Symbol)s);
                }
            }
            if (Filters.Count == 0)
                Filters.Add("Not Found");
        }
    }
}
