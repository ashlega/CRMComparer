// ======================================================================================
//  File:		CustomizationComparison.cs
//  Summary:	Contains information about the differences between customization files.
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
using System.Reflection;

namespace Microsoft.Crm.Isv
{
    /// <summary>
    /// Contains information about the differences between customization files.
    /// </summary>
    public class CustomizationComparison
    {

        public CustomizationComparison ParentComparison { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name of the item being compared.</value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the source value.
        /// </summary>
        /// <value>The value of the item in the source file.</value>
        public Object SourceValue { get; set; }

        /// <summary>
        /// Gets or sets the target value.
        /// </summary>
        /// <value>The target value of the item in the target file.</value>
        public Object TargetValue { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is different, not including the children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is different; otherwise, <c>false</c>.
        /// </value>
        public bool IsDifferentBase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is different, including the children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is different; otherwise, <c>false</c>.
        /// </value>
        public bool IsDifferent { get; set; }

        /// <summary>
        /// Gets or sets the parent property.
        /// </summary>
        /// <value>The parent property that contains this item.</value>
        public PropertyInfo ParentProperty { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children items.</value>
        public List<CustomizationComparison> Children { get; private set; }

        public CustomizationComparison(string name, object source, object target, CustomizationComparison parentComparison)
            :this(parentComparison)
        {
            Name = name;
            SourceValue = source;
            TargetValue = target;
        }

        public CustomizationComparison(CustomizationComparison parentComparison)
        {
            Children = new List<CustomizationComparison>();
            ParentComparison = parentComparison;
            IsDifferentBase = false;
        }

        /// <summary>
        /// Gets the number of children that are identical in both the source and the target.
        /// </summary>
        /// <returns></returns>
        public int GetUnchangedCount()
        {
            return this.Children.Where(c => !c.IsDifferent).Count();
        }

        /// <summary>
        /// Gets the number of children that exist in both the source and target, but are different.
        /// </summary>
        /// <returns></returns>
        public int GetChangedCount()
        {
            return this.Children.Where(c => c.IsDifferent && c.SourceValue != null && c.TargetValue != null).Count();
        }

        /// <summary>
        /// Gets the number of children that exist solely in the target.
        /// </summary>
        /// <returns></returns>
        public int GetMissingInSourceCount()
        {
            return this.Children.Where(c => c.SourceValue == null).Count();
        }

        /// <summary>
        /// Gets the number of children that exist solely in the source.
        /// </summary>
        /// <returns></returns>
        public int GetMissingInTargetCount()
        {
            return this.Children.Where(c => c.TargetValue == null).Count();
        }

        public string GetFullPath()
        {
            string result = "";
            if (ParentComparison != null)
            {
                string parentPath = ParentComparison.GetFullPath();
                result = parentPath + (parentPath == "" ? "" : ".") + Name;
            }
            return result;
        }
    }
}
