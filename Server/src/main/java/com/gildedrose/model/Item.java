package com.gildedrose.model;

import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
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
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private long id;

	@Column(nullable = false)
	private int sellIn;

	@Column(nullable = false)
	private int quality;

	@Column(nullable = true)
	private LocalDate discardedDate;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "DEFINITION_ID", nullable = false, updatable = false)
	private ItemDefinition definition;

	/* -- PUBLIC METHODS -- */

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public int getSellIn() {
		return sellIn;
	}

	public void setSellIn(int sellIn) {
		this.sellIn = sellIn;
	}

	public int getQuality() {
		return quality;
	}

	public void setQuality(int quality) {
		this.quality = quality;
	}

	public LocalDate getDiscardedDate() {
		return discardedDate;
	}

	public void setDiscardedDate(LocalDate discardedDate) {
		this.discardedDate = discardedDate;
	}

	public ItemDefinition getDefinition() {
		return definition;
	}

	public void setDefinition(ItemDefinition definition) {
		this.definition = definition;
	}
}
