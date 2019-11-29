package com.gildedrose;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * Exception thrown from within the scope of controller methods to indicate the
 * request was not performed correctly. Causes the HTTP status to be set to 400
 * (BAD REQUEST).
 */
@SuppressWarnings("serial")
@ResponseStatus(code = HttpStatus.BAD_REQUEST)
public class InvocationException extends RuntimeException {

	/* -- CONSTRUCTORS -- */
	public InvocationException(String message) {
		super(message);
	}
}
