// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 23.12.2021 12:29
// Entwickler      Manuel Fasching
// Projekt         DataVisualisation
//
// Released under MIT

using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVisualisation
{
    public class ThemeState
    {
        public string CurrentTheme { get; set; } = "standard";
    }

    public class MenuItemsService
    {
        bool added = false;
        public void AddProjectsToMenu(List<Project> projects)
        {
            if(added)
                return;

            added = true;
            var tmp = allExamples.ToList();

            var tmpChildren = new List<MenuEntry>();
            foreach (var item in projects)    
            {
                tmpChildren.Add(new MenuEntry(){Name = item.Name, Path = $"/ViewDataForProject/{item.Id}"});
            }

            tmp.Insert(2,new MenuEntry
                {
                    Name = "Projektbezogene Ansichten",
                    Icon = "&#xe7f2",
                    Children = tmpChildren.ToArray()
                });
            allExamples = tmp.ToArray();
        }

        MenuEntry[] allExamples = new[] {
            new MenuEntry()
            {
                Name = "Überblick",
                Path = "/",
                Icon = "&#xe88a"
            },
            new MenuEntry()
            {
                Name = "Physikalische Struktur",
                Path = "/ViewDataPhysics",
                Icon = "&#xe335"
            },
            new MenuEntry
            {
                Name = "Logische Ansichten",
                Path = "/ViewSettingsLogic",
                Icon = "&#xe8b8"
            },
            new MenuEntry
            {
                Name = "Statistik",
                Path = "/ViewStatistic",
                Icon = "&#xe8e5"
            }
        };

        public IEnumerable<MenuEntry> MenuEntries
        {
            get
            {
                return allExamples;
            }
        }

        public IEnumerable<MenuEntry> Filter(string term)
        {
            if (string.IsNullOrEmpty(term))
                return allExamples;

            bool contains(string value) => value.Contains(term, StringComparison.OrdinalIgnoreCase);

            bool filter(MenuEntry example) => contains(example.Name) || (example.Tags != null && example.Tags.Any(contains));

            bool deepFilter(MenuEntry example) => filter(example) || example.Children?.Any(filter) == true;

            return MenuEntries.Where(category => category.Children?.Any(deepFilter) == true)
                           .Select(category => new MenuEntry
                           {
                               Name = category.Name,
                               Expanded = true,
                               Children = category.Children.Where(deepFilter).Select(example => new MenuEntry
                               {
                                   Name = example.Name,
                                   Path = example.Path,
                                   Icon = example.Icon,
                                   Expanded = true,
                                   Children = example.Children
                               }
                               ).ToArray()
                           }).ToList();
        }

        public MenuEntry FindCurrent(Uri uri)
        {
            return MenuEntries.SelectMany(example => example.Children ?? new[] { example })
                           .FirstOrDefault(example => example.Path == uri.AbsolutePath || $"/{example.Path}" == uri.AbsolutePath);
        }

    }
}