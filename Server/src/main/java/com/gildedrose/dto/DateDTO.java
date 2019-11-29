package com.gildedrose.dto;

import java.time.LocalDate;

public class DateDTO {

	private LocalDate date;

	/* -- CONSTRUCTORS -- */

	public DateDTO() {
	}

	public DateDTO(LocalDate date) {
		this.date = date;
	}

	/* -- PUBLIC METHODS -- */

	public LocalDate getDate() {
		return date;
	}

	public void setDate(LocalDate date) {
		this.date = date;
	}
}
