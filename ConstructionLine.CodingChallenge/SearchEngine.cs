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
                ColorCounts = new List<ColorCount>() 
                { 
                    new ColorCount{ Color = Color.Red, Count=0},
                    new ColorCount{ Color = Color.Black, Count=0},
                    new ColorCount{ Color = Color.Blue, Count=0},
                    new ColorCount{ Color = Color.White, Count=0},
                    new ColorCount{ Color = Color.Yellow, Count=0},
                },
                SizeCounts = new List<SizeCount>()
                {
                    new SizeCount{Size = Size.Large, Count =0},
                    new SizeCount{Size = Size.Medium, Count =0},
                    new SizeCount{Size = Size.Small, Count =0}
                }
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

                    searchResult.ColorCounts.FirstOrDefault(c => c.Color.Name == color.Name).Count += _shirtsDictionary[dictKey].Count;
                    searchResult.SizeCounts.FirstOrDefault(c => c.Size.Name == size.Name).Count += _shirtsDictionary[dictKey].Count;
                   
                }
                
            }

            return searchResult;

        }
    }
}