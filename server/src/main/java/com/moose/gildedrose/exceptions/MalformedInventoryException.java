package com.moose.gildedrose.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.BAD_REQUEST)
public class MalformedInventoryException extends RuntimeException {
	private static final long serialVersionUID = 1L;

	public MalformedInventoryException(final String message, final Throwable cause) {
		super(message, cause);
	}
}
