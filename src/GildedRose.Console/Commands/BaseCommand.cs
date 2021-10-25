using System;
using GildedRose.Inventory.Interfaces;

namespace GildedRose.Console.Commands
{
    abstract class BaseCommand : ICommand
    {
        protected IInventoryList Inventory { get; }

        public BaseCommand(IInventoryList inventory)
        {
            Inventory = inventory;
        }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public virtual bool IsContinued => true;

        public abstract string Execute(string args);
    }
}

