// ======================================================================================
//  File:		Diff.cs
//  Summary:	This is used to compare two arrays to each other and
//              determine the longest common sequence.
// ======================================================================================
//
//  This file is part of the Microsoft CRM 4.0 SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =======================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    /// <summary>
    /// The Diff class is used to compare two arrays to each other and 
    /// determine the longest common sequence.
    /// </summary>
    public class Diff
    {
        /// <summary>
        /// Compares the specified source array to the specified target array.
        /// Diff can compare any type of array, but is typically used for strings.
        /// If two strings are being compared, T will be System.Char and the 
        /// source and target arrays can be passed in using myString.ToCharArray().
        /// If lines in a larger body of text are being compared, T will be
        /// System.String and the arrays will be the text split into lines.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="target">The target array.</param>
        /// <returns></returns>
        public IEnumerable<DiffResult<T>> Compare<T>(T[] source, T[] target)
        {
            int sStart = 0;
            int sEnd = source.Length - 1;
            int tStart = 0;
            int tEnd = target.Length - 1;
            
            // To lower the memory footprint, attempt to trim off a common 
            // subsequence at the start of the arrays.
            List<DiffResult<T>> head = new List<DiffResult<T>>();
            while (sStart <= sEnd && tStart <= tEnd && Object.Equals(source[sStart], target[tStart]))
            {
                AppendResult(head, source[sStart], DiffOwner.Both);

                sStart++;
                tStart++;
            }

            // To lower the memory footprint, attempt to trim off a common 
            // subsequence at the end of the arrays.
            List<DiffResult<T>> tail = new List<DiffResult<T>>();
            while (sStart <= sEnd && tStart <= tEnd && Object.Equals(source[sEnd], target[tEnd]))
            {
                PrependResult(tail, source[sEnd], DiffOwner.Both);

                sEnd--;
                tEnd--;
            }

            // Extract the (hopefully) now smaller source and target arrays.
            List<DiffResult<T>> mid = new List<DiffResult<T>>();
            T[] trimmedSource = new T[sEnd - sStart + 1];
            Array.Copy(source, sStart, trimmedSource, 0, trimmedSource.Length);
            T[] trimmedTarget = new T[tEnd - tStart + 1];
            Array.Copy(target, tStart, trimmedTarget, 0, trimmedTarget.Length);

            // Do the actual work
            int[,] lengths = CalculateSubsequenceLengths<T>(trimmedSource, trimmedTarget);
            CalculateDiff(mid, lengths, trimmedSource, trimmedTarget, trimmedSource.Length - 1, trimmedTarget.Length - 1);

            // Merge our diff results with the previously trimmed head and tail.
            List<DiffResult<T>> results = new List<DiffResult<T>>();
            results.AddRange(head);
            results.AddRange(mid);
            results.AddRange(tail);

            return results;
        }

        /// <summary>
        /// A recursive method that calculates the difference between the two 
        /// arrays, given that the lengths have already been calculated.  The
        /// comparison starts at the end of both arrays and moves backwards to 
        /// the beginings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="diff">A list of DiffResult&ltT&gt to populate with 
        /// the diffs.</param>
        /// <param name="lengths">The linear distance between any two elements 
        /// in the source and target.</param>
        /// <param name="source">The source array.</param>
        /// <param name="target">The target array.</param>
        /// <param name="s">The current source index.</param>
        /// <param name="t">The current target index.</param>
        private static void CalculateDiff<T>(List<DiffResult<T>> diff, int[,] lengths, T[] source, T[] target, int s, int t)
        {
            // If we still haven't hit the begining of either array, and both current elements are equal...
            if (s >= 0 && t >= 0 && Object.Equals(source[s], target[t]))
            {
                // Move backwards in both arrays and record the current element as identical in both arrays.
                CalculateDiff(diff, lengths, source, target, s - 1, t - 1);
                AppendResult(diff, source[s], DiffOwner.Both);
            }
            else
            {
                // If we still haven't hit the begining of the target array and we're either done with the source
                // array or there is higher value when moving backwards in the target array...
                if (t >= 0 && (s < 0 || lengths[s + 1, t] >= lengths[s, t + 1]))
                {
                    // Move backwards in the target array and record the current element as only present in the target.
                    CalculateDiff(diff, lengths, source, target, s, t - 1);
                    AppendResult(diff, target[t], DiffOwner.Target);
                }
                // Otherwise, if we still haven't hit the begining of the source array and we're either done with the target
                // array or there is higher value when moving backwards in the source array...
                else if (s >= 0 && (t < 0 || lengths[s + 1, t] < lengths[s, t + 1]))
                {
                    // Move backwards in the source array and record the current element as only present in the source.
                    CalculateDiff(diff, lengths, source, target, s - 1, t);
                    AppendResult(diff, source[s], DiffOwner.Source);
                }
            }
        }

        /// <summary>
        /// Append a DiffResult&lt;T&gt; to a list, merging common results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="diff">The list of DiffResults.</param>
        /// <param name="value">The new value to add to the list.</param>
        /// <param name="owner">The owner (source, target or both) of the value.</param>
        private static void AppendResult<T>(List<DiffResult<T>> diff, T value, DiffOwner owner)
        {
            // If the last entry in the list has the same owner as the new value,
            // just append the new value to the old entry.
            if (diff.Count > 0)
            {
                DiffResult<T> lastResult = diff[diff.Count - 1];
                if (lastResult.Owner == owner)
                {
                    lastResult.Value.Add(value);
                    return;
                }
            }

            // Otherwise, just add a new entry to the list.
            DiffResult<T> result = new DiffResult<T>()
            {
                Owner = owner,
            };
            result.Value.Add(value);
            diff.Add(result);
        }

        /// <summary>
        /// Prepend a DiffResult&lt;T&gt; to a list, merging common results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="diff">The list of DiffResults.</param>
        /// <param name="value">The new value to add to the list.</param>
        /// <param name="owner">The owner (source, target or both) of the value.</param>
        private static void PrependResult<T>(List<DiffResult<T>> diff, T value, DiffOwner owner)
        {
            // If the first entry in the list has the same owner as the new value,
            // just prepend the new value to the old entry.
            if (diff.Count > 0)
            {
                DiffResult<T> firstResult = diff[0];
                if (firstResult.Owner == owner)
                {
                    firstResult.Value.Insert(0, value);
                    return;
                }
            }

            // Otherwise, just add a new entry to the start of the list.
            DiffResult<T> result = new DiffResult<T>()
            {
                Owner = owner,
            };
            result.Value.Add(value);

            diff.Insert(0, result);
        }

        /// <summary>
        /// Calculates the subsequence lengths.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="target">The target array.</param>
        private static int[,] CalculateSubsequenceLengths<T>(T[] source, T[] target)
        {
            // The length array is zero padded in the first column and row, so it
            // must be bigger by one in each dimension.
            int[,] lengths = new int[source.Length + 1, target.Length + 1];

            for (int s = 0; s < source.Length; s++)
            {
                for (int t = 0; t < target.Length; t++)
                {
                    // If the current items are identical...
                    if (Object.Equals(source[s], target[t]))
                    {
                        // record the new value as one larger than the value above and to the left.
                        lengths[s + 1, t + 1] = lengths[s, t] + 1;
                    }
                    else
                    {
                        // record either the value from the left or the value above, whichever is larger.
                        lengths[s + 1, t + 1] = Math.Max(lengths[s + 1, t], lengths[s, t + 1]);
                    }
                }
            }

            return lengths;
        }
    }
}
