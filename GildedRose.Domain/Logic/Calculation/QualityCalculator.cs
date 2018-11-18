using GildedRose.Domain.Logic.Modifiers;
using GildedRose.Domain.Models;
using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Logic.Calulation
{
    public class QualityCalculator : ICalculator
    {
        private InventoryItemValue _item { get; }

        private readonly List<IItemModifier> _modifiers;

        private int _qualityValue;

        public QualityCalculator(InventoryItemValue item)
        {
            _item = item;
            _modifiers = new List<IItemModifier>()
            {
                new BaseModifier(),
                new LinearStagedInceaseModifier(),
                new DoubleDegradation(),
                new MaximumModifier(),
                new MinimumModifier(),
                new PasNoSellInHasNoValueModifier()
            };
        }

        public int Calculate()
        {
            ApplyModifiers();

            return _qualityValue;
        }

        private void ApplyModifiers()
        {
            foreach (var mod in _modifiers)
            {
                _qualityValue = mod.Apply(_item, _qualityValue);
            }
        }
    }
}
