package com.moose.gildedrose.inventory;

import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.security.AccessController;
import java.security.PrivilegedAction;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.fail;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class ReflectionUtils {

	public static <T> Constructor<T> testPrivateConstructor(final Constructor<T> constructorUnderTest) {
		AccessController.doPrivileged(new PrivilegedAction<Constructor<T>>() {
			@Override
			public Constructor<T> run() {
				try {
					assertFalse(constructorUnderTest.canAccess(null));
					constructorUnderTest.setAccessible(true);
					assertNotEquals(constructorUnderTest.newInstance(), constructorUnderTest.newInstance());
					constructorUnderTest.setAccessible(false);
				} catch (final InstantiationException | IllegalAccessException | InvocationTargetException exception) {
					fail();
				}
				return constructorUnderTest;
			}
		});
		return constructorUnderTest;
	}
}
