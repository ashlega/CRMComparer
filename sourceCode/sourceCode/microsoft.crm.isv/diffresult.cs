// =====================================================================
//  File:		DiffResult.cs
//  Summary:	Contains information about subsequence results from diff comparison.
// =====================================================================
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
// =====================================================================

using System.Collections.Generic;

namespace Microsoft.Crm.Isv
{
    /// <summary>
    /// Contains information about subsequence results from diff comparison.
    /// </summary>
    /// <typeparam name="T">The type of elements compared in the diff.</typeparam>
    public class DiffResult<T>
    {
        public DiffResult()
        {
            _value = new List<T>();
        }
        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        /// <value>A list of elements that appeared together in at least one 
        /// of the two original arrays.</value>
        List<T> _value;
        public IList<T> Value { get { return _value; } }

        /// <summary>
        /// Gets or sets the owner of the values.
        /// </summary>
        /// <value>Used to determine which array the list of values is from (source, target or both).</value>
        public DiffOwner Owner { get; set; }

        public void AddValues(IEnumerable<T> values)
        {
            _value.AddRange(values);
        }
    }
}
