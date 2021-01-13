using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private Dictionary<string, ShirtsCount> _shirtsDictionary;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _shirtsDictionary = ConstructDictionary(shirts);

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }

        public Dictionary<string, ShirtsCount> ConstructDictionary(List<Shirt> shirts)
        {
            var result = new Dictionary<string, ShirtsCount>();
            shirts.ForEach(s =>
            {
                var key = s.Size.Name + s.Color.Name;
                if (!result.ContainsKey(key))
                {
                    result.Add(key, new ShirtsCount{Shirts = new List<Shirt> { s }, Count=1 });
                }
                else
                {
                    result[key].Count+=1;
                    result[key].Shirts.Add(s);
                }
            });
            return result;
        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            if (options == null)
                throw new ArgumentException("options parm cannot be null");

            
            var searchResult = new SearchResults()
            {
                Shirts= new List<Shirt>(),
                ColorCounts = new List<ColorCount>(),
                SizeCounts = new List<SizeCount>()
            };

            string dictKey = "";

            if (options.Sizes.Count == 0)
            {
                options.Sizes = Size.All;
            }
            if (options.Colors.Count == 0)
            {
                options.Colors = Color.All;
            }

            foreach (Size size in options.Sizes)
            {
                foreach (Color color in options.Colors)
                {
                    dictKey =size.Name+color.Name;
                    if (!_shirtsDictionary.ContainsKey(dictKey))
                    {
                        continue;
                    }

                    searchResult.Shirts.AddRange(_shirtsDictionary[dictKey].Shirts);

                    var foundColor = searchResult.ColorCounts.FirstOrDefault(c => c.Color.Name == color.Name);

                    if ( foundColor!= null)
                    {
                        foundColor.Count += _shirtsDictionary[dictKey].Count;
                    }
                    else
                    {
                        searchResult.ColorCounts.Add(new ColorCount
                        {
                            Count = _shirtsDictionary[dictKey].Count,
                            Color = color
                        });
                    }

                    var foundSize = searchResult.SizeCounts.FirstOrDefault(c => c.Size.Name == size.Name);

                    if (foundSize != null)
                    {
                        foundSize.Count += _shirtsDictionary[dictKey].Count;
                    }
                    else
                    {
                        searchResult.SizeCounts.Add(new SizeCount
                        {
                            Count = _shirtsDictionary[dictKey].Count,
                            Size = size
                        });
                    }
                }
                
            }


            Color.All.ForEach(c =>
            {
                if (searchResult.ColorCounts.FirstOrDefault(color => color.Color.Name == c.Name) == null)
                {
                    searchResult.ColorCounts.Add(new ColorCount { Color = c, Count = 0 });
                }
            });

            Size.All.ForEach(s =>
            {
                if (searchResult.SizeCounts.FirstOrDefault(size => size.Size.Name == s.Name) == null)
                {
                    searchResult.SizeCounts.Add(new SizeCount { Size = s, Count = 0 });
                }
            });

            return searchResult;

        }
    }
}