package com.gildedrose.dto;

import com.fasterxml.jackson.databind.annotation.JsonSerialize;

/**
 * Used by REST APIs which don't return any data. Ensures the response body is
 * still valid JSON.
 */
@JsonSerialize // allows serialization of an empty object
public class EmptyDTO {

}
