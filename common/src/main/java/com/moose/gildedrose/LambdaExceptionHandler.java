package com.moose.gildedrose;

import java.util.function.Function;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
@SuppressWarnings({"PMD.AvoidCatchingGenericException", "PMD.AvoidThrowingRawExceptionTypes"})
public class LambdaExceptionHandler {

	public static <T, R, E extends Exception> Function<T, R> rethrowFunction(final LambdaExceptionHandler.FunctionThrows<T, R, E> function) {
		return (t) -> {
			try {
				return function.apply(t);
			} catch (final Exception var3) {
				throwAsUnchecked(var3);
				return null;
			}
		};
	}

	private static <E extends Throwable> void throwAsUnchecked(final Exception exception) throws E {
		throw new RuntimeException(exception);
	}

	@FunctionalInterface
	public interface FunctionThrows<T, R, E extends Exception> {
		R apply(T var1) throws E;
	}

}
