using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;

namespace MAF.Helpers
{
    /// <summary>
    /// Class to prevent multiple dictionary loading.
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary
    {
        /// <summary>
        /// Internal cache of loaded dictionaries 
        /// </summary>
        public static readonly Dictionary<Uri, ResourceDictionary> SharedDictionaries =
            new Dictionary<Uri, ResourceDictionary>();

        private string designSource;

        /// <summary>
        /// Local member of the source uri
        /// </summary>
        private Uri sharedSource;

        public string DesignSource
        {
            get { return designSource; }
            set
            {
                designSource = value;

                if (Execute.InDesignMode)
                {
                    var dict = Application.LoadComponent(new Uri(designSource, UriKind.Relative)) as ResourceDictionary;
                    MergedDictionaries.Add(dict);
                }
            }
        }

        public new Uri Source
        {
            get
            {
                if (Execute.InDesignMode)
                {
                    return base.Source;
                }
                return sharedSource;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (Execute.InDesignMode)
                {
                    return;
                }

                sharedSource = value;
                if (!SharedDictionaries.ContainsKey(value))
                {
                    base.Source = value;
                    SharedDictionaries.Add(value, this);
                }
                else
                {
                    MergedDictionaries.Add(SharedDictionaries[value]);
                }
            }
        }
    }
}