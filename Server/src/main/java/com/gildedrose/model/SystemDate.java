package com.gildedrose.model;

import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

/**
 * Holds a named date used by the system.
 */
@Entity
@Table(name = "SYSTEM_DATES")
public class SystemDate {

	@Id
	@Column(length = 40)
	private String id;

	@Column(nullable = false)
	private LocalDate date;

	/* -- PUBLIC METHODS -- */

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public LocalDate getDate() {
		return date;
	}

	public void setDate(LocalDate date) {
		this.date = date;
	}
}
