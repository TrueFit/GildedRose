package com.gildedrose.model;

import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

/**
 * Represents an instance of an actual item.
 */
@Entity
@Table(name = "ITEMS")
public class Item {

	@Id
	@GeneratedValue
	private long id;

	@Column(nullable = false)
	private int quality;

	@Column(nullable = false)
	private LocalDate sellByDate;

	@Column(nullable = false)
	private LocalDate qualityLastCalculatedDate;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "DEFINITION_ID")
	private ItemDefinition definition;

	/* -- PUBLIC METHODS -- */

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public int getQuality() {
		return quality;
	}

	public void setQuality(int quality) {
		this.quality = quality;
	}

	public LocalDate getSellByDate() {
		return sellByDate;
	}

	public void setSellByDate(LocalDate sellByDate) {
		this.sellByDate = sellByDate;
	}

	public LocalDate getQualityLastCalculatedDate() {
		return qualityLastCalculatedDate;
	}

	public void setQualityLastCalculatedDate(LocalDate qualityLastCalculatedDate) {
		this.qualityLastCalculatedDate = qualityLastCalculatedDate;
	}

	public ItemDefinition getDefinition() {
		return definition;
	}

	public void setDefinition(ItemDefinition definition) {
		this.definition = definition;
	}
}
