﻿using System;
using System.Linq;

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        public IOption[] CustomOptions { get; set; } = [];

        /// <summary>
        /// Adds a new option to the list of custom options of this OptionSet.
        /// </summary>
        /// <param name="optionString">The option flag.</param>
        /// <param name="value">The option value.</param>
        public void AddCustomOption<T>(string optionString, T value)
        {
            Option<T> option = new(true, optionString)
            {
                Value = value
            };
            CustomOptions = [.. CustomOptions, .. new[] { option }];
        }

        /// <summary>
        /// Sets the value of a custom option of this OptionSet.
        /// </summary>
        /// <param name="optionString">The option flag.</param>
        /// <param name="value">The option value.</param>
        public void SetCustomOption<T>(string optionString, T value)
        {
            foreach (var iOption in CustomOptions.Where(o => o.OptionStrings.Contains(optionString)))
            {
                if (iOption is Option<T> option)
                {
                    option.Value = value;
                }
                else
                {
                    throw new ArgumentException($"Value passed to option '{optionString}' has invalid type '{value.GetType()}'.");
                }
            }
        }

        /// <summary>
        /// Deletes a custom option from this OptionSet.
        /// </summary>
        /// <param name="optionString">The option flag of the option to delete.</param>
        public void DeleteCustomOption(string optionString)
        {
            CustomOptions = CustomOptions.Where(o => !o.OptionStrings.Contains(optionString)).ToArray();
        }
    }
}
