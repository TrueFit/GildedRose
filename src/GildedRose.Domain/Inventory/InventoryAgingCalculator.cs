#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Data;
using GildedRose.Entities.Inventory.Aging;

#endregion

namespace GildedRose.Domain.Inventory
{
    /// <summary>
    /// Utility class for performing aging (adjusting the SellIn and Quality) of an <see cref="Entities.Inventory.Inventory" /> item.
    /// </summary>
    public class InventoryAgingCalculator
    {
        private List<BaseAgingRule> _rules = null;
        private IDataContext _dataContext;

        public InventoryAgingCalculator(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Age an <see cref="Entities.Inventory.Inventory" /> item by one
        /// day by applying aging with the appropriate aging rule.
        /// </summary>
        /// <param name="item"></param>
        public void AgeInventory(Entities.Inventory.Inventory item)
        {
            // We are lazy loading the rules if not set
            if (_rules == null)
            {
                LoadRules();
            }

            var agingRule = GetAgingRule(item);

            agingRule.ApplyAging(item);
        }

        #region Private Helpers

        /// <summary>
        /// Inventory aging rules are matched by the following priority (from most to least specific):
        /// 1) The rule having ItemCategory = item.Category and ItemName = item.Name
        /// 2) The rule having ItemCategory = item.Category
        /// 3) The rule having ItemCategory = null and ItemName = null (this equates to the default rule)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private BaseAgingRule GetAgingRule(Entities.Inventory.Inventory item)
        {
            var matchingRule = _rules.FirstOrDefault(r => r.ItemCategory == item.Category && r.ItemName == item.Name);
            if (matchingRule != null) return matchingRule; // specific item rule found

            matchingRule = _rules.FirstOrDefault(r => r.ItemCategory == item.Category && r.ItemName == null);
            if (matchingRule != null) return matchingRule; // item category rule found

            matchingRule = _rules.FirstOrDefault(r => r.ItemCategory == null && r.ItemName == null);
            if (matchingRule != null) return matchingRule; // generic rule found

            // Otherwise we don't have a matching rule so throw an exception
            throw new ApplicationException("No matching inventory aging rule exists and no default rule is defined.");
        }

        private void LoadRules()
        {
            _rules = _dataContext.AgingRules.ToList();

            if (_rules == null || _rules.Count == 0)
            {
                throw new ApplicationException("No inventory aging rules found");
            }
        }

        #endregion
    }
}
