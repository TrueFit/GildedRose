package com.moose.gildedrose.inventory.behavior;

import com.moose.gildedrose.inventory.ArgumentsProviders;
import java.util.stream.Stream;
import static org.junit.jupiter.api.Assertions.assertEquals;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.ArgumentsSource;
import org.junit.jupiter.params.provider.MethodSource;

class AgingBehaviorTest {
    private final ItemBehavior itemBehavior = new AgingBehavior();

    @ParameterizedTest
    @MethodSource("getStartingValuesAndExpectedQuality")
    void testCalculateQuality(final Integer startingSellByDays, final Integer startingQuality, final Integer expectedQuality) {
        assertEquals(expectedQuality, this.itemBehavior.calculateQuality(startingQuality, startingSellByDays));
    }

    @ParameterizedTest
    @ArgumentsSource(ArgumentsProviders.SellByDaysCalculationProvider.class)
    void testCalculateSellByDays(final Integer startingSellByDays, final Integer expectedSellByDays) {
        assertEquals(expectedSellByDays, this.itemBehavior.calculateSellByDays(startingSellByDays));
    }

    private static Stream<Arguments> getStartingValuesAndExpectedQuality() {
        return Stream.of(
                // SellByDays, Quality, Expected Result
                Arguments.of(7, 50, 50),
                Arguments.of(4, 25, 26),
                Arguments.of(3, 30, 31),
                Arguments.of(5, 0, 1),
                Arguments.of(0, 50, 0),
                Arguments.of(-1, 45, 0),
                Arguments.of(-1, 30, 0),
                Arguments.of(-3, 0, 0)
        );
    }
}
