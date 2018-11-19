angular.module("main")
    .service("iconService", function () {
        var iconTable = [
            { category: "Weapon", icon: "sword" },
            { category: "Food", icon: "chicken" },
            { category: "Sulfuras", icon: "sulfuras" },
            { category: "Backstage Passes", icon: "ticket" },
            { category: "Potion", icon: "potion" },
            { category: "Conjured", icon: "bolt" },
            { category: "Armor", icon: "armor" },
            { category: "Misc", icon: "bag" }
        ];

        var getIcon = function (inventoryItem) {
            var match = _.find(iconTable, function (entry) { return inventoryItem && entry.category === inventoryItem.category; });
            return match ? "res/images/" + match.icon + ".png" : null;
        };

        return {
            getIcon: getIcon
        };
    });