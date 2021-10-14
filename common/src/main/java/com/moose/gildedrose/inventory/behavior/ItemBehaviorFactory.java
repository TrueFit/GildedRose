package com.moose.gildedrose.inventory.behavior;

import com.moose.gildedrose.inventory.model.ItemCategoryType;
import com.moose.gildedrose.inventory.model.RawInventoryItem;
import java.util.Map;
import java.util.function.Function;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;

/**
 * A factory pattern for discerning the correct {@link ItemBehavior} for a given {@link RawInventoryItem}.
 * The {@link ItemBehaviorFactory#ITEM_TO_BEHAVIOR} map controls all the logic for what {@link RawInventoryItem}s resolve
 * to a particular {@link ItemBehavior}.
 *
 * The map requires some arbitrary evaluation of a {@link Function} containing a {@link RawInventoryItem} to resolve to a {@link Boolean},
 * coupled to a specific implementation of {@link ItemBehavior}. That is to say, there should be some determination made about
 * a {@link RawInventoryItem} that distinctly identifies it as a specific {@link ItemBehavior}. In this event, the {@link Function}
 * should return {@code true}, otherwise it will return false.
 *
 * In cases where no evaluations return {@code true}, the {@link ItemBehaviorFactory} will return a {@link DefaultBehavior}.
 *
 * While this implementation boils down all item aging calculation decisions into this neat little map, it does so with
 * 2 very import caveats:
 * <ol>
 *     <li>An {@link RawInventoryItem} will only ever have a single unique {@link ItemBehavior}.</li>
 *     <li>The order evaluation of the {@link Function}s should not be guaranteed, no {@link RawInventoryItem} should
 *     ever satisfy more than one of the criteria specified in the {@link ItemBehaviorFactory#ITEM_TO_BEHAVIOR} map.</li>
 * </ol>
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class ItemBehaviorFactory {
    private static final Map<Function<RawInventoryItem, Boolean>, ItemBehavior> ITEM_TO_BEHAVIOR = Map.of(
            inv -> ItemCategoryType.CONJURED.equals(inv.getCategory()), new ConjuredBehavior(),
            inv -> ItemCategoryType.FOOD.equals(inv.getCategory()) && "Aged Brie".equalsIgnoreCase(inv.getName()), new AgingBehavior(),
            inv -> ItemCategoryType.LEGENDARY.equals(inv.getCategory()), new LegendaryBehavior(),
            inv -> ItemCategoryType.BACKSTAGE_PASS.equals(inv.getCategory()), new BackstageBehavior()
    );

    /**
     * Function for actually retrieving the appropriate {@link ItemBehavior} from the {@link ItemBehaviorFactory}.
     * @param rawInventoryItem The {@link RawInventoryItem} that is to be evaluated to a specific {@link ItemBehavior}.
     * @return The appropriate {@link ItemBehavior} for the given {@link RawInventoryItem}, based on the {@link ItemBehaviorFactory#ITEM_TO_BEHAVIOR} mapping.
     */
    public static ItemBehavior getItemBehavior(final RawInventoryItem rawInventoryItem) {
        return ItemBehaviorFactory.ITEM_TO_BEHAVIOR.entrySet().stream()
                .filter(itemBehaviorSet -> itemBehaviorSet.getKey().apply(rawInventoryItem))
                .findFirst()
                .map(Map.Entry::getValue)
                .orElseGet(DefaultBehavior::new);
    }
}
